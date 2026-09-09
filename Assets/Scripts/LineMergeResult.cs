/// <summary>
/// 单行合并结果
/// </summary>
public struct LineMergeResult
{
    /// <summary>
    /// 合并后的行数据
    /// </summary>
    public int[] Line;

    /// <summary>
    /// 本次合并获得的分数
    /// </summary>
    public int Score;

    public LineMergeResult(int[] line, int score)
    {
        Line = line;
        Score = score;
    }
}