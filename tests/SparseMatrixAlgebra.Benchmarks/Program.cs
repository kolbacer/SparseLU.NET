using BenchmarkDotNet.Running;
using SparseMatrixAlgebra.Benchmarks.Factorization;

namespace SparseMatrixAlgebra.Benchmarks;

internal static class Program
{
    private static void Main(string[] args)
    {
        BenchmarkRunner.Run<CsrFactorizationBenchmark>();
    }
}