using SparseMatrixAlgebra.Common.Exceptions;
using Element = SparseMatrixAlgebra.Sparse.CSR.SparseVector.Element;

namespace SparseMatrixAlgebra.Sparse.CSR;

public partial class SparseMatrixCsr
{
    /// <summary>
    /// LU-разложение с выбором ведущего элемента по столбцу
    /// </summary>
    public override SparseLUCsr LuFactorize()
    {
        if (Rows != Columns) throw new IncompatibleDimensionsException("Matrix must be square");
        
        SparseMatrixCsr L = new SparseMatrixCsr(Rows, Columns);
        SparseMatrixCsr U = this.Copy();
        stype[] P = new stype[Rows];
        for (stype i = 0; i < P.Length; ++i)
            P[i] = i + 1;

        for (stype i = 1; i < Rows; ++i)
        {
            // find max
            stype maxIndex = 0;
            vtype maxAbsValue = 0;
            for (stype j = i - 1; j < Rows; ++j)
            {
                var rowVector = U.GetRowAsVector(j);
                if (rowVector.NumberOfNonzeroElements == 0) continue;
                if (rowVector.GetIndexAt(0) != i - 1) continue;

                vtype absValue = Math.Abs(rowVector.GetValueAt(0));
                if (absValue > maxAbsValue)
                {
                    maxIndex = j;
                    maxAbsValue = absValue;
                }
            }

            if (maxAbsValue == 0) // maxAbsValue.IsZero() ?
                throw new SingularMatrixException("Matrix must be non-singular");

            // eliminate
            vtype pivotingValue = U.GetRowAsVector(maxIndex).GetValueAt(0);
            
            L.SwapRows(i, maxIndex + 1);
            U.SwapRows(i, maxIndex + 1);
            (P[i - 1], P[maxIndex]) = (P[maxIndex], P[i - 1]);

            for (stype j = i; j < Rows; ++j)
            {
                var rowVectorL = L.GetRowAsVector(j);
                var rowVectorU = U.GetRowAsVector(j);

                Element firstElement = rowVectorU[0];
                if (firstElement.Index == i - 1)
                {
                    vtype coef = firstElement.Value / pivotingValue;
                    rowVectorL.AddIndex(i - 1);
                    rowVectorL.AddValue(coef);
                    U.AddRows(j + 1, i, -coef);
                }
            }
        }

        for (stype i = 0; i < Rows; ++i)
        {
            var rowVector = L.GetRowAsVector(i);
            rowVector.AddIndex(i);
            rowVector.AddValue(1);
        }

        return new SparseLUCsr(L, U, P);
    }

