global using stype = System.Int32;  // тип индекса
global using vtype = System.Double; // тип значения

namespace SparseMatrixAlgebra.Common
{
    public static class Settings
    {
        /// <summary>
        /// Что считать нулем.
        /// </summary>
        public const vtype eps = 1e-10;
    }
}