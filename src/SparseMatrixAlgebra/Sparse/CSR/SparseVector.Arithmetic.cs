using SparseMatrixAlgebra.Common.Exceptions;
using SparseMatrixAlgebra.Common.Extensions;

namespace SparseMatrixAlgebra.Sparse.CSR;

public partial class SparseVector
{
    /// <summary>
    /// Поменять местами элементы вектора. Индексация с 1.
    /// </summary>
    /// <remarks>Сложность в худшем случае O(<see cref="NumberOfNonzeroElements"/>)</remarks>
    public override void SwapElements(stype index1, stype index2)
    {
        if (index1 < 1 || index1 > Length || index2 < 1 || index2 > Length)
            throw new OutOfVectorException();
        
        if (index1 == index2) return;
        
        stype iIndex1, iIndex2;
        (iIndex1, iIndex2) = (index1 < index2) ? (index1 - 1, index2 - 1) : (index2 - 1, index1 - 1);

        stype toElement1Index, toElement2Index;
        bool element1Found = false, element2Found = false;

        stype i = 0;
        // find element1
        for (; i < NumberOfNonzeroElements; ++i)
        {
            stype index = GetIndexAt(i);
            if (index > iIndex1) break;
            if (index == iIndex1)
            {
                element1Found = true;
                ++i;
                break;
            }
        }

        if (!element1Found)
        {
            if (i == NumberOfNonzeroElements) return;
            toElement1Index = i;
        }
        else
            toElement1Index = i - 1;

        // find element2
        for (; i < NumberOfNonzeroElements; ++i)
        {
            stype index = GetIndexAt(i);
            if (index > iIndex2) break;
            if (index == iIndex2)
            {
                element2Found = true;
                ++i;
                break;
            }
        }

        toElement2Index = i - 1;

        if (!element1Found && !element2Found) return;

        if (element1Found && element2Found)
            (Values[toElement1Index], Values[toElement2Index]) = (Values[toElement2Index], Values[toElement1Index]);
        else if (!element1Found)
        {
            if (toElement1Index == toElement2Index)
            {
                Indices[toElement1Index] = iIndex1;
                return;
            }

            vtype toElement2Value = Values[toElement2Index];

            for (stype j = toElement2Index; j > toElement1Index; --j)
            {
                Indices[j] = Indices[j - 1];
                Values[j] = Values[j - 1];
            }

            Indices[toElement1Index] = iIndex1;
            Values[toElement1Index] = toElement2Value;
        }
        else
        {
            if (toElement1Index == toElement2Index)
            {
                Indices[toElement2Index] = iIndex2;
                return;
            }
            
            vtype toElement1Value = Values[toElement1Index];

            for (stype j = toElement1Index; j < toElement2Index; ++j)
            {
                Indices[j] = Indices[j + 1];
                Values[j] = Values[j + 1];
            }

            Indices[toElement2Index] = iIndex2;
            Values[toElement2Index] = toElement1Value;
        }
    }
    
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