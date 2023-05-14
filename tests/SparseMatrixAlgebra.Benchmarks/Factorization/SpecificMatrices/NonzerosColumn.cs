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
            return factorizationTestRun.Matrix.NumberOfNonzeroElements.ToString();
        else
        {
            string resultDirectory = SpecificMatricesFactorizationBenchmark.ResultDirectory;
            string matrixName = factorizationTestRun.Title;
            string? filename = benchmarkCase.Descriptor.MethodIndex switch
            {
                0 => $"{resultDirectory}\\nonzeros.csrlufactorization.{matrixName}.txt",
                1 => $"{resultDirectory}\\nonzeros.csrlufactorizationmarkowitz.{matrixName}.txt",
                2 => $"{resultDirectory}\\nonzeros.csrlufactorizationmarkowitz2.{matrixName}.txt",
                _ => null
            };

            if (filename == null) return "unknown method";
            return File.Exists(filename) ? File.ReadAllText(filename) : "no file";
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