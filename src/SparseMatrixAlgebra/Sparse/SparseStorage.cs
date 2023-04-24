using SparseMatrixAlgebra.Common.Interfaces;

namespace SparseMatrixAlgebra.Sparse;

public abstract class SparseStorage<TKey> : ISparseStorage<TKey>
    where TKey : IComparable
{
    public TKey Rows { get; protected set; }
    public TKey Columns { get; protected set; }
    public abstract TKey NumberOfNonzeroElements { get; protected set; }
    public abstract void PrintStorage();
    public abstract SparseStorage<TKey> Copy();
}