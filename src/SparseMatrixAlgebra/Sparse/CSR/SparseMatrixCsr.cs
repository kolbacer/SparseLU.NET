using SparseMatrixAlgebra.Common.Exceptions;
using SparseMatrixAlgebra.Common.Extensions;
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
    
    public override void Print()
    {
        var logger = Common.Logging.Loggers.ConsoleLogger;

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