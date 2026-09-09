/// <summary>
/// 棋盘移动结果
/// </summary>
public struct BoardMoveResult
{
    /// <summary>
    /// 棋盘是否发生变化
    /// </summary>
    public bool Moved;

    /// <summary>
    /// 本次移动获得的分数
    /// </summary>
    public int Score;

    public BoardMoveResult(bool moved, int score)
    {
        Moved = moved;
        Score = score;
    }
}