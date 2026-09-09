using System.Collections.Generic;

/// <summary>
/// 单行移动计算结果
/// </summary>
public struct LineMoveResult
{
    /// <summary>
    /// 最终一行的数据
    /// </summary>
    public int[] Line;

    /// <summary>
    /// 本行获得的分数
    /// </summary>
    public int Score;

    /// <summary>
    /// 本行所有Tile移动信息
    /// </summary>
    public List<TileMoveInfo> MoveInfos;

    public LineMoveResult(
        int[] line,
        int score,
        List<TileMoveInfo> moveInfos)
    {
        Line = line;

        Score = score;

        MoveInfos = moveInfos;
    }
}