namespace SparseMatrixAlgebra.Common.Interfaces;

public interface IVector<TKey,TValue>
    where TKey : IComparable
    where TValue: IComparable
{
    TKey Length { get; }
    bool IsColumn { get; }
    void Print();
}