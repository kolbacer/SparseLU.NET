using BenchmarkDotNet.Running;
using SparseMatrixAlgebra.Benchmarks.Factorization.RandomMathNET;
using SparseMatrixAlgebra.Benchmarks.Factorization.RandomMatrices;
using SparseMatrixAlgebra.Benchmarks.Factorization.SpecificMatrices;

namespace SparseMatrixAlgebra.Benchmarks;

internal static class Program
{
    private static void Main(string[] args)
    {
        // BenchmarkRunner.Run<SpecificMatricesFactorizationBenchmark>();
        // BenchmarkRunner.Run<RandomMatricesFactorizationBenchmark>();
        BenchmarkRunner.Run<RandomMatricesFactorizationParallelBenchmark>();
        // BenchmarkRunner.Run<RandomMathNetFactorizationBenchmark>();
    }
}