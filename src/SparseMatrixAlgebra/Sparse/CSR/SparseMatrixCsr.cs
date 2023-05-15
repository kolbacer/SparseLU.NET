using SparseMatrixAlgebra.Common.Exceptions;
using SparseMatrixAlgebra.Common.Extensions;
using SparseMatrixAlgebra.Common.Logging;
using Element = SparseMatrixAlgebra.Sparse.CSR.SparseVector.Element;

namespace SparseMatrixAlgebra.Sparse.CSR;

public partial class SparseMatrixCsr : SparseMatrix<stype,vtype>
{
    public SparseMatrixCsr(stype Rows, stype Columns)
    {
        Storage = new CsrStorage(Rows, Columns);
    }
    
    internal SparseMatrixCsr(CsrStorage storage)
    {
        Storage = storage;
    }

    /// <summary>
    /// Получить строку матрицы в виде вектора.
    /// НЕ создает копию хранилища.
    /// </summary>
    internal SparseVector GetRowAsVector(stype rowIndex) => 
        ((CsrStorage)Storage).GetRowAsVector(rowIndex);

    public override vtype GetElement(stype row, stype column)
    {
        if (row < 1 || row > Rows || column < 1 || column > Columns)
            throw new OutOfMatrixException();
        
        stype iRow = row - 1;
        stype iCol = column - 1;

        var compressedRow = GetRowAsVector(iRow);
        for (stype i = 0; i < compressedRow.NumberOfNonzeroElements; ++i)
        {
            stype columnIndex = compressedRow.GetIndexAt(i);
            if (columnIndex == iCol)
                return compressedRow.GetValueAt(i);
            else if (columnIndex > iCol)
                return 0;
        }

        return 0;
    }
    
    public override void SetElement(stype row, stype column, vtype value)
    {
        if (row < 1 || row > Rows || column < 1 || column > Columns)
            throw new OutOfMatrixException();
        
        stype iRow = row - 1;
        stype iCol = column - 1;
        
        var compressedRow = GetRowAsVector(iRow);
        for (stype i = 0; i < compressedRow.NumberOfNonzeroElements; ++i)
        {
            stype columnIndex = compressedRow.GetIndexAt(i);
            if (columnIndex == iCol)
            {
                if (value.IsZero())
                    compressedRow.RemoveElementAt(i);
                else
                    compressedRow.SetValueAt(i, value);
                return;
            } else if (columnIndex > iCol)
            {
                if (!value.IsZero())
                    compressedRow.InsertElement(i, new Element(iCol,value));
                return;
            }
        }
        if (!value.IsZero()) 
            compressedRow.AddElement(new Element(iCol,value));
    }

    /// <summary>
    /// Сравнивает поэлементно 2 матрицы с точностью до eps
    /// </summary>
    public bool Equals(SparseMatrixCsr other)
    {
        if (this.Rows != other.Rows || this.Columns != other.Columns) return false;

        for (stype i = 0; i < Rows; ++i)
        {
            var thisRow = this.GetRowAsVector(i);
            var otherRow = other.GetRowAsVector(i);

            if (!thisRow.Equals(otherRow)) return false;
        }

        return true;
    }

    /// <summary>
    /// Получить шаблон разреженности матрицы
    /// </summary>
    /// <param name="n">размерность шаблона</param>
    public double[,] GetSparsityPattern(uint n)
    {
        if (Rows != Columns) throw new IncompatibleDimensionsException("Matrix must be square");
        if (n > Rows) throw new ArgumentException("n must be <= Rows/Columns");
        if (n == 0) return new double[0,0];

        double[,] pattern = new double[n, n];
        int bucketSize = (int)Math.Ceiling((double)Rows / (double)n);
        int elementsInBucket = bucketSize * bucketSize;

        for (stype i = 0; i < Rows; ++i)
        {
            int iBucket = (int)Math.Ceiling((double)(i + 1) / ((double)Rows / (double)n)) - 1;
            var rowVector = GetRowAsVector(i);
            for (stype j = 0; j < rowVector.NumberOfNonzeroElements; ++j)
            {
                stype column = rowVector.GetIndexAt(j);
                int jBucket = (int)Math.Ceiling((double)(column + 1) / ((double)Columns / (double)n)) - 1;
                pattern[iBucket, jBucket]++;
            }
        }
        
        for (int i = 0; i < pattern.GetLength(0); ++i) 
        for (int j = 0; j < pattern.GetLength(1); ++j)
            pattern[i, j] /= elementsInBucket;
        
        return pattern;
    }
    
    public override void Print()
    {
        PrintToLogger(Loggers.ConsoleLogger);
    }

    public void PrintToFile()
    {
        PrintToLogger(Loggers.FileLogger);
    }

    private void PrintToLogger(Logger logger)
    {
        for (stype i = 0; i < Rows; ++i)
        {
            string rowString = "";
            var compressedRow = GetRowAsVector(i);
            stype column = 0;
            for (stype j = 0; j < compressedRow.NumberOfNonzeroElements; ++j)
            {
                stype columnIndex = compressedRow.GetIndexAt(j);
                for (; column < columnIndex; ++column)
                    rowString += $"{0,6:0} ";
                rowString += $"{compressedRow.GetValueAt(j),6:0.##} ";
                ++column;
            }

            for (; column < Columns; ++column)
                rowString += $"{0,6:0} ";
            
            logger.Print(rowString);
        }
    }

    public override SparseMatrixCsr Copy() => new SparseMatrixCsr((CsrStorage)Storage.Copy());
    
    public override SparseVector<stype,vtype> SolveSLAE(SparseVector<stype,vtype> b) => throw new NotImplementedException();
}