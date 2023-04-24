using System.Numerics;
using SparseMatrixAlgebra.Common.Interfaces;

namespace SparseMatrixAlgebra.Sparse;

public abstract class SparseMatrix<TKey,TValue> : IMatrix<TKey,TValue,SparseMatrix<TKey,TValue>>, 
                                                  ISolver<TKey, TValue,
                                                      SparseVector<TKey,TValue>, 
                                                      SparseMatrix<TKey,TValue>,
                                                      SparseLU<TKey,TValue>>
    where TKey : IBinaryInteger<TKey>
    where TValue: IBinaryNumber<TValue>
{
    protected SparseStorage<TKey> Storage;
    public TKey Rows { get => Storage.Rows; }
    public TKey Columns { get => Storage.Columns; }

    public abstract TValue GetElement(TKey i, TKey j);
    public abstract void SetElement(TKey i, TKey j, TValue value);
    public abstract void Print();
    public abstract SparseMatrix<TKey,TValue> Copy();
    
    public TKey NumberOfNonzeroElements { get => Storage.NumberOfNonzeroElements; }
    public void PrintStorage() => Storage.PrintStorage();
    
    /// <summary>
    /// Сложение строк матрицы.
    /// </summary>
    /// <param name="augend">Строка, к которой прибавляем</param>
    /// <param name="addend">Строка, которую прибавляем</param>
    /// <param name="coef">Коэффициент домножения</param>
    public abstract void AddRows(TKey augend, TKey addend, TValue coef);
    
    /// <summary>
    /// Сложение строк матрицы.
    /// </summary>
    /// <param name="augend">Строка, к которой прибавляем</param>
    /// <param name="addend">Строка, которую прибавляем</param>
    public abstract void AddRows(TKey augend, TKey addend);
    
    /// <summary>
    /// Поменять местами 2 строки в матрице
    /// </summary>
    public abstract void SwapRows(TKey row1, TKey row2);
    public abstract SparseLU<TKey,TValue> LuFactorize();
    public abstract SparseVector<TKey,TValue> MultiplyByVector(SparseVector<TKey,TValue> vector);
    public abstract SparseVector<TKey,TValue> SolveSLAE(SparseVector<TKey,TValue> b);
}