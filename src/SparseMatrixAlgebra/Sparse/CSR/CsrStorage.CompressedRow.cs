using SparseMatrixAlgebra.Common.Exceptions;

namespace SparseMatrixAlgebra.Sparse.CSR;

internal partial class CsrStorage
{
    /// <summary>
    /// Абстракция для работы со строкой матрицы в формате CSR
    /// </summary>
    public class CompressedRow
    {
        private CsrStorage storage; // corresponding storage
        public int Count { get => ColumnIndices.Count; }
        
        /// <summary>
        /// Номер строки
        /// </summary>
        public stype Index { get; }
        
        /// <summary>
        /// Массив, содержащий столбцовые индексы ненулевых элементов строки матрицы
        /// </summary>
        public List<stype> ColumnIndices { get; }
        
        /// <summary>
        /// Массив, содержащий значения ненулевых элементов строки матрицы
        /// </summary>
        public List<vtype> Values { get; }
        
        public CompressedRow(CsrStorage storage, stype rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= storage.Rows) throw new OutOfMatrixException();
            this.storage = storage;
            Index = rowIndex;
            ColumnIndices = storage.ColumnIndexRows[rowIndex];
            Values = storage.ValueRows[rowIndex];
        }

        public stype GetColumnIndexAt(stype i)
        {
            if (i < 0 || i >= storage.Columns) throw new OutOfMatrixException();
            return ColumnIndices[i];
        }

        public vtype GetValueAt(stype i)
        {
            if (i < 0 || i >= storage.Columns) throw new OutOfMatrixException();
            return Values[i];
        }

        public Element GetElementAt(stype i)
        {
            if (i < 0 || i >= storage.Columns) throw new OutOfMatrixException();
            return new Element(Index, ColumnIndices[i], Values[i]);
        }
        
        public void SetColumnIndexAt(stype i, stype columnIndex)
        {
            if (i < 0 || i >= storage.Columns) throw new OutOfMatrixException();
            ColumnIndices[i] = columnIndex;
        }

        public void SetValueAt(stype i, vtype value)
        {
            if (i < 0 || i >= storage.Columns) throw new OutOfMatrixException();
            Values[i] = value;
        }

        public void SetElementAt(stype i, Element element)
        {
            if (i < 0 || i >= storage.Columns) throw new OutOfMatrixException();
            ColumnIndices[i] = element.ColumnIndex;
            Values[i] = element.Value;
        }

        public void AddColumnIndex(stype columnIndex)
        {
            ColumnIndices.Add(columnIndex);
        }
        
        public void AddValue(vtype value)
        {
            Values.Add(value);
        }

        public void AddElement(Element element)
        {
            ColumnIndices.Add(element.ColumnIndex);
            Values.Add(element.Value);
        }

        public void InsertColumnIndex(stype i, stype columnIndex)
        {
            if (i < 0 || i >= storage.Columns) throw new OutOfMatrixException();
            ColumnIndices.Insert(i, columnIndex);
        }
        
        public void InsertValue(stype i, vtype value)
        {
            if (i < 0 || i >= storage.Columns) throw new OutOfMatrixException();
            Values.Insert(i, value);
        }

        public void InsertElement(stype i, Element element)
        {
            if (i < 0 || i >= storage.Columns) throw new OutOfMatrixException();
            ColumnIndices.Insert(i, element.ColumnIndex);
            Values.Insert(i, element.Value);
        }

        public void RemoveColumnIndexAt(stype i)
        {
            if (i < 0 || i >= storage.Columns) throw new OutOfMatrixException();
            ColumnIndices.RemoveAt(i);
        }
        
        public void RemoveValueAt(stype i)
        {
            if (i < 0 || i >= storage.Columns) throw new OutOfMatrixException();
            Values.RemoveAt(i);
        }

        public void RemoveElementAt(stype i)
        {
            if (i < 0 || i >= storage.Columns) throw new OutOfMatrixException();
            ColumnIndices.RemoveAt(i);
            Values.RemoveAt(i);
        }
        
        public Element this[stype i]
        {
            get => GetElementAt(i);
            set => SetElementAt(i, value);
        }
        
        /// <summary>
        /// Объект-значение, представляющий элемент матрицы 
        /// </summary>
        public class Element
        {
            public stype? RowIndex { get; }
            public stype ColumnIndex { get; }
            public vtype Value { get; }

            public Element(stype columnIndex, vtype value)
            {
                ColumnIndex = columnIndex;
                Value = value;
            }

            public Element(stype rowIndex, stype columnIndex, vtype value) : this(columnIndex, value)
            {
                RowIndex = rowIndex;
            }
        }
    }
}