    /// <summary>
    /// LU-разложение с выбором ведущего элемента по стратегии Марковица
    /// </summary>
    /// <param name="u">Порог в диапазоне [0,1]</param>
    public SparseLUCsr LuFactorizeMarkowitz(vtype u = 0.1)
    {
        if (Rows != Columns) throw new IncompatibleDimensionsException("Matrix must be square");
        if (u is < 0 or > 1) throw new ArgumentException("u should be in range [0,1]");

        SparseMatrixCsr L = new SparseMatrixCsr(Rows, Columns);
        SparseMatrixCsr U = this.Copy();
        stype[] P = new stype[Rows];
        stype[] Q = new stype[Columns];
        for (stype i = 0; i < Rows; ++i)
        {
            P[i] = i + 1;
            Q[i] = i + 1;
        }

        stype[] countInRows = new stype[Rows];
        stype[] countInColumns = new stype[Columns];
        vtype?[] maxInColumns = new vtype?[Columns];
        for (stype i = 0; i < Rows; ++i)
        {
            // counting
            for (stype j = i; j < Rows; ++j)
            {
                countInRows[j] = 0;
                countInColumns[j] = 0;
                maxInColumns[j] = null;
            }

            for (stype j = i; j < Rows; ++j)
            {
                var rowVector = U.GetRowAsVector(j);
                countInRows[j] = rowVector.NumberOfNonzeroElements;

                for (stype k = 0; k < rowVector.NumberOfNonzeroElements; ++k)
                {
                    stype columnIndex = rowVector.GetIndexAt(k);
                    vtype value = rowVector.GetValueAt(k);

                    countInColumns[columnIndex]++;
                    if (maxInColumns[columnIndex] == null || maxInColumns[columnIndex] < Math.Abs(value))
                        maxInColumns[columnIndex] = Math.Abs(value);
                }
            }

            // find pivot
            stype? minMark = null;
            stype pivotRow = i, pivotColumn = i;
            vtype pivotValue = 0;
            for (stype j = i; j < Rows; ++j)
            {
                var rowVector = U.GetRowAsVector(j);
                for (stype k = 0; k < rowVector.NumberOfNonzeroElements; ++k)
                {
                    stype columnIndex = rowVector.GetIndexAt(k);
                    vtype value = rowVector.GetValueAt(k);
                    stype mark = (countInRows[j] - 1) * (countInColumns[columnIndex] - 1);

                    if (Math.Abs(value) < u * maxInColumns[columnIndex]) continue; // check threshold

                    if (minMark == null || mark < minMark)
                    {
                        minMark = mark;
                        pivotRow = j;
                        pivotColumn = columnIndex;
                        pivotValue = value;
                    }
                }
            }

            if (minMark == null) throw new SingularMatrixException("Matrix must be non-singular");

            // swap
            if (pivotRow != i)
            {
                L.SwapRows(i + 1, pivotRow + 1);
                U.SwapRows(i + 1, pivotRow + 1);
                (P[i], P[pivotRow]) = (P[pivotRow], P[i]);
            }

            if (pivotColumn != i)
            {
                U.SwapColumns(i + 1, pivotColumn + 1);
                (Q[i], Q[pivotColumn]) = (Q[pivotColumn], Q[i]);
            }

            // eliminate
            for (stype j = i + 1; j < Rows; ++j)
            {
                var rowVectorL = L.GetRowAsVector(j);
                var rowVectorU = U.GetRowAsVector(j);

                Element firstElement = rowVectorU[0];
                if (firstElement.Index == i)
                {
                    vtype coef = firstElement.Value / pivotValue;
                    rowVectorL.AddIndex(i);
                    rowVectorL.AddValue(coef);
                    U.AddRows(j + 1, i + 1, -coef);
                }
            }
        }

        for (stype i = 0; i < Rows; ++i)
        {
            var rowVector = L.GetRowAsVector(i);
            rowVector.AddIndex(i);
            rowVector.AddValue(1);
        }

        return new SparseLUCsr(L, U, P, Q);
    }
    
