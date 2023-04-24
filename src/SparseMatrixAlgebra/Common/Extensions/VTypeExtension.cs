namespace SparseMatrixAlgebra.Common.Extensions;

public static class VTypeExtension {
    /// <summary>
    /// Проверка, является ли число "нулем" (относительно заданного eps). 
    /// !!! В .NET Core 7.0 добавили метод с таким же наименованием.
    /// </summary>
    public static bool IsZero(this vtype value)
    {
        return Math.Abs(value) <= Settings.eps;
    }
}