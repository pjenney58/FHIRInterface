using System.Numerics;

namespace DataShapes;

public class NumericVector
{
    internal Vector<decimal> Left;
    internal Vector<decimal> Right;

    public NumericVector(int columnCount)
    {
        ColumnCount = columnCount;
    }

    public int ColumnCount { get; set; } = 2;

    public async Task Fill()
    {
        ;
    }
    
}

