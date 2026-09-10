/// <summary>
/// 单个方块的移动信息。
/// </summary>
public readonly struct TileMoveInfo
{
    public int FromRow { get; }
    public int FromColumn { get; }
    public int ToRow { get; }
    public int ToColumn { get; }
    public int Value { get; }
    public bool Merged { get; }

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
