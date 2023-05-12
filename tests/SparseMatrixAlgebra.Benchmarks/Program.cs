using BenchmarkDotNet.Running;
using SparseMatrixAlgebra.Benchmarks.Factorization.SpecificMatrices;

namespace SparseMatrixAlgebra.Benchmarks;

internal static class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<SpecificMatricesFactorizationBenchmark>();
    }
}