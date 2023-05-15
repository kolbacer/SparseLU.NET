using SparseMatrixAlgebra.Utils;

namespace Example.Cases;

public static class RandomMatrixGeneration
{
    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("########### RandomMatrixGeneration ###########");
        Console.WriteLine();

        Console.WriteLine();
        Console.WriteLine("GenerateRandomCsr(5, 10, 20)");
        var matrix = MatrixBuilder.GenerateRandomCsr(5, 10, 20);
        matrix.Print();
        
        Console.WriteLine();
        Console.WriteLine("GenerateRandomCsr(10, 5, 20)");
        matrix = MatrixBuilder.GenerateRandomCsr(10, 5, 20);
        matrix.Print();
        
        Console.WriteLine();
        Console.WriteLine("GenerateRandomCsr(3, 3, 5, 999)");
        matrix = MatrixBuilder.GenerateRandomCsr(3, 3, 5, 999);
        matrix.Print();
        
        Console.WriteLine();
        Console.WriteLine("GenerateRandomCsr(3, 3, 5, 999)");
        matrix = MatrixBuilder.GenerateRandomCsr(3, 3, 5, 999);
        matrix.Print();
        
        Console.WriteLine();
        Console.WriteLine("GenerateRandomCsr(10, 10, 30)");
        matrix = MatrixBuilder.GenerateRandomCsr(10, 10, 30);
        matrix.Print();
    }
}