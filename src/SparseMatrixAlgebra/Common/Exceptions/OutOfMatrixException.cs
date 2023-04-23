namespace SparseMatrixAlgebra.Common.Exceptions;

public class OutOfMatrixException : Exception
{
    public OutOfMatrixException() : base() {}
    
    public OutOfMatrixException(string message) : base(message) {}
}