namespace SparseMatrixAlgebra.Common.Exceptions;

public class OutOfVectorException : Exception
{
    public OutOfVectorException() : base() {}
    
    public OutOfVectorException(string message) : base(message) {}
}