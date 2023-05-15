using SparseMatrixAlgebra.Sparse.CSR;

namespace SparseMatrixAlgebra.Benchmarks.Factorization.SpecificMatrices;

/// <summary>
/// Класс-обертка для тестовых матриц.
/// </summary>
public class FactorizationTestRun: ITestRun
{
    public readonly SparseMatrixCsr Matrix;
    public string Title { get; set; }

    public string Case { get; } = "specific";

    public FactorizationTestRun(string title, SparseMatrixCsr matrix)
    {
        Title = title;
        Matrix = matrix;
    }

    public FactorizationTestRun(SparseMatrixCsr matrix) : this(matrix.ToString(), matrix) { }

    public override string ToString() => Title;
}