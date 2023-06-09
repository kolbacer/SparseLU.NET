﻿using System.Numerics;
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
    public override string ToString() => $"{Rows}x{Columns} ({NumberOfNonzeroElements} nnz)";
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
    /// Поменять местами 2 строки в матрице. Индексация с 1.
    /// </summary>
    public abstract void SwapRows(TKey row1, TKey row2);
    
    /// <summary>
    /// Поменять местами 2 столбца в матрице. Индексация с 1.
    /// </summary>
    public abstract void SwapColumns(TKey col1, TKey col2);

    /// <summary>
    /// Создать транспонированную матрицу
    /// </summary>
    public abstract SparseMatrix<TKey, TValue> Transposed();
    public abstract SparseLU<TKey,TValue> LuFactorize();
    
    /// <summary>
    /// Умножить матрицу на вектор-столбец
    /// </summary>
    public abstract SparseVector<TKey,TValue> MultiplyByVector(SparseVector<TKey,TValue> vector);
    
    /// <summary>
    /// Умножить матрицу на матрицу
    /// </summary>
    public abstract SparseMatrix<TKey,TValue> MultiplyByMatrix(SparseMatrix<TKey,TValue> matrix);
    public abstract SparseVector<TKey,TValue> SolveSLAE(SparseVector<TKey,TValue> b);
}