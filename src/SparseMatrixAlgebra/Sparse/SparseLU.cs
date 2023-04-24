using System.Numerics;
using SparseMatrixAlgebra.Common.Interfaces;

namespace SparseMatrixAlgebra.Sparse;

public abstract class SparseLU<TKey,TValue> : ILU<TKey,TValue,SparseVector<TKey,TValue>,SparseMatrix<TKey,TValue>>
    where TKey : IBinaryInteger<TKey>
    where TValue: IBinaryNumber<TValue>
{
    public abstract SparseMatrix<TKey,TValue> L { get; }
    public abstract SparseMatrix<TKey,TValue> U { get; }
    
    public abstract SparseVector<TKey,TValue> SolveLy_b(SparseVector<TKey,TValue> b);
    public abstract SparseVector<TKey,TValue> SolveUx_y(SparseVector<TKey,TValue> y);
}