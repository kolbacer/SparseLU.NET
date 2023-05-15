using SparseMatrixAlgebra.Sparse.CSR;
using SparseMatrixAlgebra.Utils;

namespace SparseMatrixAlgebra.Benchmarks.Factorization.RandomMatrices;

/// <summary>
/// Класс-обертка для тестируемых рандомных матриц.
/// </summary>
public class RandomTestRun: ITestRun
{
    public const int N = RandomMatricesFactorizationBenchmark.NumberOfIterations;

    public readonly SparseMatrixCsr[] MatrixArray = new SparseMatrixCsr[N];
    public string Title { get; set; }

    public string Case { get; } = "random";
    
    /// <summary>
    /// Создает массив из <see cref="N"/> случайных матриц.
    /// </summary>
    /// <param name="size">размерность матрицы</param>
    /// <param name="fillInRow">среднее кол-во ненулевых элементов в строке</param>
    public RandomTestRun(int size, int fillInRow)
    {
        Title = $"N={size}_F={fillInRow}N";
        for (int i = 0; i < N; ++i)
        {
            MatrixArray[i] = MatrixBuilder.GenerateRandomCsr(size, size, fillInRow * size, size + fillInRow + i);
        }
    }

    public override string ToString() => Title;
}