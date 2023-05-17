using MathNet.Numerics.LinearAlgebra.Factorization;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;

namespace SparseMatrixAlgebra.Benchmarks.Factorization.RandomMathNET;

[DryJob] // запускать 1 раз
[MemoryDiagnoser]
// [RPlotExporter] // Должен быть установлен R и добавлен в PATH
[Config(typeof(Config))]
[HideColumns(Column.Gen0, Column.Gen1, Column.Gen2, Column.Error, Column.RatioSD)]
public class RandomMathNetFactorizationBenchmark
{ 
    private class Config : ManualConfig
    {
        public Config()
        {
            SummaryStyle =
                SummaryStyle.Default
                    .WithRatioStyle(RatioStyle.Percentage)
                    .WithTimeUnit(Perfolizer.Horology.TimeUnit.Second);
        }
    }

    LU<double>[] LUs = new LU<double>[NumberOfIterations];
    
    public const int NumberOfIterations = 8; // количество матриц одного типа

    public IEnumerable<object> TestMatrices()
    {
        yield return new RandomMathNetRun(1000, 10, NumberOfIterations);
        yield return new RandomMathNetRun(1000, 20, NumberOfIterations);
        yield return new RandomMathNetRun(1000, 30, NumberOfIterations);
        yield return new RandomMathNetRun(1000, 40, NumberOfIterations);
        yield return new RandomMathNetRun(1000, 50, NumberOfIterations);
        // yield return new RandomMathNetRun(1000, 60, NumberOfIterations);
        // yield return new RandomMathNetRun(1000, 70, NumberOfIterations);
        // yield return new RandomMathNetRun(1000, 80, NumberOfIterations);
        // yield return new RandomMathNetRun(1000, 90, NumberOfIterations);
        // yield return new RandomMathNetRun(1000, 100, NumberOfIterations);
    }

    [Benchmark(Baseline = true, OperationsPerInvoke = NumberOfIterations)]
    [ArgumentsSource(nameof(TestMatrices))]
    public void MathNetSparseLU(RandomMathNetRun TestMatrix)
    {
        for (int i = 0; i < NumberOfIterations; ++i)
        {
            LUs[i] = TestMatrix.SparseMatrixArray[i].LU();
        }
    }
    
    [Benchmark(OperationsPerInvoke = NumberOfIterations)]
    [ArgumentsSource(nameof(TestMatrices))]
    public void MathNetDenseLU(RandomMathNetRun TestMatrix)
    {
        for (int i = 0; i < NumberOfIterations; ++i)
        {
            LUs[i] = TestMatrix.DenseMatrixArray[i].LU();
        }
    }

}