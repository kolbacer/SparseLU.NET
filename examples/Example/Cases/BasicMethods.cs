using SparseMatrixAlgebra.Sparse;
using SparseMatrixAlgebra.Sparse.CSR;
using SparseMatrixAlgebra.Utils;

namespace Example.Cases;

public static class BasicMethods
{
    public static void Run()
    {
        Console.WriteLine();
        Console.WriteLine("########### BasicMethods ###########");
        Console.WriteLine();
        
        var matrix = MatrixBuilder.CreateCsr(5, 5);
        matrix.SetElement(1, 1, 5);
        matrix.SetElement(1, 2, 7);
        matrix.SetElement(3, 2, 9);
        matrix.SetElement(4, 5, 2);

        Console.WriteLine("SparseMatrix output:");
        matrix.Print();

        Console.WriteLine("\nExample output:");
        for (int i = 1; i <= 5; ++i)
        {
            for (int j = 1; j <= 5; ++j) 
                Console.Write($"{matrix.GetElement(i,j)} ");
            Console.WriteLine();
        }


        SparseMatrix<int, double> matrix1 = new SparseMatrixCsr(5, 5);
        matrix1.SetElement(1, 1, 5);
        matrix1.SetElement(1, 2, 7.43);
        matrix1.SetElement(1, 3, 84.235);
        matrix1.SetElement(3, 2, 9.01);
        matrix1.SetElement(3, 5, 342.0156);
        matrix1.SetElement(4, 5, 2.32);
        matrix1.SetElement(5, 1, 82.442);
        matrix1.SetElement(5, 4, 6.28);
        
        Console.WriteLine();
        Console.WriteLine("SparseMatrix output:");
        matrix1.Print();
        
        Console.WriteLine("\nExample output:");
        for (int i = 1; i <= 5; ++i)
        {
            for (int j = 1; j <= 5; ++j) 
                Console.Write($"{matrix1.GetElement(i,j)} ");
            Console.WriteLine();
        }
        
        Console.WriteLine("\nStorage:");
        matrix1.PrintStorage();

        Console.WriteLine();
        Console.WriteLine($"Rows: {matrix1.Rows}");
        Console.WriteLine($"Columns: {matrix1.Columns}");
        Console.WriteLine($"Nonzeros: {matrix1.NumberOfNonzeroElements}");

        Console.WriteLine();
        var matrix2 = matrix1.Copy();
        matrix2.SetElement(3,2,7.77);
        matrix2.SetElement(4,3,-5.55);
        matrix2.SetElement(1,3,0);
        Console.WriteLine("Copied matrix:");
        matrix2.Print();

        Console.WriteLine();
        Console.WriteLine("Original matrix:");
        matrix1.Print();
        
        Console.WriteLine();
        matrix1.SwapColumns(2,5);
        Console.WriteLine("Swapped columns 2 and 5:");
        matrix1.Print();

        Console.WriteLine();
        matrix1.SwapRows(3,5);
        Console.WriteLine("Swapped rows 3 and 5:");
        matrix1.Print();

        Console.WriteLine();
        matrix1.AddRows(2, 1, 2);
        Console.WriteLine("row2 += row1*2");
        matrix1.Print();
        
        Console.WriteLine();
        matrix1.SwapColumns(4,5);
        Console.WriteLine("Swapped columns 4 and 5:");
        matrix1.Print();
        
        Console.WriteLine();
        matrix1.AddRows(2, 5, 0.5);
        Console.WriteLine("row2 += row5*0.5");
        matrix1.Print();
        
        Console.WriteLine();
        matrix1.AddRows(4, 4, 2);
        Console.WriteLine("row4 += row4*2");
        matrix1.Print();
        
        Console.WriteLine();
        matrix1.AddRows(2, 2, -1);
        Console.WriteLine("row2 += row2*(-1)");
        matrix1.Print();

        Console.WriteLine();
        var matrix3 = matrix1.Transposed();
        Console.WriteLine("Transposed:");
        matrix3.Print();

        Console.WriteLine();
        var matrix4 = MatrixBuilder.CreateCsr(3, 5);
        matrix4.SetElement(1, 1, 5);
        matrix4.SetElement(1, 3, 8);
        matrix4.SetElement(2, 2, 4);
        matrix4.SetElement(2, 4, 4.6);
        matrix4.SetElement(2, 5, 77);
        matrix4.SetElement(3, 4, 0.13);
        Console.WriteLine("Non-square matrix:");
        matrix4.Print();

        Console.WriteLine();
        Console.WriteLine("Transposed:");
        matrix4.Transposed().Print();
        
        Console.WriteLine();
        var vector = MatrixBuilder.CreateCsrVector(5);
        vector.SetElement(1, 3);
        vector.SetElement(3, 4.56);
        vector.SetElement(4, 8);
        Console.WriteLine("Vector:");
        vector.Print();

        Console.WriteLine();
        vector.IsColumn = true;
        Console.WriteLine("As column:");
        vector.Print();

        Console.WriteLine();
        Console.WriteLine("Example output:");
        for (int i = 1; i <= vector.Length; ++i)
        {
            Console.Write($"{vector.GetElement(i)} ");
        }
        Console.WriteLine();

        Console.WriteLine();
        Console.WriteLine("Storage:");
        vector.PrintStorage();

        Console.WriteLine();
        var vector1 = MatrixBuilder.CreateCsrVector(5);
        vector1.SetElement(2,3.5);
        vector1.SetElement(3, 1);
        vector1.SetElement(4, 2);
        Console.WriteLine("Vector1");
        vector1.Print();

        Console.WriteLine();
        Console.WriteLine("Vector(row)*Vector1(column) = " + vector.MultiplyRowByColumn(vector1));
        Console.WriteLine("Vector1(row)*Vector(column) = " + vector1.MultiplyRowByColumn(vector));

        Console.WriteLine();
        var matrix5 = vector.MultiplyColumnByRow(vector1);
        Console.WriteLine("Vector(column)*Vector1(row)");
        matrix5.Print();

        Console.WriteLine();
        var vector2 = MatrixBuilder.CreateCsrVector(3);
        vector2.SetElement(1, 5.2);
        vector2.SetElement(3, 4);
        Console.WriteLine("Vector2:");
        vector2.Print();
        
        Console.WriteLine();
        Console.WriteLine("matrix6 = Vector1(column)*Vector2(row):");
        var matrix6 = vector1.MultiplyColumnByRow(vector2);
        matrix6.Print();
        
        Console.WriteLine();
        Console.WriteLine("Vector2(column)*Vector1(row):");
        vector2.MultiplyColumnByRow(vector1).Print();

        Console.WriteLine();
        var matrix7 = MatrixBuilder.CsrOfArray(new double[,]
        {
            { 0, 5, 7, 0, 3 },
            { 4, 0, 6, 0, 0 },
            { 0, 0, 9, 1, 8 }
        });
        Console.WriteLine("matrix7:");
        matrix7.Print();

        Console.WriteLine();
        Console.WriteLine("matrix7*vector1:");
        matrix7.MultiplyByVector(vector1).Print();
        
        Console.WriteLine();
        Console.WriteLine("matrix7*vector1:");
        matrix7.MultiplyByVector(vector1).Print();
        
        Console.WriteLine();
        Console.WriteLine("vector2*matrix7:");
        vector2.MultiplyRowByMatrix(matrix7).Print();
        
        Console.WriteLine();
        Console.WriteLine("vector2*matrix7:");
        vector2.MultiplyRowByMatrix(matrix7).Print();

        Console.WriteLine();
        Console.WriteLine("matrix7*matrix6:");
        matrix7.MultiplyByMatrix(matrix6).Print();
        
        Console.WriteLine();
        Console.WriteLine("matrix6*matrix7:");
        matrix6.MultiplyByMatrix(matrix7).Print();
        
        Console.WriteLine();
        var vector3 = MatrixBuilder.CreateCsrVector(18);
        vector3.SetElement(3, 2);
        vector3.SetElement(5, 1);
        vector3.SetElement(7, 8);
        vector3.SetElement(10, 7);
        vector3.SetElement(13, 6);
        vector3.SetElement(18, 3);
        Console.WriteLine("Vector3:");
        vector3.Print();
        
        Console.WriteLine();
        vector3.SwapElements(5, 13);
        Console.WriteLine("Vector3.SwapElements(5, 13):");
        vector3.Print();
        
        Console.WriteLine();
        vector3.SwapElements(6, 13);
        Console.WriteLine("Vector3.SwapElements(6, 13):");
        vector3.Print();
        
        Console.WriteLine();
        vector3.SwapElements(6, 9);
        Console.WriteLine("Vector3.SwapElements(6, 9):");
        vector3.Print();
        
        Console.WriteLine();
        vector3.SwapElements(10, 13);
        Console.WriteLine("Vector3.SwapElements(10, 13):");
        vector3.Print();
    }
}