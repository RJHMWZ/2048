/// <summary>
/// 单行计算使用的原始方块数据。
/// </summary>
public readonly struct LineTileData
{
    public int Value { get; }
    public int Row { get; }
    public int Column { get; }

    public LineTileData(int value, int row, int column)
    {
        Value = value;
        Row = row;
        Column = column;
    }
}
