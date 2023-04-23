namespace SparseMatrixAlgebra.Sparse.CSR;

internal partial class CsrStorage : SparseStorage<stype>
{
    private List<List<stype>> ColumnIndexRows;
    private List<List<vtype>> ValueRows;

    public CsrStorage(stype Rows, stype Columns)
    {
        this.Rows = Rows;
        this.Columns = Columns;

        ColumnIndexRows = new List<List<stype>>(Rows);
        ValueRows = new List<List<vtype>>(Rows);

        for (stype i = 0; i < Rows; ++i)
        {
            ColumnIndexRows.Add(new List<stype>());
            ValueRows.Add(new List<vtype>());
        }
    }
    
    public override stype NumberOfNonzeroElements
    {
        get
        {
            stype totalCount = 0;
            foreach (var row in ColumnIndexRows)
            {
                totalCount += row.Count;
            }

            return totalCount;
        }
        protected set {}
    }
    
    /// <summary>
    /// Выводит в лог содержимое хранилища: столбцовые индексы и значения элементов.
    /// Строки матрицы разделены символом "|".
    /// </summary>
    public override void PrintStorage()
    {
        var logger = Common.Logging.Loggers.ConsoleLogger;

        string indicesString = "Column indices: | ";
        string valuesString  = "Values:         | ";
        
        for (stype i = 0; i < Rows; ++i)
        {
            var compressedRow = GetCompressedRow(i);
            for (stype j = 0; j < compressedRow.Count; ++j)
            {
                var element = compressedRow[j];
                indicesString += $"{element.ColumnIndex,6:0} ";
                valuesString += $"{element.Value,6:0.##} ";
            }

            indicesString += "| ";
            valuesString += "| ";
        }
        
        logger.Print(indicesString);
        logger.Print(valuesString);
    }
    
    public CompressedRow GetCompressedRow(stype rowIndex)
    {
        return new CompressedRow(this, rowIndex);
    }
}