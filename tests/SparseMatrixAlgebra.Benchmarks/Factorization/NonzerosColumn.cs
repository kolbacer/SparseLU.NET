using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using SparseMatrixAlgebra.Benchmarks.Factorization.RandomMatrices;
using SparseMatrixAlgebra.Benchmarks.Factorization.SpecificMatrices;

namespace SparseMatrixAlgebra.Benchmarks.Factorization;

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
        ITestRun testRun = null;
        if (benchmarkCase.Descriptor.Type == typeof(SpecificMatricesFactorizationBenchmark))
            testRun = (FactorizationTestRun)benchmarkCase.Parameters.GetArgument("TestMatrix").Value;
        else if (benchmarkCase.Descriptor.Type == typeof(RandomMatricesFactorizationBenchmark))
            testRun = (RandomTestRun)benchmarkCase.Parameters.GetArgument("TestMatrix").Value;
        else if (benchmarkCase.Descriptor.Type == typeof(RandomMatricesFactorizationParallelBenchmark))
            testRun = (RandomTestRun)benchmarkCase.Parameters.GetArgument("TestMatrix").Value;


        if (init)
        {
            if (testRun.Case == "specific")
                return ((FactorizationTestRun)testRun).Matrix.NumberOfNonzeroElements.ToString();
            else if (testRun.Case == "random")
            {
                int totalNonzeros = 0;
                var MatrixArray = ((RandomTestRun)testRun).MatrixArray;
                for (int i = 0; i < MatrixArray.Length; ++i)
                    totalNonzeros += MatrixArray[i].NumberOfNonzeroElements;
                return (totalNonzeros / MatrixArray.Length).ToString();
            }
            else return "?";
        }
        else
        {
            string resultDirectory = null;
            if (benchmarkCase.Descriptor.Type == typeof(SpecificMatricesFactorizationBenchmark))
                resultDirectory = SpecificMatricesFactorizationBenchmark.ResultDirectory;
            else if (benchmarkCase.Descriptor.Type == typeof(RandomMatricesFactorizationBenchmark))
                resultDirectory = RandomMatricesFactorizationBenchmark.ResultDirectory;
            else if (benchmarkCase.Descriptor.Type == typeof(RandomMatricesFactorizationParallelBenchmark))
                resultDirectory = RandomMatricesFactorizationParallelBenchmark.ResultDirectory;
            string methodName = benchmarkCase.Descriptor.WorkloadMethod.Name.ToLower();
            string matrixName = testRun.Title;
            string filename = $"{resultDirectory}\\nonzeros.{methodName}.{matrixName}.txt";

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