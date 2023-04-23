namespace SparseMatrixAlgebra.Common.Interfaces;

public interface IMatrix<TKey,TValue,TMatrix>
    where TKey : IComparable
    where TValue: IComparable
    where TMatrix: IMatrix<TKey,TValue,TMatrix>
{
    TKey Rows { get; }
    TKey Columns { get; }
    
    /// <summary>
    /// Получить элемент матрицы (индексация с 1).
    /// </summary>
    /// <param name="row">строка</param>
    /// <param name="column">столбец</param>
    TValue GetElement(TKey i, TKey j);
    
    /// <summary>
    /// Присвоить значение элементу матрицы (индексация с 1).
    /// </summary>
    /// <param name="row">строка</param>
    /// <param name="column">столбец</param>
    /// <param name="value">значение</param>
    void SetElement(TKey i, TKey j, TValue value);
    
    /// <summary>
    /// Выводит в лог матрицу в плотном виде.
    /// </summary>
    void Print();
    TMatrix Copy();
}