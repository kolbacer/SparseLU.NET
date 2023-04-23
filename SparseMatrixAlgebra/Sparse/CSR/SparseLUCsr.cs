namespace SparseMatrixAlgebra.Sparse.CSR;

public class SparseLUCsr : SparseLU<stype,vtype>
{
    public override SparseMatrixCsr L { get; }
    public override SparseMatrixCsr U { get; }
    
    public override SparseVector<stype,vtype> SolveLy_b(SparseVector<stype,vtype> b) => throw new NotImplementedException();
    public override SparseVector<stype,vtype> SolveUx_y(SparseVector<stype,vtype> y) => throw new NotImplementedException();
}