    /// <summary>
    /// LU-разложение с выбором ведущего элемента по стратегии Марковица.
    /// Версия с отложенной перестановкой столбцов.
    /// </summary>
    /// <param name="u">Порог в диапазоне [0,1]</param>
    public SparseLUCsr LuFactorizeMarkowitz2(vtype u = 0.1)
    {
        if (Rows != Columns) throw new IncompatibleDimensionsException("Matrix must be square");
        if (u is < 0 or > 1) throw new ArgumentException("u should be in range [0,1]");

        SparseMatrixCsr L = new SparseMatrixCsr(Rows, Columns);
        SparseMatrixCsr U = this.Copy();
        stype[] P = new stype[Rows];
        stype[] Q = new stype[Columns];
        for (stype i = 0; i < Rows; ++i)
        {
            P[i] = i + 1;
            Q[i] = i + 1;
        }

        stype[] countInRows = new stype[Rows];
        stype[] countInColumns = new stype[Columns];
        vtype?[] maxInColumns = new vtype?[Columns];
        bool[] eliminatedColumns = new bool[Columns];
        for (stype i = 0; i < Columns; ++i)
            eliminatedColumns[i] = false;
        for (stype i = 0; i < Rows; ++i)
        {
            // counting
            for (stype j = 0; j < Rows; ++j)
            {
                countInRows[j] = 0;
                countInColumns[j] = 0;
                maxInColumns[j] = null;
            }

            for (stype j = i; j < Rows; ++j)
            {
                var rowVector = U.GetRowAsVector(j);
                countInRows[j] = rowVector.NumberOfNonzeroElements;

                for (stype k = 0; k < rowVector.NumberOfNonzeroElements; ++k)
                {
                    stype columnIndex = rowVector.GetIndexAt(k);
                    if (eliminatedColumns[columnIndex]) continue;
                    vtype value = rowVector.GetValueAt(k);

                    countInColumns[columnIndex]++;
                    if (maxInColumns[columnIndex] == null || maxInColumns[columnIndex] < Math.Abs(value))
                        maxInColumns[columnIndex] = Math.Abs(value);
                }
            }

            // find pivot
            stype? minMark = null;
            stype pivotRow = i, pivotColumn = i;
            vtype pivotValue = 0;
            for (stype j = i; j < Rows; ++j)
            {
                var rowVector = U.GetRowAsVector(j);
                for (stype k = 0; k < rowVector.NumberOfNonzeroElements; ++k)
                {
                    stype columnIndex = rowVector.GetIndexAt(k);
                    if (eliminatedColumns[columnIndex]) continue;
                    vtype value = rowVector.GetValueAt(k);
                    stype mark = (countInRows[j] - 1) * (countInColumns[columnIndex] - 1);

                    if (Math.Abs(value) < u * maxInColumns[columnIndex]) continue; // check threshold

                    if (minMark == null || mark < minMark)
                    {
                        minMark = mark;
                        pivotRow = j;
                        pivotColumn = columnIndex;
                        pivotValue = value;
                    }
                }
            }

            if (minMark == null) throw new SingularMatrixException("Matrix must be non-singular");

            // swap
            if (pivotRow != i)
            {
                L.SwapRows(i + 1, pivotRow + 1);
                U.SwapRows(i + 1, pivotRow + 1);
                (P[i], P[pivotRow]) = (P[pivotRow], P[i]);
            }
            
            Q[i] = pivotColumn + 1;

            // eliminate
            for (stype j = i + 1; j < Rows; ++j)
            {
                var rowVectorL = L.GetRowAsVector(j);
                var rowVectorU = U.GetRowAsVector(j);

                int indexToDivide = rowVectorU.Indices.BinarySearch(pivotColumn);
                if (indexToDivide < 0) continue;
                vtype valueToDivide = rowVectorU.GetValueAt(indexToDivide);
                
                vtype coef = valueToDivide / pivotValue;
                rowVectorL.AddIndex(i);
                rowVectorL.AddValue(coef);
                U.AddRows(j + 1, i + 1, -coef);
            }

            eliminatedColumns[pivotColumn] = true;
        }

        for (stype i = 0; i < Rows; ++i)
        {
            var rowVector = L.GetRowAsVector(i);
            rowVector.AddIndex(i);
            rowVector.AddValue(1);
        }

        U.PermuteColumnsFast(Q);

        return new SparseLUCsr(L, U, P, Q);
    }
    
    // parallel implementations
    
    /// <summary>
    /// LU-разложение с выбором ведущего элемента по столбцу.
    /// Этап с вычитанием строк выполняется параллельно.
    /// </summary>
    public SparseLUCsr LuFactorizeParallel()
    {
        if (Rows != Columns) throw new IncompatibleDimensionsException("Matrix must be square");
        
        SparseMatrixCsr L = new SparseMatrixCsr(Rows, Columns);
        SparseMatrixCsr U = this.Copy();
        stype[] P = new stype[Rows];
        for (stype i = 0; i < P.Length; ++i)
            P[i] = i + 1;

        for (stype i = 1; i < Rows; ++i)
        {
            // find max
            stype maxIndex = 0;
            vtype maxAbsValue = 0;
            for (stype j = i - 1; j < Rows; ++j)
            {
                var rowVector = U.GetRowAsVector(j);
                if (rowVector.NumberOfNonzeroElements == 0) continue;
                if (rowVector.GetIndexAt(0) != i - 1) continue;

                vtype absValue = Math.Abs(rowVector.GetValueAt(0));
                if (absValue > maxAbsValue)
                {
                    maxIndex = j;
                    maxAbsValue = absValue;
                }
            }

            if (maxAbsValue == 0) // maxAbsValue.IsZero() ?
                throw new SingularMatrixException("Matrix must be non-singular");

            // eliminate
            vtype pivotingValue = U.GetRowAsVector(maxIndex).GetValueAt(0);
            
            L.SwapRows(i, maxIndex + 1);
            U.SwapRows(i, maxIndex + 1);
            (P[i - 1], P[maxIndex]) = (P[maxIndex], P[i - 1]);

            Parallel.For(i, Rows, (j) =>
            {
                var rowVectorL = L.GetRowAsVector(j);
                var rowVectorU = U.GetRowAsVector(j);

                Element firstElement = rowVectorU[0];
                if (firstElement.Index == i - 1)
                {
                    vtype coef = firstElement.Value / pivotingValue;
                    rowVectorL.AddIndex(i - 1);
                    rowVectorL.AddValue(coef);
                    U.AddRows(j + 1, i, -coef);
                }
            });
        }

        for (stype i = 0; i < Rows; ++i)
        {
            var rowVector = L.GetRowAsVector(i);
            rowVector.AddIndex(i);
            rowVector.AddValue(1);
        }

        return new SparseLUCsr(L, U, P);
    }
    
