using Microsoft.Extensions.Logging;

namespace SparseMatrixAlgebra.Common.Logging;

public class Logger
{
    private readonly ILogger? _logger;

    public Logger(ILogger? logger = null)
    {
        _logger = logger;
    }
        
    public void Trace(string? message, params object?[] args) => _logger?.LogTrace(message, args);
    public void Debug(string? message, params object?[] args) => _logger?.LogDebug(message, args);
    public void Info(string? message, params object?[] args) => _logger?.LogInformation(message, args);
    public void Warn(string? message, params object?[] args) => _logger?.LogWarning(message, args);
    public void Error(string? message, params object?[] args) => _logger?.LogError(message, args);
    public void Critical(string? message, params object?[] args) => _logger?.LogCritical(message, args);
        
    public void Print(string? message, params object?[] args) => Info(message, args);
}