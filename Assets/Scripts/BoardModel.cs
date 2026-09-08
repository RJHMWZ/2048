using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2048棋盘数据模型
/// </summary>
public class BoardModel
{
    /// <summary>
    /// 棋盘尺寸
    /// </summary>
    public const int Size = 4;

    /// <summary>
    /// 棋盘数据
    /// 0表示空格，其余数字表示方块数值
    /// </summary>
    private readonly int[,] cells = new int[Size, Size];

    /// <summary>
    /// 获取指定位置的数字
    /// </summary>
    /// <param name="row">行</param>
    /// <param name="column">列</param>
    /// <returns>当前位置的数字</returns>
    public int GetValue(int row, int column)
    {
        return cells[row, column];
    }

    /// <summary>
    /// 设置指定位置的数字
    /// </summary>
    /// <param name="row">行</param>
    /// <param name="column">列</param>
    /// <param name="value">数字</param>
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

    /// <summary>
    /// 在随机空位置生成一个新的数字方块
    /// </summary>
    /// <returns>是否生成成功</returns>
    public bool GenerateRandomTile()
    {
        List<Vector2Int> emptyCells = GetEmptyCells();

        //没有空位置，无法继续生成
        if (emptyCells.Count == 0)
            return false;

        //随机选择一个空位置
        int randomIndex = Random.Range(0, emptyCells.Count);
        Vector2Int position = emptyCells[randomIndex];

        //90%概率生成2，10%概率生成4
        int value = Random.value < 0.9f ? 2 : 4;

        //x表示列，y表示行
        cells[position.y, position.x] = value;

        return true;
    }

    /// <summary>
    /// 获取棋盘中所有空位置
    /// </summary>
    /// <returns>所有空位置</returns>
    private List<Vector2Int> GetEmptyCells()
    {
        List<Vector2Int> emptyCells = new List<Vector2Int>();

        for (int row = 0; row < Size; row++)
        {
            for (int column = 0; column < Size; column++)
            {
                if (cells[row, column] == 0)
                {
                    emptyCells.Add(new Vector2Int(column, row));
                }
            }
        }

        return emptyCells;
    }
}