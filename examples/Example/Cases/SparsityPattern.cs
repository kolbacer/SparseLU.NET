using SparseMatrixAlgebra.Utils;

namespace Example.Cases;

public static class SparsityPattern
{
    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("########### SparsityPattern ###########");
        Console.WriteLine();

        Console.WriteLine();
        var matrix = MatrixBuilder.CsrOfArray(new double[,]
        {
            { 0, 5, 7, 0, 3, 2 },
            { 4, 0, 6, 0, 0, 1 },
            { 0, 0, 9, 1, 8, 0 },
            { 4, 0, 3, 1, 3, 0 },
            { 0, 0, 0, 0, 0, 2 },
            { 3, 4, 0, 0, 8, 5 },
        });
        Console.WriteLine("matrix:");
        matrix.Print();

        Console.WriteLine();
        Console.WriteLine("Sparsity pattern 3x3");
        var pattern = matrix.GetSparsityPattern(3);
        for (int i = 0; i < pattern.GetLength(0); ++i)
        {
            for (int j = 0; j < pattern.GetLength(1); ++j)
                Console.Write($"{pattern[i, j],5:0.##} ");
            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine("Sparsity pattern 2x2");
        pattern = matrix.GetSparsityPattern(2);
        for (int i = 0; i < pattern.GetLength(0); ++i)
        {
            for (int j = 0; j < pattern.GetLength(1); ++j)
                Console.Write($"{pattern[i, j],5:0.##} ");
            Console.WriteLine();
        }

        Console.WriteLine();
        Console.WriteLine("Sparsity pattern 4x4");
        pattern = matrix.GetSparsityPattern(4);
        for (int i = 0; i < pattern.GetLength(0); ++i)
        {
            for (int j = 0; j < pattern.GetLength(1); ++j)
                Console.Write($"{pattern[i, j],5:0.##} ");
            Console.WriteLine();
        }
    }
}