using System.Collections.Generic;

/// <summary>
/// 一次完整棋盘移动的只读结果。
/// </summary>
public readonly struct BoardMoveResult
{
    private static readonly IReadOnlyList<TileMoveInfo> NoMoves = new TileMoveInfo[0];

    public static BoardMoveResult Unchanged => new BoardMoveResult(false, 0, NoMoves);

    public bool Moved { get; }
    public int Score { get; }
    public IReadOnlyList<TileMoveInfo> MoveInfos { get; }

    public BoardMoveResult(bool moved, int score, IReadOnlyList<TileMoveInfo> moveInfos)
    {
        Moved = moved;
        Score = score;
        MoveInfos = moveInfos ?? NoMoves;
    }
}
