using MathNet.Numerics.LinearAlgebra.Double;

namespace SparseMatrixAlgebra.Benchmarks.Factorization.RandomMathNET;

/// <summary>
/// Класс-обертка для тестируемых рандомных матриц Math.NET Numerics
/// </summary>
public class RandomMathNetRun: ITestRun
{
    public readonly int N = 1;

    public readonly SparseMatrix[] SparseMatrixArray;
    public readonly DenseMatrix[] DenseMatrixArray;
    public string Title { get; set; }

    public string Case { get; } = "random";
    
    /// <summary>
    /// Создает 2 массива из <see cref="N"/> случайных разреженных и плотных матриц.
    /// </summary>
    /// <param name="size">размерность матрицы</param>
    /// <param name="fillInRow">среднее кол-во ненулевых элементов в строке</param>
    public RandomMathNetRun(int size, int fillInRow, int N)
    {
        this.N = N;
        this.SparseMatrixArray = new SparseMatrix[N];
        this.DenseMatrixArray = new DenseMatrix[N];
        Title = $"N={size}_F={fillInRow}N";
        for (int i = 0; i < N; ++i)
        {
            SparseMatrixArray[i] = GenerateRandomSparseMathNET(size, size, fillInRow * size, size + fillInRow + i);
            DenseMatrixArray[i] = GenerateRandomDenseMathNET(size, size, fillInRow * size, size + fillInRow + i);
        }
    }

    public override string ToString() => Title;
    
    /// <summary>
    /// Сгенерировать случайную разреженную матрицу Math.NET Numerics
    /// </summary>
    /// <param name="rows">Кол-во строк</param>
    /// <param name="cols">Кол-во столбцов</param>
    /// <param name="fill">Приблизительное кол-во ненулевых элементов</param>
    /// <param name="seed">Инициализатор для генератора</param>
    public static SparseMatrix GenerateRandomSparseMathNET(int rows, int cols, int fill, int? seed = null)
    {
        SparseMatrix matrix = SparseMatrix.Create(rows, cols, 0);
        
        Random rnd = (seed != null) ? new Random(seed.Value) : new Random();
        int bound = (rows < cols) ? rows : cols;
        
        for (int i = 0; i < fill - bound; ++i)
        {
            int row = rnd.Next(rows);
            int col = rnd.Next(cols);
            double value = rnd.NextDouble() + 1;
            matrix[row, col] = value;
        }

        for (int i = 0; i < bound; ++i)
        {
            matrix[i, i] = rnd.NextDouble() + 1;
        }

        return matrix;
    }
    
    /// <summary>
    /// Сгенерировать случайную плотную матрицу Math.NET Numerics
    /// </summary>
    /// <param name="rows">Кол-во строк</param>
    /// <param name="cols">Кол-во столбцов</param>
    /// <param name="fill">Приблизительное кол-во ненулевых элементов</param>
    /// <param name="seed">Инициализатор для генератора</param>
    public static DenseMatrix GenerateRandomDenseMathNET(int rows, int cols, int fill, int? seed = null)
    {
        DenseMatrix matrix = DenseMatrix.Create(rows, cols, 0);
        
        Random rnd = (seed != null) ? new Random(seed.Value) : new Random();
        int bound = (rows < cols) ? rows : cols;
        
        for (int i = 0; i < fill - bound; ++i)
        {
            int row = rnd.Next(rows);
            int col = rnd.Next(cols);
            double value = rnd.NextDouble() + 1;
            matrix[row, col] = value;
        }

        for (int i = 0; i < bound; ++i)
        {
            matrix[i, i] = rnd.NextDouble() + 1;
        }

        return matrix;
    }
}