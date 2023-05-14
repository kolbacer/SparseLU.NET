using SparseMatrixAlgebra.Sparse.CSR;
using SparseMatrixAlgebra.Utils;

namespace SparseMatrixAlgebra.UnitTests.CSR;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [TestCaseSource(nameof(TestMatrices))]
    public void CsrLuFactorizedMatrixShouldBeEqualOrigin(SparseMatrixCsr matrix)
    {
        var factorized = matrix.LuFactorize();
        
        Assert.True(factorized.GetOrigin().Equals(matrix));
    }
    
    [TestCaseSource(nameof(TestMatrices))]
    public void CsrLuFactorizedMarkowitzMatrixShouldBeEqualOrigin(SparseMatrixCsr matrix)
    {
        var factorized = matrix.LuFactorizeMarkowitz(0.001);
        
        Assert.True(factorized.GetOrigin().Equals(matrix));
    }
    
    [TestCaseSource(nameof(TestMatrices))]
    public void CsrLuFactorizedMarkowitz2MatrixShouldBeEqualOrigin(SparseMatrixCsr matrix)
    {
        var factorized = matrix.LuFactorizeMarkowitz2(0.001);
        
        Assert.True(factorized.GetOrigin().Equals(matrix));
    }
    
    public static object[] TestMatrices =
    {
        MatrixBuilder.CsrOfArray(new double[,] 
        { 
            { 3, 4, 0 }, 
            { -2, 5, 0 }, 
            { 5, -1, 7 } 
        }),
        MatrixBuilder.CsrOfArray(new double[,] 
        {
            { -7.730, 4.489, -0.714, 3.796, -4.444, 0.892, 6.831, -1.373, -6.645, -6.808 },
            {-0.035, 1.967, 6.761, -3.161, 4.310, -0.007, 2.896, -2.069, -0.114, -3.535},
            { 2.106, -5.008, 5.371, -4.060, 7.922, 8.691, -6.072, -5.313, 8.463, 6.330 },
            {-7.921, -3.011, -4.517, 3.576, -3.258, -4.107, 8.125, 3.459, -1.730, 5.595},
            {-7.753, 0.300, -8.554, -4.990, 4.047, -6.877, -0.454, -7.212, -2.202, -6.723},
            {-5.243, -3.007, 7.826, -1.160, -0.695, -1.382, 5.138, -4.098, -8.197, 3.484},
            { -4.328, -6.598, -5.700, 6.407, -0.023, -2.780, -4.026, 3.549, -3.793, 5.862 },
            {-8.840, 5.751, 2.502, 8.156, -6.686, 4.626, 4.182, 1.417, -3.985, -5.964},
            { -5.157, 3.594, -2.677, -6.225, -7.812, 4.089, 8.239, -1.803, -0.680, -8.669 },
            {7.867, -1.730, 3.828, -3.133, 0.630, -1.938, -4.237, 3.399, 0.711, -7.430}
        }),
        MatrixBuilder.CsrOfArray(new double[,] 
        {
            {0, 0, 1.69, 0.865, 0, 1.37, 0, 0, 0, 0, 0, 0, 0, 0, -1.25},
            {-0.973, 0, 0.555, 0, 0.905, 0, 0.467, 0, 0, 0, 0, 0, 0, 0, 0},
            {0, 1.19, 0, 0, 0.337, 0, -1.61, 0, 0, 0, -0.417, 0, -1.34, -1.04, -1.73},
            {0, 0, 0, 0, -1.11, 0, 0.883, 1.09, 0, 0, 0, 0, 0, -0.284, 0},
            {0.693, 0.506, 0.0892, 0, 0.748, 0, 0, -1.29, -0.701, 0, 0, 0, 0, 0, -0.408},
            {0, 0, 0, 0.431, 0, 0, 0, 0.305, 0, 0.393, -1.38, 0, 1.78, 0, 0},
            {-1.52, -0.975, 0, 0, 0, -0.371, 0, 0, 1.86, 0, 0.736, 0, 1.05, 0, 0},
            {1.97, 0, 0, 0, 0, 0, 0.805, 1.46, 1.03, 0, 1.91, -0.82, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, -0.157, 0.23, 0, 0, 0.756, 0, 1.95, 0},
            {0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0.34, 0, -0.337, 0, 0},
            {0.509, -0.553, 0, 0, 0, -1.19, 1.74, -0.598, 0, 0, 0, 0, 0, 0, 0},
            {0, 0, 0, 0, 0, 0, 0, -1.61, -1.62, 0, 0, 1.14, 0, 0, 0},
            {0, 0, 0.947, -1.69, 1.76, 0, 0, 0, 0, 0, 0, 0, 0, 1.71, 0},
            {0, 0, 1.49, 0, 0, 0, 0, -1.36, 0, 0, 0, 0, 0, -1.08, 0},
            {-0.876, 0, 0, 0, 1.3, 0, 1.6, 0, -0.517, 0, 0, 0, 0, 0, 0}
        }),
    };
}