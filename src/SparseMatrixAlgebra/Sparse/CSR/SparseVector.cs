using SparseMatrixAlgebra.Common.Exceptions;
using SparseMatrixAlgebra.Common.Extensions;

namespace SparseMatrixAlgebra.Sparse.CSR;

public partial class SparseVector : SparseVector<stype, vtype>
{
    public List<stype> Indices { get; } = new List<stype>();
    public List<vtype> Values { get; } = new List<vtype>();

    public override stype Length { get; } = 0;
    public override bool IsColumn { get; set; } = false;
    public override stype NumberOfNonzeroElements { get => Indices.Count; }

    public SparseVector(stype length)
    {
        if (length < 0) throw new OutOfVectorException("Invalid vector length");
        Length = length;
    }
    public SparseVector(stype length, bool isColumn) : this(length) { IsColumn = isColumn;}

    public override vtype GetElement(stype index)
    {
        if (index < 1 || index > Length) throw new OutOfVectorException();

        stype iIndex = index - 1;
        for (stype i = 0; i < Indices.Count; ++i)
        {
            if (Indices[i] == iIndex)
                return Values[i];
            else if (Indices[i] > iIndex)
                return 0;
        }

        return 0;
    }

    public override void SetElement(stype index, vtype value)
    {
        if (index < 1 || index > Length) throw new OutOfVectorException();
        
        stype iIndex = index - 1;
        for (stype i = 0; i < Indices.Count; ++i)
        {
            if (Indices[i] == iIndex)
            {
                if (value.IsZero())
                    RemoveElementAt(i);
                else
                    SetValueAt(i, value);
                return;
            } else if (Indices[i] > iIndex)
            {
                if (!value.IsZero()) 
                    InsertElement(i, new Element(iIndex,value));
                return;
            }
        }
        if (!value.IsZero()) 
            AddElement(new Element(iIndex,value));
    }

    public override void Print() => Print(IsColumn);

    public override void Print(bool asColumn)
    {
        if (asColumn) PrintAsColumn();
        else PrintAsRow();
    }

    public void PrintAsRow()
    {
        var logger = Common.Logging.Loggers.ConsoleLogger;
        
        string vectorString = "";
        stype i = 0;
        for (stype j = 0; j < NumberOfNonzeroElements; ++j)
        {
            stype index = GetIndexAt(j);
            for (; i < index; ++i)
                vectorString += "0 ";
            vectorString += $"{GetValueAt(j)} ";
            ++i;
        }

        for (; i < Length; ++i)
            vectorString += "0 ";
            
        logger.Print(vectorString);
    }

    public void PrintAsColumn()
    {
        var logger = Common.Logging.Loggers.ConsoleLogger;
        
        stype i = 0;
        for (stype j = 0; j < NumberOfNonzeroElements; ++j)
        {
            stype index = GetIndexAt(j);
            for (; i < index; ++i)
                logger.Print("0");
            logger.Print($"{GetValueAt(j)} ");
            ++i;
        }

        for (; i < Length; ++i)
            logger.Print("0");
    }

    public override void PrintStorage()
    {
        var logger = Common.Logging.Loggers.ConsoleLogger;

        string indicesString = "Indices: | ";
        string valuesString  = "Values:  | ";
        
        for (stype j = 0; j < NumberOfNonzeroElements; ++j)
        {
            indicesString += $"{GetIndexAt(j),6:0} ";
            valuesString += $"{GetValueAt(j),6:0.##} ";
        }

        indicesString += "|";
        valuesString += "|";
        
        logger.Print(indicesString);
        logger.Print(valuesString);
    }
}