    /// <summary>
    /// LU-разложение с выбором ведущего элемента по стратегии Марковица.
    /// Этап с вычитанием строк выполняется параллельно.
    /// </summary>
    /// <param name="u">Порог в диапазоне [0,1]</param>
    public SparseLUCsr LuFactorizeMarkowitzParallel(vtype u = 0.1)
    {
        if (Rows != Columns) throw new IncompatibleDimensionsException("Matrix must be square");
        if (u is < 0 or > 1) throw new ArgumentException("u should be in range [0,1]");

        SparseMatrixCsr L = new SparseMatrixCsr(Rows, Columns);
        SparseMatrixCsr U = this.Copy();
        stype[] P = new stype[Rows];
        stype[] Q = new stype[Columns];
        for (stype i = 0; i < Rows; ++i)
        {
            P[i] = i + 1;
            Q[i] = i + 1;
        }

        stype[] countInRows = new stype[Rows];
        stype[] countInColumns = new stype[Columns];
        vtype?[] maxInColumns = new vtype?[Columns];
        for (stype i = 0; i < Rows; ++i)
        {
            // counting
            for (stype j = i; j < Rows; ++j)
            {
                countInRows[j] = 0;
                countInColumns[j] = 0;
                maxInColumns[j] = null;
            }

            for (stype j = i; j < Rows; ++j)
            {
                var rowVector = U.GetRowAsVector(j);
                countInRows[j] = rowVector.NumberOfNonzeroElements;

                for (stype k = 0; k < rowVector.NumberOfNonzeroElements; ++k)
                {
                    stype columnIndex = rowVector.GetIndexAt(k);
                    vtype value = rowVector.GetValueAt(k);

                    countInColumns[columnIndex]++;
                    if (maxInColumns[columnIndex] == null || maxInColumns[columnIndex] < Math.Abs(value))
                        maxInColumns[columnIndex] = Math.Abs(value);
                }
            }

            // find pivot
            stype? minMark = null;
            stype pivotRow = i, pivotColumn = i;
            vtype pivotValue = 0;
            for (stype j = i; j < Rows; ++j)
            {
                var rowVector = U.GetRowAsVector(j);
                for (stype k = 0; k < rowVector.NumberOfNonzeroElements; ++k)
                {
                    stype columnIndex = rowVector.GetIndexAt(k);
                    vtype value = rowVector.GetValueAt(k);
                    stype mark = (countInRows[j] - 1) * (countInColumns[columnIndex] - 1);

                    if (Math.Abs(value) < u * maxInColumns[columnIndex]) continue; // check threshold

                    if (minMark == null || mark < minMark)
                    {
                        minMark = mark;
                        pivotRow = j;
                        pivotColumn = columnIndex;
                        pivotValue = value;
                    }
                }
            }

            if (minMark == null) throw new SingularMatrixException("Matrix must be non-singular");

            // swap
            if (pivotRow != i)
            {
                L.SwapRows(i + 1, pivotRow + 1);
                U.SwapRows(i + 1, pivotRow + 1);
                (P[i], P[pivotRow]) = (P[pivotRow], P[i]);
            }

            if (pivotColumn != i)
            {
                U.SwapColumns(i + 1, pivotColumn + 1);
                (Q[i], Q[pivotColumn]) = (Q[pivotColumn], Q[i]);
            }

            // eliminate
            Parallel.For(i + 1, Rows, (j) =>
            {
                var rowVectorL = L.GetRowAsVector(j);
                var rowVectorU = U.GetRowAsVector(j);

                Element firstElement = rowVectorU[0];
                if (firstElement.Index == i)
                {
                    vtype coef = firstElement.Value / pivotValue;
                    rowVectorL.AddIndex(i);
                    rowVectorL.AddValue(coef);
                    U.AddRows(j + 1, i + 1, -coef);
                }
            });
        }

        for (stype i = 0; i < Rows; ++i)
        {
            var rowVector = L.GetRowAsVector(i);
            rowVector.AddIndex(i);
            rowVector.AddValue(1);
        }

        return new SparseLUCsr(L, U, P, Q);
    }
    
