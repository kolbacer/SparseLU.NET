namespace SparseMatrixAlgebra.Common.Extensions;

public static class VTypeExtension {
    public static bool IsZero(this vtype value)
    {
        return Math.Abs(value) <= Settings.eps;
    }
}