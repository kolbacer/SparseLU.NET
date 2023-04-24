using System.Numerics;

namespace SparseMatrixAlgebra.Common.Interfaces;

public interface ILU<TKey,TValue,TVector,TMatrix>
    where TKey : IBinaryInteger<TKey>
    where TValue: IBinaryNumber<TValue>
    where TVector: IVector<TKey, TValue>
    where TMatrix: IMatrix<TKey,TValue,TMatrix>
{
    TMatrix L { get; }
    TMatrix U { get; }
    
    TVector SolveLy_b(TVector b);
    TVector SolveUx_y(TVector y);
}