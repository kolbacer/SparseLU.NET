using System.Numerics;

namespace SparseMatrixAlgebra.Common.Interfaces;

public interface ISparseStorage<TKey>
    where TKey : IBinaryInteger<TKey>
{
    TKey NumberOfNonzeroElements { get; }
    
    void PrintStorage();
    // ToDense();
}