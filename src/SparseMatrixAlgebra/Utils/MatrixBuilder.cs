using SparseMatrixAlgebra.Common.Extensions;
using SparseMatrixAlgebra.Sparse.CSR;

namespace SparseMatrixAlgebra.Utils;

public static class MatrixBuilder
{
    /// <summary>
    /// Создает разреженную матрицу заданного размера в формате CSR. (Все элементы нулевые). 
    /// </summary>
    /// <param name="rows">кол-во строк</param>
    /// <param name="cols">кол-во столбцов</param>
    public static SparseMatrixCsr CreateCsr(stype rows, stype cols)
    {
        return new SparseMatrixCsr(rows, cols);
    }

    /// <summary>
    /// Создает разреженную матрицу в формате CSR на основе переданного массива.
    /// </summary>
    /// <param name="array">Двумерный массив</param>
    public static SparseMatrixCsr CsrOfArray(vtype[,] array)
    {
        stype rows = array.GetUpperBound(0) + 1;
        stype columns = array.GetUpperBound(1) + 1;

        if (rows == 0 || columns == 0) throw new ArgumentException("array must be not empty");

        CsrStorage storage = new CsrStorage(rows, columns);
        for (stype i = 0; i < rows; ++i)
        {
            for (stype j = 0; j < columns; ++j)
            {
                if (!array[i, j].IsZero())
                {
                    storage.ColumnIndexRows[i].Add(j);
                    storage.ValueRows[i].Add(array[i, j]);
                }
            }
        }

        return new SparseMatrixCsr(storage);
    }

    public static SparseMatrixCsr ReadCsrFromFile(string filepath) => throw new NotImplementedException();

    public static SparseVector CreateCsrVector(stype length)
    {
        return new SparseVector(length);
    }
}