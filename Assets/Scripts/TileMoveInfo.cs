/// <summary>
/// 单个2048方块的移动信息
/// </summary>
public struct TileMoveInfo
{
    /// <summary>
    /// 起始行
    /// </summary>
    public int FromRow;

    /// <summary>
    /// 起始列
    /// </summary>
    public int FromColumn;

    /// <summary>
    /// 目标行
    /// </summary>
    public int ToRow;

    /// <summary>
    /// 目标列
    /// </summary>
    public int ToColumn;

    /// <summary>
    /// 移动前的数字
    /// </summary>
    public int Value;

    /// <summary>
    /// 是否参与了合并
    /// </summary>
    public bool Merged;

    public TileMoveInfo(
        int fromRow,
        int fromColumn,
        int toRow,
        int toColumn,
        int value,
        bool merged)
    {
        FromRow = fromRow;
        FromColumn = fromColumn;

        ToRow = toRow;
        ToColumn = toColumn;

        Value = value;

        Merged = merged;
    }
}