using Microsoft.Extensions.Logging;

namespace SparseMatrixAlgebra.Common.Logging;

public static class Loggers
{
    public static Logger ConsoleLogger = new Logger();
    public static Logger FileLogger = new Logger();
    
    public static void InitLoggers(ILogger? consoleLogger = null, ILogger? fileLogger = null)
    {
        if (consoleLogger is not null)
        {
            ConsoleLogger = new Logger(consoleLogger);
        }

        if (fileLogger is not null)
        {
            FileLogger = new Logger(fileLogger);
        }
    }
}