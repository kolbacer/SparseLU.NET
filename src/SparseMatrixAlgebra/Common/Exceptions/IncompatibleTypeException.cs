namespace SparseMatrixAlgebra.Common.Exceptions;

public class IncompatibleTypeException : Exception
{
    public IncompatibleTypeException() : base() {}
    
    public IncompatibleTypeException(string message) : base(message) {}
}