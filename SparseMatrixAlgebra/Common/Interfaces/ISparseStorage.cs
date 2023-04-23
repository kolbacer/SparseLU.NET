namespace SparseMatrixAlgebra.Common.Interfaces;

public interface ISparseStorage<TKey>
    where TKey : IComparable
{
    TKey NumberOfNonzeroElements { get; }
    
    void PrintStorage();
    // ToDense();
}