﻿using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using SparseMatrixAlgebra.Sparse.CSR;
using SparseMatrixAlgebra.Utils;
using SparseMatrixAnalysis.Plots;

namespace SparseMatrixAnalysis.Tests;

public static class FactorizationTest
{
    public static uint resolution = 100;
    public static bool interpolation = false;
    
    public static void Run(string filepath)
    {
        var matrix = MatrixBuilder.ReadCsrFromFile(filepath);
        string matrixMame = Path.GetFileNameWithoutExtension(filepath);

        var plot = new SparsityPatternPlot();
        var plotView = new SparsityPatternPlotView();
        plot.DataContext = plotView;
        plot.Resources.Add("SizeCaption", $"Размер матрицы: {matrix.Rows}x{matrix.Columns}");
        plot.Resources.Add("NonzerosCaption", $"Количество ненулевых элементов: {matrix.NumberOfNonzeroElements}");
        var model = GetSparsityPatternPlotModelOfMatrix(matrix);
        plotView.SparsityPatternModel = model;
        model.Title = $"Шаблон разреженности матрицы {matrixMame}";
        plot.Show();

        // LU-factorization

        string windowTitle = $"LU-разложение матрицы {matrixMame} (выбор ведущего элемента по столбцу)";

        var timer = new Stopwatch();
        
        timer.Start();
        var LU = matrix.LuFactorizeParallel();
        timer.Stop();
        TimeSpan elapsed = timer.Elapsed;
        string timeString = 
            (elapsed.Hours != 0) ? $"{elapsed.Hours}ч {elapsed.Minutes}м {elapsed.Seconds}с {elapsed.Milliseconds}мс" :
            (elapsed.Minutes != 0) ? $"{elapsed.Minutes}м {elapsed.Seconds}с {elapsed.Milliseconds}мс" :
            (elapsed.Seconds != 0) ? $"{elapsed.Seconds}с {elapsed.Milliseconds}мс" :
            $"{elapsed.Milliseconds}мс";
        string factorizationTimeCaption = "Время работы метода LuFactorizeParallel(): " + timeString;
        var plotLU = new SparsityPatternFactorizationPlot();
        var plotViewLU = new SparsityPatternPlotView();
        plotLU.DataContext = plotViewLU;
        plotLU.Resources.Add("FactorizationTitle", windowTitle);
        plotLU.Resources.Add("FactorizationTimeCaption", factorizationTimeCaption);
        plotLU.Resources.Add("NonzerosLCaption", $"Кол-во ненулевых элементов: {LU.L.NumberOfNonzeroElements}");
        plotLU.Resources.Add("NonzerosUCaption", $"Кол-во ненулевых элементов: {LU.U.NumberOfNonzeroElements}");
        plotLU.Resources.Add("NonzerosTotalCaption", $"Общее кол-во ненулевых элементов: {LU.L.NumberOfNonzeroElements + LU.U.NumberOfNonzeroElements}");
        var modelL = GetSparsityPatternPlotModelOfMatrix(LU.L);
        plotViewLU.SparsityPatternModelL = modelL;
        var modelU = GetSparsityPatternPlotModelOfMatrix(LU.U);
        plotViewLU.SparsityPatternModelU = modelU;
        modelL.Title = "Матрица L";
        modelU.Title = "Матрица U";
        plotLU.Show();
        
        // LU-factorization Markowitz
        
        string windowTitleMarkowitz = $"LU-разложение матрицы {matrixMame} (выбор ведущего элемента по стратегии Марковица)";
        
        timer.Reset();
        
        timer.Start();
        LU = matrix.LuFactorizeMarkowitz2Parallel(0.001);
        timer.Stop();
        elapsed = timer.Elapsed;
        timeString = 
            (elapsed.Hours != 0) ? $"{elapsed.Hours}ч {elapsed.Minutes}м {elapsed.Seconds}с {elapsed.Milliseconds}мс" :
            (elapsed.Minutes != 0) ? $"{elapsed.Minutes}м {elapsed.Seconds}с {elapsed.Milliseconds}мс" :
            (elapsed.Seconds != 0) ? $"{elapsed.Seconds}с {elapsed.Milliseconds}мс" :
            $"{elapsed.Milliseconds}мс";
        string factorizationMarkowitzTimeCaption = "Время работы метода LuFactorizeMarkowitz2Parallel(): " + timeString;
        var plotLUMarkowitz = new SparsityPatternFactorizationPlot();
        var plotViewLUMarkowitz = new SparsityPatternPlotView();
        plotLUMarkowitz.DataContext = plotViewLUMarkowitz;
        plotLUMarkowitz.Resources.Add("FactorizationTitle", windowTitleMarkowitz);
        plotLUMarkowitz.Resources.Add("FactorizationTimeCaption", factorizationMarkowitzTimeCaption);
        plotLUMarkowitz.Resources.Add("NonzerosLCaption", $"Кол-во ненулевых элементов: {LU.L.NumberOfNonzeroElements}");
        plotLUMarkowitz.Resources.Add("NonzerosUCaption", $"Кол-во ненулевых элементов: {LU.U.NumberOfNonzeroElements}");
        plotLUMarkowitz.Resources.Add("NonzerosTotalCaption", $"Общее кол-во ненулевых элементов: {LU.L.NumberOfNonzeroElements + LU.U.NumberOfNonzeroElements}");
        var modelLMarkowitz = GetSparsityPatternPlotModelOfMatrix(LU.L);
        plotViewLUMarkowitz.SparsityPatternModelL = modelLMarkowitz;
        var modelUMarkowitz = GetSparsityPatternPlotModelOfMatrix(LU.U);
        plotViewLUMarkowitz.SparsityPatternModelU = modelUMarkowitz;
        modelLMarkowitz.Title = "Матрица L";
        modelUMarkowitz.Title = "Матрица U";
        plotLUMarkowitz.Show();
        
    }

    private static PlotModel GetSparsityPatternPlotModelOfMatrix(SparseMatrixCsr matrix)
    {
        var pattern = matrix.GetSparsityPattern(resolution);
        double[,] transposedPattern = new double[resolution, resolution];
        for (int i = 0; i < pattern.GetLength(0); ++i)
        for (int j = 0; j < pattern.GetLength(1); ++j)
            transposedPattern[i, j] = pattern[j, i];
        
        var model = new PlotModel();

        var palette =
            OxyPalette.Interpolate(256, 
                new[] { OxyColor.FromRgb(255, 255, 255), OxyColor.FromRgb(0, 0, 100) });
        
        model.Axes.Add(new LinearColorAxis
        {
            Palette = palette
            // Palette = OxyPalettes.Rainbow(100)
        });

        var heatMapSeries = new HeatMapSeries
        {
            X0 = 0,
            X1 = resolution - 1,
            Y0 = resolution - 1,
            Y1 = 0,
            Interpolate = interpolation,
            RenderMethod = HeatMapRenderMethod.Bitmap,
            Data = transposedPattern
        };

        model.Series.Add(heatMapSeries);

        return model;
    }
}