using OxyPlot;

namespace SparseMatrixAnalysis.Plots;

public class RandomMatricesBenchmarkPlotView
{
    public RandomMatricesBenchmarkPlotView()
    {
        this.RandomMatricesBenchmarkPlotModel = new PlotModel { Title = "Бенчмарки на случайных матрицах" };
    }
    
    public PlotModel RandomMatricesBenchmarkPlotModel { get; set; }
}