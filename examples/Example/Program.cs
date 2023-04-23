using Microsoft.Extensions.Logging;
using SparseMatrixAlgebra.Sparse;
using SparseMatrixAlgebra.Sparse.CSR;
using Serilog;
using Serilog.Extensions.Logging;
using SparseMatrixAlgebra.Utils;

namespace Example;

internal static class Program
{
    private static void Main(string[] args)
    {
        SetupLogging();
        
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
        matrix.SetElement(1, 1, 5);
        matrix.SetElement(1, 2, 7.43);
        matrix.SetElement(1, 3, 84.235);
        matrix.SetElement(3, 2, 9.01);
        matrix.SetElement(3, 5, 342.0156);
        matrix.SetElement(4, 5, 2.32);
        matrix.SetElement(5, 1, 82.442);
        matrix.SetElement(5, 4, 6.28);
        
        Console.WriteLine();
        Console.WriteLine("SparseMatrix output:");
        matrix.Print();
        
        Console.WriteLine("\nExample output:");
        for (int i = 1; i <= 5; ++i)
        {
            for (int j = 1; j <= 5; ++j) 
                Console.Write($"{matrix.GetElement(i,j)} ");
            Console.WriteLine();
        }
        
        Console.WriteLine("\nStorage:");
        matrix.PrintStorage();

        Console.WriteLine();
        Console.WriteLine($"Rows: {matrix.Rows}");
        Console.WriteLine($"Columns: {matrix.Columns}");
        Console.WriteLine($"Nonzeros: {matrix.NumberOfNonzeroElements}");

        Console.ReadLine();
    }

    private static void SetupLogging()
    {
        var consoleLogger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .CreateLogger();
        
        var consoleLoggerMicrosoft = new SerilogLoggerFactory(consoleLogger)
            .CreateLogger<SparseMatrixAlgebra.Common.Logging.Logger>();
        
        string projectDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
        string logPath = $"{projectDirectory}\\logs\\log.txt";
        
        var fileLogger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.File(
                path: logPath,
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:HH:mm:ss} [{Level:u3}]{NewLine}{Message:lj}{NewLine}{Exception}")
            .CreateLogger();
        
        var fileLoggerMicrosoft = new SerilogLoggerFactory(fileLogger)
            .CreateLogger<SparseMatrixAlgebra.Common.Logging.Logger>();
        
        SparseMatrixAlgebra.Common.Logging.Loggers.InitLoggers(
            consoleLogger: consoleLoggerMicrosoft,
            fileLogger: fileLoggerMicrosoft);
    }
}