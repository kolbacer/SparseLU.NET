using System.Runtime.InteropServices;
using SparseMatrixAlgebra.Common.Exceptions;
using SparseMatrixAlgebra.Common.Extensions;
using Element = SparseMatrixAlgebra.Sparse.CSR.SparseVector.Element;
    
namespace SparseMatrixAlgebra.Sparse.CSR;

public partial class SparseMatrixCsr
{
    public override void AddRows(stype augend, stype addend) => AddRows(augend, addend, 1);
    
    public override void AddRows(stype augend, stype addend, vtype coef)
    {
        if (augend < 1 || augend > Rows || addend < 1 || addend > Rows)
            throw new OutOfMatrixException();
        
        if (coef == 0) return; // coef.IsZero()?

        stype iAugend = augend - 1;
        stype iAddend = addend - 1;
        
        var columnIndexRows = ((CsrStorage)Storage).ColumnIndexRows;
        var valueRows = ((CsrStorage)Storage).ValueRows;
        
        if (columnIndexRows[iAddend].Count == 0) return; // addend row is empty

        // augend == addend
        if (iAugend == iAddend)
        {
            if (coef == -1)
            {
                columnIndexRows[iAugend] = new List<stype>();
                valueRows[iAugend] = new List<vtype>();
                
                return;
            }

            var compressedRow = GetRowAsVector(iAugend);
            for (stype i = 0; i < compressedRow.NumberOfNonzeroElements; ++i)
            {
                compressedRow.SetValueAt(i, compressedRow.GetValueAt(i) * (1 + coef));
            }
            
            return;
        }
        
        // augend != addend

        var augendRow = GetRowAsVector(iAugend);
        var addendRow = GetRowAsVector(iAddend);
        
        // augend row is empty
        if (augendRow.NumberOfNonzeroElements == 0)
        {
            var augendIndices = augendRow.Indices;
            var augendValues = augendRow.Values;
            
            augendIndices.Capacity = addendRow.NumberOfNonzeroElements;
            augendValues.Capacity = addendRow.NumberOfNonzeroElements;
            
            for (stype i = 0; i < addendRow.NumberOfNonzeroElements; ++i)
            {
                augendIndices.Add(addendRow.GetIndexAt(i));
                augendValues.Add(addendRow.GetValueAt(i) * coef);
            }
            
            return;
        }
        
        // augend row is not empty
        
        List<stype> newColumnIndices = new List<stype>(augendRow.NumberOfNonzeroElements + addendRow.NumberOfNonzeroElements);
        List<vtype> newValues = new List<vtype>(augendRow.NumberOfNonzeroElements + addendRow.NumberOfNonzeroElements);

        stype i_aug = 0;
        stype i_add = 0;
        while (i_aug < augendRow.NumberOfNonzeroElements || i_add < addendRow.NumberOfNonzeroElements)
        {
            stype? augColumnIndex = (i_aug < augendRow.NumberOfNonzeroElements) ? augendRow.GetIndexAt(i_aug) : null;
            stype? addColumnIndex = (i_add < addendRow.NumberOfNonzeroElements) ? addendRow.GetIndexAt(i_add) : null;

            stype colIndexToAdd;
            vtype valueToAdd;

            if ((addColumnIndex is null) || (augColumnIndex < addColumnIndex))
            {
                colIndexToAdd = augColumnIndex.Value;
                valueToAdd = augendRow.GetValueAt(i_aug);
                ++i_aug;
            } else if ((augColumnIndex is null) || (addColumnIndex < augColumnIndex))
            {
                colIndexToAdd = addColumnIndex.Value;
                valueToAdd = addendRow.GetValueAt(i_add) * coef;
                ++i_add;
            } else
            {
                colIndexToAdd = augColumnIndex.Value;
                valueToAdd = augendRow.GetValueAt(i_aug) + (addendRow.GetValueAt(i_add) * coef);
                ++i_aug;
                ++i_add;
            }

            if (!valueToAdd.IsZero())
            {
                newColumnIndices.Add(colIndexToAdd);
                newValues.Add(valueToAdd);
            }
        }
        
        newColumnIndices.TrimExcess();
        newValues.TrimExcess();

        columnIndexRows[iAugend] = newColumnIndices;
        valueRows[iAugend] = newValues;
    }

    public override void SwapRows(stype row1, stype row2)
    {
        if (row1 < 1 || row1 > Rows || row2 < 1 || row2 > Rows)
            throw new OutOfMatrixException();
        
        if (row1 == row2) return;

        stype iRow1 = row1 - 1;
        stype iRow2 = row2 - 1;

        var columnIndexRows = ((CsrStorage)Storage).ColumnIndexRows;
        var valueRows = ((CsrStorage)Storage).ValueRows;

        var tmpIndexRow = columnIndexRows[iRow1];
        var tmpValueRow = valueRows[iRow1];
        columnIndexRows[iRow1] = columnIndexRows[iRow2];
        valueRows[iRow1] = valueRows[iRow2];
        columnIndexRows[iRow2] = tmpIndexRow;
        valueRows[iRow2] = tmpValueRow;
    }

    /// <summary>
    /// Поменять местами 2 столбца в матрице. Индексация с 1.
    /// </summary>
    /// <remarks>Сложность в худшем случае O(NumberOfNonzeroElements)</remarks>
    public override void SwapColumns(stype col1, stype col2)
    {
        if (col1 < 1 || col1 > Columns || col2 < 1 || col2 > Columns)
            throw new OutOfMatrixException();
        
        if (col1 == col2) return;

        for (stype i = 0; i < Rows; ++i)
        {
            GetRowAsVector(i).SwapElements(col1, col2);
        }
    }

