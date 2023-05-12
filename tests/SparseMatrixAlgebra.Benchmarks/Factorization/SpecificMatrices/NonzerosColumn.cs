using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;

namespace SparseMatrixAlgebra.Benchmarks.Factorization.SpecificMatrices;

public class NonzerosColumn : IColumn
{
    private readonly bool init;
    public string Id { get; }
    public string ColumnName { get; }
    
    public NonzerosColumn(string columnName, bool init)
    {
        this.init = init;
        ColumnName = columnName;
        Id = nameof(NonzerosColumn) + "." + ColumnName;
    }
    
    public bool IsDefault(Summary summary, BenchmarkCase benchmarkCase) => false;

    public string GetValue(Summary summary, BenchmarkCase benchmarkCase)
    {
        var factorizationTestRun = (FactorizationTestRun)
            benchmarkCase.Parameters.GetArgument("TestMatrix").Value;

        if (init)
            return factorizationTestRun.InitNonzeros.ToString();
        else
        {
            return benchmarkCase.Descriptor.MethodIndex switch
            {
                0 => factorizationTestRun.ResultNonzeros.ToString(),
                1 => factorizationTestRun.ResultNonzerosMarkowitz.ToString(),
                _ => "unknown method"
            };
        }
    }
    
    public bool IsAvailable(Summary summary) => true;
    public bool AlwaysShow => true;
    public ColumnCategory Category => ColumnCategory.Custom;
    public int PriorityInCategory => 0;
    public bool IsNumeric => true;
    public UnitType UnitType => UnitType.Dimensionless;
    public string Legend => (init ? "Initial " : "Resulting (L+U) total ") + "number of non-zero elements";
    public string GetValue(Summary summary, BenchmarkCase benchmarkCase, SummaryStyle style) => GetValue(summary, benchmarkCase);
    public override string ToString() => ColumnName;
}