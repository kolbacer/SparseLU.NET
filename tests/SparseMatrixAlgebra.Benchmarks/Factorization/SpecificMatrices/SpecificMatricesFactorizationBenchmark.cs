using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using SparseMatrixAlgebra.Utils;

namespace SparseMatrixAlgebra.Benchmarks.Factorization.SpecificMatrices;

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
                    .WithRatioStyle(RatioStyle.Percentage)
                    .WithTimeUnit(Perfolizer.Horology.TimeUnit.Millisecond);
            AddColumn(new NonzerosColumn("Init Nonzeros", true));
            AddColumn(new NonzerosColumn("Total (L+U) Nonzeros", false));
        }
    }

    // absolute path for test matrices folder
    private readonly string _matrixDirectory =
        "D:\\Workspace\\SparseLU.NET\\tests\\SparseMatrixAlgebra.Benchmarks\\TestFiles\\SpecificMatrices";

    public IEnumerable<object> TestMatrices()
    {
        yield return new FactorizationTestRun("west2021", 
            MatrixBuilder.ReadCsrFromFile($"{_matrixDirectory}\\west2021.mtx"));
        yield return new FactorizationTestRun("add20",
            MatrixBuilder.ReadCsrFromFile($"{_matrixDirectory}\\add20.mtx"));
        yield return new FactorizationTestRun("circuit_1",
            MatrixBuilder.ReadCsrFromFile($"{_matrixDirectory}\\circuit_1.mtx"));
    }

    [Benchmark] // (OperationsPerInvoke = 2)
    [ArgumentsSource(nameof(TestMatrices))]
    public object CsrLuFactorization(FactorizationTestRun TestMatrix)
    { 
        return TestMatrix.Matrix.LuFactorize();
    }

    [Benchmark(Baseline = true)] // (OperationsPerInvoke = 10)
    [ArgumentsSource(nameof(TestMatrices))]
    public object CsrLuFactorizationMarkowitz(FactorizationTestRun TestMatrix)
    {
        return TestMatrix.Matrix.LuFactorizeMarkowitz(0.001);
    }
}