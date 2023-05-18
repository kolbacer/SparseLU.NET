using System;
using BenchmarkDotNet.Running;
using SparseMatrixAlgebra.Benchmarks.Factorization.RandomMatrices;
using System.Globalization;
using System.Windows;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Legends;
using OxyPlot.Series;
using SparseMatrixAnalysis.Plots;

namespace SparseMatrixAnalysis.Tests;

public static class FactorizationBenchmarksTest
{
    public static void Run(bool parallel = false)
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            ((WindowSettings)Application.Current.MainWindow.DataContext).LoaderTextTab2 = "Бенчмарки работают";
            ((WindowSettings)Application.Current.MainWindow.DataContext).ShowLoaderTab2 = Visibility.Visible;
        });
        
        var summary = parallel
            ? BenchmarkRunner.Run<RandomMatricesFactorizationParallelBenchmark>()
            : BenchmarkRunner.Run<RandomMatricesFactorizationBenchmark>();

        int methodIndex = Array.IndexOf(summary.Table.FullHeader, "Method");
        int matrixIndex = Array.IndexOf(summary.Table.FullHeader, "TestMatrix");
        int timeIndex = Array.IndexOf(summary.Table.FullHeader, "Mean");

        var methodColumn = summary.Table.Columns[methodIndex];
        var matrixColumn = summary.Table.Columns[matrixIndex];
        var timeColumn = summary.Table.Columns[timeIndex];

        summary = null;
        GC.Collect();
        
        Application.Current.Dispatcher.Invoke(() =>
        {
            ((WindowSettings)Application.Current.MainWindow.DataContext).LoaderTextTab2 = "Отрисовка графика";
        });

        int contentLength = methodColumn.Content.Length;

        string methodName1 = parallel ? "LuFactorizeParallel()" : "LuFactorize()";
        string methodName2 = parallel ? "LuFactorizeMarkowitzParallel()" : "LuFactorizeMarkowitz()";
        string methodName3 = parallel ? "LuFactorizeMarkowitz2Parallel()" : "LuFactorizeMarkowitz2()";
        
        string benchmarkName1 = parallel ? "CsrLuFactorizationParallel" : "CsrLuFactorization";
        string benchmarkName2 = parallel ? "CsrLuFactorizationMarkowitzParallel" : "CsrLuFactorizationMarkowitz";
        string benchmarkName3 = parallel ? "CsrLuFactorizationMarkowitz2Parallel" : "CsrLuFactorizationMarkowitz2";

        // create lines and fill them with data points
        var line1 = new LineSeries()
        {
            Title = methodName1,
            Color = OxyPlot.OxyColors.Blue,
            StrokeThickness = 1
        };

        var line2 = new LineSeries()
        {
            Title = methodName2,
            Color = OxyPlot.OxyColors.Red,
            StrokeThickness = 1
        };
        
        var line3 = new LineSeries()
        {
            Title = methodName3,
            Color = OxyPlot.OxyColors.Green,
            StrokeThickness = 1
        };

        Comparison<DataPoint> comparison = (p1, p2) =>
        {
            if (p1.X < p2.X) return -1;
            if (p1.X > p2.X) return 1;
            return 0;
        };

        for (int i = 0; i < contentLength; i++)
        {
            int fillPoint = GetFillFromMatrix(matrixColumn.Content[i]);
            double timePoint = GetTimeFromString(timeColumn.Content[i]);

            if (methodColumn.Content[i] == benchmarkName1)
                line1.Points.Add(new DataPoint(fillPoint, timePoint));
            else if (methodColumn.Content[i] == benchmarkName2)
                line2.Points.Add(new DataPoint(fillPoint, timePoint));
            else if (methodColumn.Content[i] == benchmarkName3) 
                line3.Points.Add(new DataPoint(fillPoint, timePoint));
        }

        line1.Points.Sort(comparison);
        line2.Points.Sort(comparison);
        line3.Points.Sort(comparison);

        // create the model and add the lines to it
        var model = new PlotModel
        {
            Title = "Время работы " + (parallel ? "параллельных" : "последовательных") + " алгоримов на случайных матрицах размера 1000x1000"
        };
        model.Series.Add(line1);
        model.Series.Add(line2);
        model.Series.Add(line3);
        
        model.Legends.Add(new Legend
        {
            LegendPlacement = LegendPlacement.Outside,
            LegendPosition = LegendPosition.TopRight,
        });

        model.Axes.Add(new LinearAxis()
        {
            Position = AxisPosition.Bottom,
            Title = "Среднее кол-во ненулевых элементов в строке"
        });
        model.Axes.Add(new LinearAxis()
        {
            Position = AxisPosition.Left,
            Title = "Время работы, с"
        });

        GC.Collect();
        
        Application.Current.Dispatcher.Invoke(() =>
        {
            RandomMatricesBenchmarkPlot plot = new RandomMatricesBenchmarkPlot();
            RandomMatricesBenchmarkPlotView plotView = new RandomMatricesBenchmarkPlotView();
            plot.DataContext = plotView;
            plotView.RandomMatricesBenchmarkPlotModel = model;

            plot.Show();
            
            ((WindowSettings)Application.Current.MainWindow.DataContext).ShowLoaderTab2 = Visibility.Hidden;
        });
    }

    private static int GetFillFromMatrix(string matrixName)
    {
        string fill = matrixName.Split('_')[1];
        fill = fill.Substring(2, fill.Length - 3);
        return Int32.Parse(fill);
    }

    private static double GetTimeFromString(string timeString)
    {
        return Double.Parse(timeString.Split(' ')[0], CultureInfo.InvariantCulture);
    }
}