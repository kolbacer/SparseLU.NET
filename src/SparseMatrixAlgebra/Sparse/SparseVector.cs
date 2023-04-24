using System.Numerics;
using SparseMatrixAlgebra.Common.Interfaces;

namespace SparseMatrixAlgebra.Sparse;

public abstract class SparseVector<TKey,TValue> : IVector<TKey,TValue>, ISparseStorage<TKey>
    where TKey : IBinaryInteger<TKey>
    where TValue: IBinaryNumber<TValue>
{
    public abstract TKey Length { get; }
    public abstract bool IsColumn { get; }
    public abstract void Print();
    
    public abstract TKey NumberOfNonzeroElements { get; }
    public abstract void PrintStorage();
}