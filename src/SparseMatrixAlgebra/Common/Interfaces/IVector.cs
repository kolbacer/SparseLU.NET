using System.Numerics;

namespace SparseMatrixAlgebra.Common.Interfaces;

public interface IVector<TKey,TValue>
    where TKey : IBinaryInteger<TKey>
    where TValue: IBinaryNumber<TValue>
{
    TKey Length { get; }
    bool IsColumn { get; }
    void Print();
}