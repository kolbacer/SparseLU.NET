using BenchmarkDotNet.Running;
using SparseMatrixAlgebra.Benchmarks.Factorization.RandomMatrices;

namespace SparseMatrixAlgebra.Benchmarks;

internal static class Program
{
    private static void Main(string[] args)
    {
        // BenchmarkRunner.Run<SpecificMatricesFactorizationBenchmark>();
        BenchmarkRunner.Run<RandomMatricesFactorizationBenchmark>();
    }
}