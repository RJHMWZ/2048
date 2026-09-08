using UnityEngine;

/// <summary>
/// 2048棋盘数据
/// </summary>
public class BoardModel
{
    // 棋盘大小
    public const int Size = 4;

    private readonly int[,] cells = new int[Size, Size];

    /// <summary>
    /// 获取指定位置的数字
    /// </summary>
    public int GetValue(int row, int column)
    {
        return cells[row, column];
    }

    /// <summary>
    /// 设置指定位置的数字
    /// </summary>
    public void SetValue(int row, int column, int value)
    {
        cells[row, column] = value;
    }

    /// <summary>
    /// 清空棋盘
    /// </summary>
    public void Clear()
    {
        for (int row = 0; row < Size; row++)
        {
            for (int column = 0; column < Size; column++)
            {
                cells[row, column] = 0;
            }
        }
    }
}