    /// <summary>
    /// LU-разложение с выбором ведущего элемента по стратегии Марковица.
    /// Версия с отложенной перестановкой столбцов.
    /// Этап с вычитанием строк выполняется параллельно.
    /// </summary>
    /// <param name="u">Порог в диапазоне [0,1]</param>
    public SparseLUCsr LuFactorizeMarkowitz2Parallel(vtype u = 0.1)
    {
        if (Rows != Columns) throw new IncompatibleDimensionsException("Matrix must be square");
        if (u is < 0 or > 1) throw new ArgumentException("u should be in range [0,1]");

        SparseMatrixCsr L = new SparseMatrixCsr(Rows, Columns);
        SparseMatrixCsr U = this.Copy();
        stype[] P = new stype[Rows];
        stype[] Q = new stype[Columns];
        for (stype i = 0; i < Rows; ++i)
        {
            P[i] = i + 1;
            Q[i] = i + 1;
        }

        stype[] countInRows = new stype[Rows];
        stype[] countInColumns = new stype[Columns];
        vtype?[] maxInColumns = new vtype?[Columns];
        bool[] eliminatedColumns = new bool[Columns];
        for (stype i = 0; i < Columns; ++i)
            eliminatedColumns[i] = false;
        for (stype i = 0; i < Rows; ++i)
        {
            // counting
            for (stype j = 0; j < Rows; ++j)
            {
                countInRows[j] = 0;
                countInColumns[j] = 0;
                maxInColumns[j] = null;
            }

            for (stype j = i; j < Rows; ++j)
            {
                var rowVector = U.GetRowAsVector(j);
                countInRows[j] = rowVector.NumberOfNonzeroElements;

                for (stype k = 0; k < rowVector.NumberOfNonzeroElements; ++k)
                {
                    stype columnIndex = rowVector.GetIndexAt(k);
                    if (eliminatedColumns[columnIndex]) continue;
                    vtype value = rowVector.GetValueAt(k);

                    countInColumns[columnIndex]++;
                    if (maxInColumns[columnIndex] == null || maxInColumns[columnIndex] < Math.Abs(value))
                        maxInColumns[columnIndex] = Math.Abs(value);
                }
            }

            // find pivot
            stype? minMark = null;
            stype pivotRow = i, pivotColumn = i;
            vtype pivotValue = 0;
            for (stype j = i; j < Rows; ++j)
            {
                var rowVector = U.GetRowAsVector(j);
                for (stype k = 0; k < rowVector.NumberOfNonzeroElements; ++k)
                {
                    stype columnIndex = rowVector.GetIndexAt(k);
                    if (eliminatedColumns[columnIndex]) continue;
                    vtype value = rowVector.GetValueAt(k);
                    stype mark = (countInRows[j] - 1) * (countInColumns[columnIndex] - 1);

                    if (Math.Abs(value) < u * maxInColumns[columnIndex]) continue; // check threshold

                    if (minMark == null || mark < minMark)
                    {
                        minMark = mark;
                        pivotRow = j;
                        pivotColumn = columnIndex;
                        pivotValue = value;
                    }
                }
            }

            if (minMark == null) throw new SingularMatrixException("Matrix must be non-singular");

            // swap
            if (pivotRow != i)
            {
                L.SwapRows(i + 1, pivotRow + 1);
                U.SwapRows(i + 1, pivotRow + 1);
                (P[i], P[pivotRow]) = (P[pivotRow], P[i]);
            }
            
            Q[i] = pivotColumn + 1;

            // eliminate
            Parallel.For(i + 1, Rows, (j) =>
            {
                var rowVectorL = L.GetRowAsVector(j);
                var rowVectorU = U.GetRowAsVector(j);

                int indexToDivide = rowVectorU.Indices.BinarySearch(pivotColumn);
                if (indexToDivide < 0) return;
                vtype valueToDivide = rowVectorU.GetValueAt(indexToDivide);
                
                vtype coef = valueToDivide / pivotValue;
                rowVectorL.AddIndex(i);
                rowVectorL.AddValue(coef);
                U.AddRows(j + 1, i + 1, -coef);
            });

            eliminatedColumns[pivotColumn] = true;
        }

        for (stype i = 0; i < Rows; ++i)
        {
            var rowVector = L.GetRowAsVector(i);
            rowVector.AddIndex(i);
            rowVector.AddValue(1);
        }

        U.PermuteColumnsFast(Q);

        return new SparseLUCsr(L, U, P, Q);
    }
}