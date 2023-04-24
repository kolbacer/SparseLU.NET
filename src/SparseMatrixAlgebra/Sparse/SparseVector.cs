using System.Numerics;
using SparseMatrixAlgebra.Common.Interfaces;

namespace SparseMatrixAlgebra.Sparse;

public abstract class SparseVector<TKey,TValue> : IVector<TKey,TValue>, ISparseStorage<TKey>
    where TKey : IBinaryInteger<TKey>
    where TValue: IBinaryNumber<TValue>
{
    public abstract TKey Length { get; }
    public abstract bool IsColumn { get; set; }
    public abstract TKey NumberOfNonzeroElements { get; }
    public abstract TValue GetElement(TKey index);
    public abstract void SetElement(TKey index, TValue value);
    public abstract void Print();
    public abstract void Print(bool asColumn);
    public abstract void PrintStorage();
}