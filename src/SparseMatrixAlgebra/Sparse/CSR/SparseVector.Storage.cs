using SparseMatrixAlgebra.Common.Exceptions;

namespace SparseMatrixAlgebra.Sparse.CSR;

// internal методы для работы с хранилищем
public partial class SparseVector
{

    /// <summary>
    /// Создать вектор на основе массивов индексов и значений.
    /// НЕ создает копию хранилища.
    /// </summary>
    internal SparseVector(stype length, bool isColumn, List<stype> indices, List<vtype> values) : this(length, isColumn)
    {
        Indices = indices;
        Values = values;
    }
    
    internal stype GetIndexAt(stype i)
    {
        if (i < 0 || i >= Length) throw new OutOfVectorException();
        return Indices[i];
    }

    internal vtype GetValueAt(stype i)
    {
        if (i < 0 || i >= Length) throw new OutOfVectorException();
        return Values[i];
    }

    internal Element GetElementAt(stype i)
    {
        if (i < 0 || i >= Length) throw new OutOfVectorException();
        return new Element(Indices[i], Values[i]);
    }

    internal void SetIndexAt(stype i, stype index)
    {
        if (i < 0 || i >= Length) throw new OutOfVectorException();
        Indices[i] = index;
    }

    internal void SetValueAt(stype i, vtype value)
    {
        if (i < 0 || i >= Length) throw new OutOfVectorException();
        Values[i] = value;
    }

    internal void SetElementAt(stype i, Element element)
    {
        if (i < 0 || i >= Length) throw new OutOfVectorException();
        Indices[i] = element.Index;
        Values[i] = element.Value;
    }

    internal void AddIndex(stype index)
    {
        Indices.Add(index);
    }

    internal void AddValue(vtype value)
    {
        Values.Add(value);
    }

    internal void AddElement(Element element)
    {
        Indices.Add(element.Index);
        Values.Add(element.Value);
    }

    internal void InsertIndex(stype i, stype index)
    {
        if (i < 0 || i >= Length) throw new OutOfVectorException();
        Indices.Insert(i, index);
    }

    internal void InsertValue(stype i, vtype value)
    {
        if (i < 0 || i >= Length) throw new OutOfVectorException();
        Values.Insert(i, value);
    }

    internal void InsertElement(stype i, Element element)
    {
        if (i < 0 || i >= Length) throw new OutOfVectorException();
        Indices.Insert(i, element.Index);
        Values.Insert(i, element.Value);
    }

    internal void RemoveIndexAt(stype i)
    {
        if (i < 0 || i >= Length) throw new OutOfVectorException();
        Indices.RemoveAt(i);
    }

    internal void RemoveValueAt(stype i)
    {
        if (i < 0 || i >= Length) throw new OutOfVectorException();
        Values.RemoveAt(i);
    }

    internal void RemoveElementAt(stype i)
    {
        if (i < 0 || i >= Length) throw new OutOfVectorException();
        Indices.RemoveAt(i);
        Values.RemoveAt(i);
    }
    
    internal Element this[stype i]
    {
        get => GetElementAt(i);
        set => SetElementAt(i, value);
    }

    /// <summary>
    /// Объект-значение, представляющий элемент вектора 
    /// </summary>
    public class Element
    {
        public stype Index { get; }
        public vtype Value { get; }

        public Element(stype index, vtype value)
        {
            Index = index;
            Value = value;
        }
    }
}