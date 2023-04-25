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
}