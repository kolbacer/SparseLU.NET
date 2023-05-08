using SparseMatrixAlgebra.Utils;

namespace Example.Cases;

public static class MatrixFromFile
{
    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("########### MatrixFromFile ###########");
        Console.WriteLine();
        
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        string matrixDirectory = $"{projectDirectory}\\TestFiles\\";
        string matrixFileName = "1138_bus.mtx";
        
        Console.WriteLine();
        Console.WriteLine("Matrix from file:");
        var matrix = MatrixBuilder.ReadCsrFromFile(matrixDirectory + matrixFileName);
        Console.WriteLine(matrix);
        // matrix.PrintToFile();

        Console.WriteLine();
        Console.WriteLine("LuFactorize...");
        var LUPQ = matrix.LuFactorize();
        Console.WriteLine("Nonzeros in L: " + LUPQ.L.NumberOfNonzeroElements);
        Console.WriteLine("Nonzeros in U: " + LUPQ.U.NumberOfNonzeroElements);
        var origin = LUPQ.GetOrigin();
        Console.Write("matrix == LU.origin: ");
        Console.WriteLine(origin.Equals(matrix)); // numerically unstable
        
        // LUPQ.L.PrintToFile();
        // LUPQ.U.PrintToFile();
        
        Console.WriteLine();
        Console.WriteLine("LuFactirizeMarkowitz...");
        var LUPQ1 = matrix.LuFactorizeMarkowitz(0.001);
        Console.WriteLine("Nonzeros in L: " + LUPQ1.L.NumberOfNonzeroElements);
        Console.WriteLine("Nonzeros in U: " + LUPQ1.U.NumberOfNonzeroElements);
        var origin1 = LUPQ1.GetOrigin();
        Console.Write("matrix == LU.origin: ");
        Console.WriteLine(origin1.Equals(matrix)); // ok
        
        // LUPQ1.L.PrintToFile();
        // LUPQ1.U.PrintToFile();
    }
}