    public override SparseMatrixCsr Transposed()
    {
        var transposedStorage = new CsrStorage(Columns, Rows);
        var trColumnIndexRows = transposedStorage.ColumnIndexRows;
        var trValueRows = transposedStorage.ValueRows;

        for (stype i = 0; i < Rows; ++i)
        {
            var compressedRow = GetRowAsVector(i);
            for (stype j = 0; j < compressedRow.NumberOfNonzeroElements; ++j)
            {
                trColumnIndexRows[compressedRow.GetIndexAt(j)].Add(i);
                trValueRows[compressedRow.GetIndexAt(j)].Add(compressedRow.GetValueAt(j));
            }
        }

        return new SparseMatrixCsr(transposedStorage);
    }
    
    public override SparseVector<stype, vtype> MultiplyByVector(SparseVector<stype, vtype> vector)
    {
        if (vector is not SparseVector) throw new IncompatibleTypeException("vector must be SparseVector");
        if (Columns != vector.Length) throw new IncompatibleDimensionsException();
        
        SparseVector colVector = (SparseVector)vector;
        SparseVector resultVector = new SparseVector(Rows,true);

        for (stype i = 0; i < Rows; ++i)
        {
            vtype value = GetRowAsVector(i).MultiplyRowByColumn(colVector);
            if (!value.IsZero())
                resultVector.AddElement(new Element(i, value));
        }

        return resultVector;
    }
    
    public override SparseMatrixCsr MultiplyByMatrix(SparseMatrix<stype, vtype> matrix)
    {
        if (matrix is not SparseMatrixCsr) throw new IncompatibleTypeException("matrix must be SparseMatrixCsr");
        if (Columns != matrix.Rows) throw new IncompatibleDimensionsException();
    
        SparseMatrixCsr thisMatrix = this;
        SparseMatrixCsr otherMatrix = (SparseMatrixCsr)matrix;
        SparseMatrixCsr otherMatrixTransposed = otherMatrix.Transposed();

        List<List<stype>> newColumnIndexRows = new List<List<stype>>(thisMatrix.Rows);
        List<List<vtype>> newValueRows= new List<List<vtype>>(thisMatrix.Rows);

        for (stype i = 0; i < thisMatrix.Rows; ++i)
        {
            SparseVector newRow = thisMatrix.GetRowAsVector(i).MultiplyRowByMatrix(otherMatrixTransposed, true);
            newColumnIndexRows.Add(newRow.Indices);
            newValueRows.Add(newRow.Values);
        }

        return new SparseMatrixCsr(new CsrStorage(
            thisMatrix.Rows, otherMatrix.Columns, newColumnIndexRows, newValueRows)
        );
    }

    /// <summary>
    /// Переставить строки, умножив P*A
    /// </summary>
    /// <param name="P">Перестановочная матрица в виде массива</param>
    public SparseMatrixCsr PermuteRows(stype[] P)
    {
        if (P.Length != Rows) throw new ArgumentException("P.Length must be of the number of Rows");

        CsrStorage newStorage = new CsrStorage(Rows, Columns);
        for (stype i = 0; i < Rows; ++i)
        {
            newStorage.ColumnIndexRows[i] = new List<stype>(((CsrStorage)Storage).ColumnIndexRows[P[i] - 1]);
            newStorage.ValueRows[i] = new List<vtype>(((CsrStorage)Storage).ValueRows[P[i] - 1]);
        }

        return new SparseMatrixCsr(newStorage);
    }

    /// <summary>
    /// Переставить столбцы, умножив A*Q
    /// </summary>
    /// <param name="Q">Перестановочная матрица в виде массива</param>
    public SparseMatrixCsr PermuteColumns(stype[] Q)
    {
        if (Q.Length != Columns) throw new ArgumentException("Q.Length must be of the number of Columns");

        CsrStorage QStorage = new CsrStorage(Q.Length, Q.Length);
        for (stype i = 0; i < Q.Length; ++i)
        {
            QStorage.ColumnIndexRows[Q[i] - 1].Add(i);
            QStorage.ValueRows[Q[i] - 1].Add(1);
        }

        return MultiplyByMatrix(new SparseMatrixCsr(QStorage));
    }

    /// <summary>
    /// Переставить столбцы в соответствии с перестановкой Q.
    /// НЕ создает копию хранилища.
    /// </summary>
    /// <param name="Q">Перестановочная матрица в виде массива</param>
    public SparseMatrixCsr PermuteColumnsFast(stype[] Q)
    {
        if (Q.Length != Columns) throw new ArgumentException("Q.Length must be of the number of Columns");

        List<List<stype>> columnIndexRows = ((CsrStorage)Storage).ColumnIndexRows;
        List<List<vtype>> valueRows = ((CsrStorage)Storage).ValueRows;

        stype[] inversedQ = new stype[Q.Length];
        for (stype i = 0; i < Q.Length; ++i)
            inversedQ[Q[i] - 1] = i + 1;
        
        Parallel.For(0, Rows, (i) =>
        {
            Span<stype> indexSpan = CollectionsMarshal.AsSpan(columnIndexRows[i]);
            Span<vtype> valueSpan = CollectionsMarshal.AsSpan(valueRows[i]);

            for (int j = 0; j < indexSpan.Length; ++j)
                indexSpan[j] = inversedQ[indexSpan[j]] - 1;
            
            indexSpan.Sort(valueSpan);
        });

        return this;
    }
}