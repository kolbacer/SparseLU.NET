namespace SparseMatrixAlgebra.Sparse.CSR;

public class SparseLUCsr : SparseLU<stype,vtype>
{
    public override SparseMatrixCsr L { get; }
    public override SparseMatrixCsr U { get; }
    public stype[] P { get; }

    public SparseLUCsr(SparseMatrixCsr L, SparseMatrixCsr U, stype[] P)
    {
        this.L = L;
        this.U = U;
        this.P = P;
    }
    
    public override SparseVector<stype,vtype> SolveLy_b(SparseVector<stype,vtype> b) => throw new NotImplementedException();
    public override SparseVector<stype,vtype> SolveUx_y(SparseVector<stype,vtype> y) => throw new NotImplementedException();
}