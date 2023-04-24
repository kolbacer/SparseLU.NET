namespace SparseMatrixAlgebra.Common.Exceptions;

public class IncompatibleDimensionsException : Exception
{
    public IncompatibleDimensionsException() : base() {}
    
    public IncompatibleDimensionsException(string message) : base(message) {}
}