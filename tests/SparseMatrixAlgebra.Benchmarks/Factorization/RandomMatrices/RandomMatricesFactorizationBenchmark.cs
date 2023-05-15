using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using SparseMatrixAlgebra.Sparse.CSR;

namespace SparseMatrixAlgebra.Benchmarks.Factorization.RandomMatrices;

[DryJob] // запускать 1 раз
[MemoryDiagnoser]
[RPlotExporter] // Должен быть установлен R и добавлен в PATH
[Config(typeof(Config))]
[HideColumns(Column.Gen0, Column.Gen1, Column.Gen2, Column.Error, Column.RatioSD)]
public class RandomMatricesFactorizationBenchmark
{ 
    private class Config : ManualConfig
    {
        public Config()
        {
            SummaryStyle =
                SummaryStyle.Default
                    .WithRatioStyle(RatioStyle.Percentage);
                    // .WithTimeUnit(Perfolizer.Horology.TimeUnit.Millisecond);
            AddColumn(new NonzerosColumn("Avg init nonzeros", true));
            AddColumn(new NonzerosColumn("Avg (L+U) Nonzeros", false));
        }
    }

    public const int NumberOfIterations = 10; // количество матриц одного типа
    private SparseLUCsr[] resultLUs = new SparseLUCsr[NumberOfIterations];
    private string resultFile;

    [GlobalSetup]
    public void ResultDirectorySetup()
    {
        if (!Directory.Exists(ResultDirectory))
        {
            Directory.CreateDirectory(ResultDirectory);
        }
    }
    
    [GlobalCleanup]
    public void WriteFileCleanup()
    {
        int totalNonzeros = 0;
        for (int i = 0; i < resultLUs.Length; ++i)
            totalNonzeros += resultLUs[i].L.NumberOfNonzeroElements + resultLUs[i].U.NumberOfNonzeroElements;
        File.WriteAllText(resultFile, (totalNonzeros / resultLUs.Length).ToString());
    }

    // absolute path for resulting nonzero folder
    public const string ResultDirectory =
        "D:\\Workspace\\SparseLU.NET\\tests\\SparseMatrixAlgebra.Benchmarks\\TestFiles\\RandomMatrices\\results";

    public IEnumerable<object> TestMatrices()
    {
        yield return new RandomTestRun(1000, 10);
        yield return new RandomTestRun(1000, 20);
        yield return new RandomTestRun(1000, 30);
        yield return new RandomTestRun(1000, 40);
        yield return new RandomTestRun(1000, 50);
        // yield return new RandomTestRun(1000, 60);
        // yield return new RandomTestRun(1000, 70);
        // yield return new RandomTestRun(1000, 80);
        // yield return new RandomTestRun(1000, 90);
        // yield return new RandomTestRun(1000, 100);
    }

    [Benchmark(Baseline = true, OperationsPerInvoke = NumberOfIterations)]
    [ArgumentsSource(nameof(TestMatrices))]
    public void CsrLuFactorization(RandomTestRun TestMatrix)
    {
        for (int i = 0; i < NumberOfIterations; ++i)
        {
            resultLUs[i] = TestMatrix.MatrixArray[i].LuFactorize();
        }
        resultFile = $"{ResultDirectory}\\nonzeros.csrlufactorization.{TestMatrix.Title}.txt";
    }

    [Benchmark(OperationsPerInvoke = NumberOfIterations)]
    [ArgumentsSource(nameof(TestMatrices))]
    public void CsrLuFactorizationMarkowitz(RandomTestRun TestMatrix)
    {
        for (int i = 0; i < NumberOfIterations; ++i)
        {
            resultLUs[i] = TestMatrix.MatrixArray[i].LuFactorizeMarkowitz(0.001);
        }
        resultFile = $"{ResultDirectory}\\nonzeros.csrlufactorizationmarkowitz.{TestMatrix.Title}.txt";
    }
    
    [Benchmark(OperationsPerInvoke = NumberOfIterations)]
    [ArgumentsSource(nameof(TestMatrices))]
    public void CsrLuFactorizationMarkowitz2(RandomTestRun TestMatrix)
    {
        for (int i = 0; i < NumberOfIterations; ++i)
        {
            resultLUs[i] = TestMatrix.MatrixArray[i].LuFactorizeMarkowitz2(0.001);
        }
        resultFile = $"{ResultDirectory}\\nonzeros.csrlufactorizationmarkowitz2.{TestMatrix.Title}.txt";
    }
}