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
    /// 0表示空格
    /// </summary>
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

    /// <summary>
    /// 随机生成一个新的数字方块
    /// </summary>
    public bool GenerateRandomTile()
    {
        List<Vector2Int> emptyCells = GetEmptyCells();

        if (emptyCells.Count == 0)
            return false;

        int randomIndex = Random.Range(0, emptyCells.Count);

        Vector2Int position = emptyCells[randomIndex];

        int value = Random.value < 0.9f ? 2 : 4;

        cells[position.y, position.x] = value;

        return true;
    }

    /// <summary>
    /// 根据指定方向移动棋盘
    /// </summary>
    public BoardMoveResult Move(MoveDirection direction)
    {
        switch (direction)
        {
            case MoveDirection.Left:
                return MoveLeft();

            case MoveDirection.Right:
                return MoveRight();

            case MoveDirection.Up:
                return MoveUp();

            case MoveDirection.Down:
                return MoveDown();
        }

        return new BoardMoveResult(false, 0);
    }

    /// <summary>
    /// 向左移动
    /// </summary>
    private BoardMoveResult MoveLeft()
    {
        bool moved = false;
        int score = 0;

        for (int row = 0; row < Size; row++)
        {
            int[] line = new int[Size];

            //读取当前行
            for (int column = 0; column < Size; column++)
            {
                line[column] = cells[row, column];
            }

            //计算当前行的合并结果
            LineMergeResult mergeResult =
                LineMergeUtility.MergeLeft(line);

            int[] result = mergeResult.Line;

            //累加这一行获得的分数
            score += mergeResult.Score;

            //比较并写回
            for (int column = 0; column < Size; column++)
            {
                if (cells[row, column] != result[column])
                {
                    moved = true;
                }

                cells[row, column] = result[column];
            }
        }

        return new BoardMoveResult(moved, score);
    }

    /// <summary>
    /// 向右移动
    /// </summary>
    private BoardMoveResult MoveRight()
    {
        bool moved = false;
        int score = 0;

        for (int row = 0; row < Size; row++)
        {
            int[] line = new int[Size];

            //反向读取当前行
            for (int column = 0; column < Size; column++)
            {
                line[column] =
                    cells[row, Size - 1 - column];
            }

            LineMergeResult mergeResult =
                LineMergeUtility.MergeLeft(line);

            int[] result = mergeResult.Line;

            score += mergeResult.Score;

            //反向写回
            for (int column = 0; column < Size; column++)
            {
                int targetColumn =
                    Size - 1 - column;

                if (cells[row, targetColumn] != result[column])
                {
                    moved = true;
                }

                cells[row, targetColumn] = result[column];
            }
        }

        return new BoardMoveResult(moved, score);
    }

    /// <summary>
    /// 向上移动
    /// </summary>
    private BoardMoveResult MoveUp()
    {
        bool moved = false;
        int score = 0;

        for (int column = 0; column < Size; column++)
        {
            int[] line = new int[Size];

            //从上往下读取
            for (int row = 0; row < Size; row++)
            {
                line[row] = cells[row, column];
            }

            LineMergeResult mergeResult =
                LineMergeUtility.MergeLeft(line);

            int[] result = mergeResult.Line;

            score += mergeResult.Score;

            //从上往下写回
            for (int row = 0; row < Size; row++)
            {
                if (cells[row, column] != result[row])
                {
                    moved = true;
                }

                cells[row, column] = result[row];
            }
        }

        return new BoardMoveResult(moved, score);
    }

    /// <summary>
    /// 向下移动
    /// </summary>
    private BoardMoveResult MoveDown()
    {
        bool moved = false;
        int score = 0;

        for (int column = 0; column < Size; column++)
        {
            int[] line = new int[Size];

            //从下往上读取
            for (int row = 0; row < Size; row++)
            {
                line[row] =
                    cells[Size - 1 - row, column];
            }

            LineMergeResult mergeResult =
                LineMergeUtility.MergeLeft(line);

            int[] result = mergeResult.Line;

            score += mergeResult.Score;

            //从下往上写回
            for (int row = 0; row < Size; row++)
            {
                int targetRow =
                    Size - 1 - row;

                if (cells[targetRow, column] != result[row])
                {
                    moved = true;
                }

                cells[targetRow, column] = result[row];
            }
        }

        return new BoardMoveResult(moved, score);
    }

    /// <summary>
    /// 获取所有空位置
    /// </summary>
    private List<Vector2Int> GetEmptyCells()
    {
        List<Vector2Int> emptyCells = new List<Vector2Int>();

        for (int row = 0; row < Size; row++)
        {
            for (int column = 0; column < Size; column++)
            {
                if (cells[row, column] == 0)
                {
                    emptyCells.Add(
                        new Vector2Int(column, row)
                    );
                }
            }
        }

        return emptyCells;
    }
}