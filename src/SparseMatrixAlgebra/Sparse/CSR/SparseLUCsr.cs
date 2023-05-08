namespace SparseMatrixAlgebra.Sparse.CSR;

public class SparseLUCsr : SparseLU<stype,vtype>
{
    public override SparseMatrixCsr L { get; }
    public override SparseMatrixCsr U { get; }
    /// <summary>
    /// Перестановки строк
    /// </summary>
    public stype[]? P { get; }
    /// <summary>
    /// Перестановки столбцов
    /// </summary>
    public stype[]? Q { get; }

    public SparseLUCsr(SparseMatrixCsr L, SparseMatrixCsr U, stype[]? P = null, stype[]? Q = null)
    {
        this.L = L;
        this.U = U;
        this.P = P;
        this.Q = Q;
    }

    /// <summary>
    /// Получить исходную матрицу, вычислив (P^-1)*L*U*(Q^-1)
    /// </summary>
    public SparseMatrixCsr GetOrigin()
    {
        SparseMatrixCsr origin = L.MultiplyByMatrix(U);

        if (P != null)
        {
            stype[] _P = new stype[P.Length];
            for (stype i = 0; i < P.Length; ++i)
                _P[P[i] - 1] = i + 1;
            origin = origin.PermuteRows(_P);
        }

        if (Q != null)
        {
            stype[] _Q = new stype[Q.Length];
            for (stype i = 0; i < Q.Length; ++i)
                _Q[Q[i] - 1] = i + 1;
            origin = origin.PermuteColumns(_Q);
        }

        return origin;
    }
    
    public override SparseVector<stype,vtype> SolveLy_b(SparseVector<stype,vtype> b) => throw new NotImplementedException();
    public override SparseVector<stype,vtype> SolveUx_y(SparseVector<stype,vtype> y) => throw new NotImplementedException();
}