namespace SparseMatrixAlgebra.Common.Exceptions;

public class SingularMatrixException : Exception
{
    public SingularMatrixException() : base() {}
    
    public SingularMatrixException(string message) : base(message) {}
}