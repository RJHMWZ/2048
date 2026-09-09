/// <summary>
/// 单行计算时使用的Tile数据
/// </summary>
public struct LineTileData
{
    /// <summary>
    /// 数字
    /// </summary>
    public int Value;

    /// <summary>
    /// 原始行
    /// </summary>
    public int Row;

    /// <summary>
    /// 原始列
    /// </summary>
    public int Column;

    public LineTileData(
        int value,
        int row,
        int column)
    {
        Value = value;

        Row = row;

        Column = column;
    }
}