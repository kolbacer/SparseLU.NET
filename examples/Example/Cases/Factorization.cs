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

        Console.WriteLine();
        var matrix2 = MatrixBuilder.CsrOfArray(new double[,]
        {
            { 0, 0, 1.69, 0.865, 0, 1.37, 0, 0, 0, 0, 0, 0, 0, 0, -1.25 },
            { -0.973, 0, 0.555, 0, 0.905, 0, 0.467, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1.19, 0, 0, 0.337, 0, -1.61, 0, 0, 0, -0.417, 0, -1.34, -1.04, -1.73 },
            { 0, 0, 0, 0, -1.11, 0, 0.883, 1.09, 0, 0, 0, 0, 0, -0.284, 0 },
            { 0.693, 0.506, 0.0892, 0, 0.748, 0, 0, -1.29, -0.701, 0, 0, 0, 0, 0, -0.408 },
            { 0, 0, 0, 0.431, 0, 0, 0, 0.305, 0, 0.393, -1.38, 0, 1.78, 0, 0 },
            { -1.52, -0.975, 0, 0, 0, -0.371, 0, 0, 1.86, 0, 0.736, 0, 1.05, 0, 0 },
            { 1.97, 0, 0, 0, 0, 0, 0.805, 1.46, 1.03, 0, 1.91, -0.82, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, -0.157, 0.23, 0, 0, 0.756, 0, 1.95, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.34, 0, -0.337, 0, 0 },
            { 0.509, -0.553, 0, 0, 0, -1.19, 1.74, -0.598, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, -1.61, -1.62, 0, 0, 1.14, 0, 0, 0 },
            { 0, 0, 0.947, -1.69, 1.76, 0, 0, 0, 0, 0, 0, 0, 0, 1.71, 0 },
            { 0, 0, 1.49, 0, 0, 0, 0, -1.36, 0, 0, 0, 0, 0, -1.08, 0 },
            { -0.876, 0, 0, 0, 1.3, 0, 1.6, 0, -0.517, 0, 0, 0, 0, 0, 0 }
        });
        Console.WriteLine("matrix2:");
        matrix2.Print();
        
        Console.WriteLine();
        Console.WriteLine("matrix2.LuFactorize()...");
        var LUP2 = matrix2.LuFactorize();
        Console.WriteLine("L:");
        LUP2.L.Print();
        
        Console.WriteLine();
        Console.WriteLine("U:");
        LUP2.U.Print();

        Console.WriteLine();
        Console.WriteLine("P:");
        foreach (var i in LUP2.P)
            Console.Write($"{i} ");
        Console.WriteLine();

        Console.WriteLine();
        Console.WriteLine("matrix2.LuFactorizeMarkowitz()...");
        LUP2 = matrix2.LuFactorizeMarkowitz();
        Console.WriteLine("L:");
        LUP2.L.Print();
        
        Console.WriteLine();
        Console.WriteLine("U:");
        LUP2.U.Print();

        Console.WriteLine();
        Console.WriteLine("P:");
        foreach (var i in LUP2.P)
            Console.Write($"{i} ");
        Console.WriteLine();
        
        Console.WriteLine();
        Console.WriteLine("Q:");
        foreach (var i in LUP2.Q)
            Console.Write($"{i} ");
        Console.WriteLine();
        
        Console.WriteLine();
        Console.WriteLine("matrix2.LuFactorizeMarkowitz2()...");
        LUP2 = matrix2.LuFactorizeMarkowitz2();
        Console.WriteLine("L:");
        LUP2.L.Print();
        
        Console.WriteLine();
        Console.WriteLine("U:");
        LUP2.U.Print();

        Console.WriteLine();
        Console.WriteLine("P:");
        foreach (var i in LUP2.P)
            Console.Write($"{i} ");
        Console.WriteLine();
        
        Console.WriteLine();
        Console.WriteLine("Q:");
        foreach (var i in LUP2.Q)
            Console.Write($"{i} ");
        Console.WriteLine();
    }
}