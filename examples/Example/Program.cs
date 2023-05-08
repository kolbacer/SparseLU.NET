using Example.Cases;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace Example;

internal static class Program
{
    private static void Main(string[] args)
    {
        SetupLogging();

        BasicMethods.Run();
        Factorization.Run();
        MatrixFromFile.Run();

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