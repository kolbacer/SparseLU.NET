using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using SparseMatrixAlgebra.Sparse.CSR;
using SparseMatrixAlgebra.Utils;

namespace SparseMatrixAlgebra.Benchmarks.Factorization.SpecificMatrices;

[DryJob] // запускать 1 раз
[MemoryDiagnoser]
[RPlotExporter] // Должен быть установлен R и добавлен в PATH
[Config(typeof(Config))]
[HideColumns(Column.Gen0, Column.Gen1, Column.Gen2)]
public class SpecificMatricesFactorizationBenchmark
{
    private class Config : ManualConfig
    {
        public Config()
        {
            SummaryStyle =
                SummaryStyle.Default
                    .WithRatioStyle(RatioStyle.Percentage);
                    // .WithTimeUnit(Perfolizer.Horology.TimeUnit.Millisecond);
            AddColumn(new NonzerosColumn("Init Nonzeros", true));
            AddColumn(new NonzerosColumn("Total (L+U) Nonzeros", false));
        }
    }

    private SparseLUCsr resultLU;
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
        File.WriteAllText(resultFile,
            (resultLU.L.NumberOfNonzeroElements + resultLU.U.NumberOfNonzeroElements).ToString());
    }

    // absolute path for test matrices folder
    public const string MatrixDirectory =
        "D:\\Workspace\\SparseLU.NET\\tests\\SparseMatrixAlgebra.Benchmarks\\TestFiles\\SpecificMatrices";

    // absolute path for resulting nonzero folder
    public const string ResultDirectory = $"{MatrixDirectory}\\results";

    public IEnumerable<object> TestMatrices()
    {
        yield return new FactorizationTestRun("west2021", 
            MatrixBuilder.ReadCsrFromFile($"{MatrixDirectory}\\west2021.mtx"));
        yield return new FactorizationTestRun("add20",
            MatrixBuilder.ReadCsrFromFile($"{MatrixDirectory}\\add20.mtx"));
        yield return new FactorizationTestRun("circuit_1",
            MatrixBuilder.ReadCsrFromFile($"{MatrixDirectory}\\circuit_1.mtx"));
    }

    [Benchmark] // (OperationsPerInvoke = 10)
    [ArgumentsSource(nameof(TestMatrices))]
    public void CsrLuFactorization(FactorizationTestRun TestMatrix)
    {
        resultLU = TestMatrix.Matrix.LuFactorize();
        resultFile = $"{ResultDirectory}\\nonzeros.csrlufactorization.{TestMatrix.Title}.txt";
    }

    [Benchmark(Baseline = true)] // (OperationsPerInvoke = 10)
    [ArgumentsSource(nameof(TestMatrices))]
    public void CsrLuFactorizationMarkowitz(FactorizationTestRun TestMatrix)
    {
        resultLU = TestMatrix.Matrix.LuFactorizeMarkowitz(0.001);
        resultFile = $"{ResultDirectory}\\nonzeros.csrlufactorizationmarkowitz.{TestMatrix.Title}.txt";
    }
    
    [Benchmark] // (OperationsPerInvoke = 10)
    [ArgumentsSource(nameof(TestMatrices))]
    public void CsrLuFactorizationMarkowitz2(FactorizationTestRun TestMatrix)
    {
        resultLU = TestMatrix.Matrix.LuFactorizeMarkowitz2(0.001);
        resultFile = $"{ResultDirectory}\\nonzeros.csrlufactorizationmarkowitz2.{TestMatrix.Title}.txt";
    }
}