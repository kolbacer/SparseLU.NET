namespace SparseMatrixAlgebra.Sparse.CSR;

public partial class SparseMatrixCsr
{
    public override void AddRows(stype augend, stype addend, vtype coef) => throw new NotImplementedException();
    public override void SwapRows(stype row1, stype row2) => throw new NotImplementedException();
    public override SparseVector<stype,vtype> MultiplyByVector(SparseVector<stype,vtype> vector) => throw new NotImplementedException();
}