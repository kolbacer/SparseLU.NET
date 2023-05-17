using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using SparseMatrixAlgebra.Sparse.CSR;

namespace SparseMatrixAlgebra.Benchmarks.Factorization.RandomMatrices;

[DryJob] // запускать 1 раз
[MemoryDiagnoser]
// [RPlotExporter] // Должен быть установлен R и добавлен в PATH
[Config(typeof(Config))]
[HideColumns(Column.Gen0, Column.Gen1, Column.Gen2, Column.Error, Column.RatioSD)]
public class RandomMatricesFactorizationParallelBenchmark
{ 
    private class Config : ManualConfig
    {
        public Config()
        {
            SummaryStyle =
                SummaryStyle.Default
                    .WithRatioStyle(RatioStyle.Percentage)
                    .WithTimeUnit(Perfolizer.Horology.TimeUnit.Second);
            AddColumn(new NonzerosColumn("Avg init nonzeros", true));
            AddColumn(new NonzerosColumn("Avg (L+U) Nonzeros", false));
        }
    }

    public const int NumberOfIterations = 8; // количество матриц одного типа
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
        yield return new RandomTestRun(1000, 10, NumberOfIterations);
        yield return new RandomTestRun(1000, 20, NumberOfIterations);
        yield return new RandomTestRun(1000, 30, NumberOfIterations);
        yield return new RandomTestRun(1000, 40, NumberOfIterations);
        yield return new RandomTestRun(1000, 50, NumberOfIterations);
        // yield return new RandomTestRun(1000, 60, NumberOfIterations);
        // yield return new RandomTestRun(1000, 70, NumberOfIterations);
        // yield return new RandomTestRun(1000, 80, NumberOfIterations);
        // yield return new RandomTestRun(1000, 90, NumberOfIterations);
        // yield return new RandomTestRun(1000, 100, NumberOfIterations);
    }

    [Benchmark(Baseline = true, OperationsPerInvoke = NumberOfIterations)]
    [ArgumentsSource(nameof(TestMatrices))]
    public void CsrLuFactorizationParallel(RandomTestRun TestMatrix)
    {
        for (int i = 0; i < NumberOfIterations; ++i)
        {
            resultLUs[i] = TestMatrix.MatrixArray[i].LuFactorizeParallel();
        }
        resultFile = $"{ResultDirectory}\\nonzeros.csrlufactorizationparallel.{TestMatrix.Title}.txt";
    }

    [Benchmark(OperationsPerInvoke = NumberOfIterations)]
    [ArgumentsSource(nameof(TestMatrices))]
    public void CsrLuFactorizationMarkowitzParallel(RandomTestRun TestMatrix)
    {
        for (int i = 0; i < NumberOfIterations; ++i)
        {
            resultLUs[i] = TestMatrix.MatrixArray[i].LuFactorizeMarkowitzParallel(0.001);
        }
        resultFile = $"{ResultDirectory}\\nonzeros.csrlufactorizationmarkowitzparallel.{TestMatrix.Title}.txt";
    }
    
    [Benchmark(OperationsPerInvoke = NumberOfIterations)]
    [ArgumentsSource(nameof(TestMatrices))]
    public void CsrLuFactorizationMarkowitz2Parallel(RandomTestRun TestMatrix)
    {
        for (int i = 0; i < NumberOfIterations; ++i)
        {
            resultLUs[i] = TestMatrix.MatrixArray[i].LuFactorizeMarkowitz2Parallel(0.001);
        }
        resultFile = $"{ResultDirectory}\\nonzeros.csrlufactorizationmarkowitz2parallel.{TestMatrix.Title}.txt";
    }
}