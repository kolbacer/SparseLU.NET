using System.Globalization;
using SparseMatrixAlgebra.Common.Exceptions;
using SparseMatrixAlgebra.Common.Extensions;
using SparseMatrixAlgebra.Sparse.CSR;

namespace SparseMatrixAlgebra.Utils;

public static class MatrixBuilder
{
    /// <summary>
    /// Создает разреженную матрицу заданного размера в формате CSR. (Все элементы нулевые). 
    /// </summary>
    /// <param name="rows">кол-во строк</param>
    /// <param name="cols">кол-во столбцов</param>
    public static SparseMatrixCsr CreateCsr(stype rows, stype cols)
    {
        return new SparseMatrixCsr(rows, cols);
    }

    /// <summary>
    /// Создает разреженную матрицу в формате CSR на основе переданного массива.
    /// </summary>
    /// <param name="array">Двумерный массив</param>
    public static SparseMatrixCsr CsrOfArray(vtype[,] array)
    {
        stype rows = array.GetUpperBound(0) + 1;
        stype columns = array.GetUpperBound(1) + 1;

        if (rows == 0 || columns == 0) throw new ArgumentException("array must be not empty");

        CsrStorage storage = new CsrStorage(rows, columns);
        for (stype i = 0; i < rows; ++i)
        {
            for (stype j = 0; j < columns; ++j)
            {
                if (!array[i, j].IsZero())
                {
                    storage.ColumnIndexRows[i].Add(j);
                    storage.ValueRows[i].Add(array[i, j]);
                }
            }
        }

        return new SparseMatrixCsr(storage);
    }
    
    private enum Symmetry
    {
        General,
        Symmetric,
        SkewSymmetric
    }

    /// <summary>
    /// Читает CSR матрицу из файла в формате MatrixMarket coordinate.
    /// Читается заголовок (если есть), пропускаются комментарии, далее строка, содержащая измерения матрицы,
    /// затем тройки чисел (строка, столбец, значение).
    /// </summary>
    /// <param name="filepath">Полное имя файла</param>
    public static SparseMatrixCsr ReadCsrFromFile(string filepath)
    {
        using StreamReader reader = new StreamReader(filepath);
        string? line;

        Symmetry symmetry = Symmetry.General;
        string[] tokens;
            
        // read header, skip comments
        while ((line = reader.ReadLine()) != null)
        {
            line = line.Trim();
            if (line.Length == 0) continue;
            if (!line.StartsWith('%')) break;
            if (line.StartsWith("%%"))
            {
                tokens = line.Split(' ');
                
                if (tokens[2] != "coordinate") 
                    throw new InvalidFileFormatException("Format must be coordinate");
                
                if (tokens.Length > 3)
                    if (tokens[3] != "real" && tokens[3] != "integer") 
                        throw new InvalidFileFormatException("Field must be real or integer");
                
                if (tokens.Length > 4)
                {
                    switch (tokens[4])
                    {
                        case "general":
                            symmetry = Symmetry.General;
                            break;
                        case "symmetric":
                            symmetry = Symmetry.Symmetric;
                            break;
                        case "skew-symmetric":
                            symmetry = Symmetry.SkewSymmetric;
                            break;
                        default:
                            throw new InvalidFileFormatException("Symmetry must be general, symmetric or skew-symmetric");
                    }
                }
            }
        }

        if (line == null) throw new InvalidFileFormatException("Can't read matrix dimensions");
            
        // read size
        tokens = line.Split(' ');
        stype rows = stype.Parse(tokens[0]);
        stype columns = stype.Parse(tokens[1]);
        SparseMatrixCsr matrix = new SparseMatrixCsr(rows, columns);
            
        // read entries
        while ((line = reader.ReadLine()) != null)
        {
            line = line.Trim();
            if (line.Length == 0) continue;

            tokens = line.Split(' ');
            stype row = stype.Parse(tokens[0]);
            stype col = stype.Parse(tokens[1]);
            vtype value = vtype.Parse(tokens[2], CultureInfo.InvariantCulture);

            matrix.SetElement(row, col, value);
            if (symmetry == Symmetry.Symmetric && row != col)
                matrix.SetElement(col, row, value);
            else if (symmetry == Symmetry.SkewSymmetric)
                matrix.SetElement(col, row, -value);
        }

        return matrix;
    }

    public static SparseVector CreateCsrVector(stype length)
    {
        return new SparseVector(length);
    }
}