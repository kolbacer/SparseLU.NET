using OxyPlot;

namespace SparseMatrixAnalysis.Plots;

public class SparsityPatternPlotView
{
    public SparsityPatternPlotView() { }

    public PlotModel SparsityPatternModel { get; set; }
    public PlotModel SparsityPatternModelL { get; set; }
    public PlotModel SparsityPatternModelU { get; set; }
}