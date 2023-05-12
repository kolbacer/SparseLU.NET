using SparseMatrixAlgebra.Sparse.CSR;

namespace SparseMatrixAlgebra.Benchmarks.Factorization.SpecificMatrices;

/// <summary>
/// Класс-обертка для тестовых матриц.
/// При создании экземпляра прогоняет алгоритмы и находит результирующее кол-во ненулевых элементов.
/// </summary>
public class FactorizationTestRun
{
    public readonly SparseMatrixCsr Matrix;
    public string Title { get; set; }
    public int InitNonzeros { get; set; }
    public int ResultNonzeros { get; set; } // подсчитывается заранее
    public int ResultNonzerosMarkowitz { get; set; } // подсчитывается заранее

    public FactorizationTestRun(string title, SparseMatrixCsr matrix)
    {
        Title = title;
        Matrix = matrix;
        InitNonzeros = matrix.NumberOfNonzeroElements;
        var LU = matrix.LuFactorize();
        ResultNonzeros = LU.L.NumberOfNonzeroElements + LU.U.NumberOfNonzeroElements;
        LU = matrix.LuFactorizeMarkowitz();
        ResultNonzerosMarkowitz = LU.L.NumberOfNonzeroElements + LU.U.NumberOfNonzeroElements;
    }

    public FactorizationTestRun(SparseMatrixCsr matrix) : this(matrix.ToString(), matrix) { }

    public override string ToString() => Title;
}