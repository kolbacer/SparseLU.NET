using SparseMatrixAlgebra.Sparse;
using SparseMatrixAlgebra.Sparse.CSR;

namespace SparseMatrixAlgebra.Utils;

public static class MatrixBuilder
{
    /// <summary>
    /// Создает разреженную матрицу в формате CSR.
    /// </summary>
    /// <param name="rows">кол-во строк</param>
    /// <param name="cols">кол-во столбцов</param>
    public static SparseMatrix<stype, vtype> CreateCsr(stype rows, stype cols)
    {
        return new SparseMatrixCsr(rows, cols);
    }

    public static SparseMatrix<stype, vtype> ReadCsrFromFile(string filepath) => throw new NotImplementedException();

    public static SparseVector<stype, vtype> CreateCsrVector(stype length)
    {
        return new SparseVector(length);
    }
}