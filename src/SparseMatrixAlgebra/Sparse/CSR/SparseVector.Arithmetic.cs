using SparseMatrixAlgebra.Common.Exceptions;
using SparseMatrixAlgebra.Common.Extensions;

namespace SparseMatrixAlgebra.Sparse.CSR;

public partial class SparseVector
{
    /// <summary>
    /// Умножить вектор, как вектор-строку на вектор-столбец.
    /// </summary>
    public vtype MultiplyRowByColumn(SparseVector columnVector)
    {
        if (this.Length == 0 || this.Length != columnVector.Length)
            throw new IncompatibleDimensionsException();

        SparseVector rowVector = this;
        vtype sum = 0;
        
        stype i_row = 0;
        stype i_col = 0;
        while ((i_row < rowVector.NumberOfNonzeroElements) && (i_col < columnVector.NumberOfNonzeroElements))
        {
            stype rowVectorIndex = rowVector.GetIndexAt(i_row);
            stype colVectorIndex = columnVector.GetIndexAt(i_col);

            if (rowVectorIndex < colVectorIndex)
                ++i_row;
            else if (colVectorIndex < rowVectorIndex)
                ++i_col;
            else
            {
                sum += rowVector.GetValueAt(i_row) * columnVector.GetValueAt(i_col);
                ++i_row;
                ++i_col;
            }
        }

        return sum; // can be very small
    }

    /// <summary>
    /// Умножить вектор, как вектор-столбец на вектор-строку.
    /// </summary>
    public SparseMatrixCsr MultiplyColumnByRow(SparseVector rowVector)
    {
        if (this.Length == 0 || rowVector.Length == 0)
            throw new IncompatibleDimensionsException("Vectors shouldn't be empty");

        SparseVector columnVector = this;
        var resultStorage = new CsrStorage(columnVector.Length, rowVector.Length);

        for (stype i = 0; i < columnVector.NumberOfNonzeroElements; ++i)
        {
            Element colElement = columnVector[i];
            for (stype j = 0; j < rowVector.NumberOfNonzeroElements; ++j)
            {
                Element rowElement = rowVector[j];
                vtype prod = colElement.Value * rowElement.Value;
                if (!prod.IsZero())
                    resultStorage.GetRowAsVector(colElement.Index).AddElement(new Element(rowElement.Index, prod));
            }
        }

        return new SparseMatrixCsr(resultStorage);
    }

    /// <summary>
    /// Умножить вектор-строку на матрицу.
    /// Можно передать сразу в транспонированном виде.
    /// </summary>
    public SparseVector MultiplyRowByMatrix(SparseMatrix<stype,vtype> matrix, bool transposed = false)
    {
        if (matrix is not SparseMatrixCsr) throw new IncompatibleTypeException("matrix must be SparseMatrixCsr");
        if ((!transposed && Length != matrix.Rows) || (transposed && Length != matrix.Columns)) 
            throw new IncompatibleDimensionsException();

        SparseMatrixCsr transposedMatrix = transposed ? (SparseMatrixCsr)matrix : (SparseMatrixCsr)matrix.Transposed();
        SparseVector resultVector = new SparseVector(transposedMatrix.Rows);

        for (stype i = 0; i < transposedMatrix.Rows; ++i)
        {
            vtype value = this.MultiplyRowByColumn(transposedMatrix.GetRowAsVector(i));
            if (!value.IsZero())
                resultVector.AddElement(new Element(i, value));
        }

        return resultVector;
    }
}