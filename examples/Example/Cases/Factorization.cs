using SparseMatrixAlgebra.Utils;

namespace Example.Cases;

public static class Factorization
{
    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("########### Factorization ###########");
        Console.WriteLine();
        
        Console.WriteLine();
        var matrix = MatrixBuilder.CreateCsr(3, 3);
        matrix.SetElement(1, 1, 3);
        matrix.SetElement(1, 2, 4);
        // matrix8.SetElement(1, 3, 6);
        matrix.SetElement(2, 1, -2);
        matrix.SetElement(2, 2, 5);
        // matrix8.SetElement(2, 3, 17);
        matrix.SetElement(3, 1, 5);
        matrix.SetElement(3, 2, -1);
        matrix.SetElement(3, 3, 7);
        Console.WriteLine("matrix:");
        matrix.Print();

        Console.WriteLine();
        Console.WriteLine("matrix.LuFactorize()...");
        var LUP = matrix.LuFactorize();
        Console.WriteLine("L:");
        LUP.L.Print();
        
        Console.WriteLine();
        Console.WriteLine("U:");
        LUP.U.Print();

        Console.WriteLine();
        Console.WriteLine("P:");
        foreach (var i in LUP.P)
            Console.Write($"{i} ");
        Console.WriteLine();

        Console.WriteLine();
        Console.WriteLine("L*U:");
        LUP.L.MultiplyByMatrix(LUP.U).Print();
        
        Console.WriteLine();
        Console.WriteLine("P*A:");
        matrix.PermuteRows(LUP.P).Print();

        Console.WriteLine();
        Console.WriteLine("matrix.LuFactorizeMarkowitz()...");
        var LUPQ = matrix.LuFactorizeMarkowitz(0.001);
        Console.WriteLine("L:");
        LUPQ.L.Print();
        
        Console.WriteLine();
        Console.WriteLine("U:");
        LUPQ.U.Print();

        Console.WriteLine();
        Console.WriteLine("P:");
        foreach (var i in LUPQ.P)
            Console.Write($"{i} ");
        Console.WriteLine();
        
        Console.WriteLine();
        Console.WriteLine("Q:");
        foreach (var i in LUPQ.Q)
            Console.Write($"{i} ");
        Console.WriteLine();

        Console.WriteLine();
        Console.WriteLine("L*U:");
        LUPQ.L.MultiplyByMatrix(LUPQ.U).Print();
        
        Console.WriteLine();
        Console.WriteLine("P*A*Q:");
        matrix.PermuteRows(LUPQ.P).PermuteColumns(LUPQ.Q).Print();
        
        Console.WriteLine();
        Console.WriteLine("(P^-1)*L*U*(Q^-1):");
        var origin = LUPQ.GetOrigin();
        origin.Print();

        Console.WriteLine();
        Console.Write("matrix == LU.origin: ");
        Console.WriteLine(origin.Equals(matrix));
    }
}