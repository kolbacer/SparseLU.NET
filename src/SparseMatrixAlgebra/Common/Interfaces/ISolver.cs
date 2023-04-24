using System.Numerics;

namespace SparseMatrixAlgebra.Common.Interfaces;

public interface ISolver<TKey,TValue,TVector,TMatrix,TLU>
    where TKey : IBinaryInteger<TKey>
    where TValue: IBinaryNumber<TValue>
    where TVector: IVector<TKey, TValue>
    where TMatrix: IMatrix<TKey, TValue, TMatrix>
    where TLU: ILU<TKey, TValue, TVector, TMatrix>
{
    void AddRows(TKey augend, TKey addend, TValue coef);
    void SwapRows(TKey row1, TKey row2);
    TLU LuFactorize();
    TVector MultiplyByVector(TVector vector);
    TVector SolveSLAE(TVector b);
}