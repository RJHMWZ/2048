using System.Collections.Generic;

/// <summary>
/// 单行移动计算的只读结果。
/// </summary>
public readonly struct LineMoveResult
{
    public int[] Line { get; }
    public int Score { get; }
    public IReadOnlyList<TileMoveInfo> MoveInfos { get; }

    public LineMoveResult(int[] line, int score, IReadOnlyList<TileMoveInfo> moveInfos)
    {
        Line = line;
        Score = score;
        MoveInfos = moveInfos;
    }
}
