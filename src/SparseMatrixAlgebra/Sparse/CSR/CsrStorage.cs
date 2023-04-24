namespace SparseMatrixAlgebra.Sparse.CSR;

internal partial class CsrStorage : SparseStorage<stype>
{
    public List<List<stype>> ColumnIndexRows { get; private set; }
    public List<List<vtype>> ValueRows { get; private set; }

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
    
    private CsrStorage() {}
    
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

    public override CsrStorage Copy()
    {
        CsrStorage newStorage = new CsrStorage();
        newStorage.Rows = Rows;
        newStorage.Columns = Columns;
        newStorage.ColumnIndexRows = new List<List<stype>>(Rows);
        newStorage.ValueRows = new List<List<vtype>>(Rows);
        for (stype i = 0; i < Rows; ++i)
        {
            newStorage.ColumnIndexRows.Add(new List<stype>(ColumnIndexRows[i]));
            newStorage.ValueRows.Add(new List<vtype>(ValueRows[i]));
        }

        return newStorage;
    }
}