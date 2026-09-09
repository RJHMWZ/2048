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
    public BoardMoveResult Move(
    MoveDirection direction)
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

        return new BoardMoveResult(
            false,
            0,
            new List<TileMoveInfo>()
        );
    }

    /// <summary>
    /// 向左移动
    /// </summary>
    private BoardMoveResult MoveLeft()
    {
        bool moved = false;
        int score = 0;

        List<TileMoveInfo> moveInfos =
            new List<TileMoveInfo>();

        for (int row = 0; row < Size; row++)
        {
            List<LineTileData> tiles =
                new List<LineTileData>();

            //从左往右读取非0数字
            for (int column = 0;
                column < Size;
                column++)
            {
                int value =
                    cells[row, column];

                if (value == 0)
                    continue;

                tiles.Add(
                    new LineTileData(
                        value,
                        row,
                        column
                    )
                );
            }

            LineMoveResult result =
                LineMergeUtility.MergeLeftWithMoveInfo(
                    tiles,
                    row,
                    true,
                    false
                );

            score += result.Score;

            moveInfos.AddRange(
                result.MoveInfos
            );

            for (int column = 0;
                column < Size;
                column++)
            {
                if (cells[row, column] !=
                    result.Line[column])
                {
                    moved = true;
                }

                cells[row, column] =
                    result.Line[column];
            }
        }

        return new BoardMoveResult(
            moved,
            score,
            moveInfos
        );
    }

    /// <summary>
    /// 向右移动
    /// </summary>
    private BoardMoveResult MoveRight()
    {
        bool moved = false;
        int score = 0;

        List<TileMoveInfo> moveInfos =
            new List<TileMoveInfo>();

        for (int row = 0;
            row < Size;
            row++)
        {
            List<LineTileData> tiles =
                new List<LineTileData>();

            //从右往左读取
            for (int column = Size - 1;
                column >= 0;
                column--)
            {
                int value =
                    cells[row, column];

                if (value == 0)
                    continue;

                tiles.Add(
                    new LineTileData(
                        value,
                        row,
                        column
                    )
                );
            }

            LineMoveResult result =
                LineMergeUtility.MergeLeftWithMoveInfo(
                    tiles,
                    row,
                    true,
                    true
                );

            score += result.Score;

            moveInfos.AddRange(
                result.MoveInfos
            );

            for (int column = 0;
                column < Size;
                column++)
            {
                if (cells[row, column] !=
                    result.Line[Size - 1 - column])
                {
                    moved = true;
                }

                cells[row, column] =
                    result.Line[Size - 1 - column];
            }
        }

        return new BoardMoveResult(
            moved,
            score,
            moveInfos
        );
    }

    /// <summary>
    /// 向上移动
    /// </summary>
    private BoardMoveResult MoveUp()
    {
        bool moved = false;
        int score = 0;

        List<TileMoveInfo> moveInfos =
            new List<TileMoveInfo>();

        for (int column = 0;
            column < Size;
            column++)
        {
            List<LineTileData> tiles =
                new List<LineTileData>();

            //从上往下读取
            for (int row = 0;
                row < Size;
                row++)
            {
                int value =
                    cells[row, column];

                if (value == 0)
                    continue;

                tiles.Add(
                    new LineTileData(
                        value,
                        row,
                        column
                    )
                );
            }

            LineMoveResult result =
                LineMergeUtility.MergeLeftWithMoveInfo(
                    tiles,
                    column,
                    false,
                    false
                );

            score += result.Score;

            moveInfos.AddRange(
                result.MoveInfos
            );

            for (int row = 0;
                row < Size;
                row++)
            {
                if (cells[row, column] !=
                    result.Line[row])
                {
                    moved = true;
                }

                cells[row, column] =
                    result.Line[row];
            }
        }

        return new BoardMoveResult(
            moved,
            score,
            moveInfos
        );
    }

    /// <summary>
    /// 向下移动
    /// </summary>
    private BoardMoveResult MoveDown()
    {
        bool moved = false;
        int score = 0;

        List<TileMoveInfo> moveInfos =
            new List<TileMoveInfo>();

        for (int column = 0;
            column < Size;
            column++)
        {
            List<LineTileData> tiles =
                new List<LineTileData>();

            //从下往上读取
            for (int row = Size - 1;
                row >= 0;
                row--)
            {
                int value =
                    cells[row, column];

                if (value == 0)
                    continue;

                tiles.Add(
                    new LineTileData(
                        value,
                        row,
                        column
                    )
                );
            }

            LineMoveResult result =
                LineMergeUtility.MergeLeftWithMoveInfo(
                    tiles,
                    column,
                    false,
                    true
                );

            score += result.Score;

            moveInfos.AddRange(
                result.MoveInfos
            );

            for (int row = 0;
                row < Size;
                row++)
            {
                if (cells[row, column] !=
                    result.Line[Size - 1 - row])
                {
                    moved = true;
                }

                cells[row, column] =
                    result.Line[Size - 1 - row];
            }
        }

        return new BoardMoveResult(
            moved,
            score,
            moveInfos
        );
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

    /// <summary>
    /// 检查棋盘中是否存在指定数字
    /// </summary>
    /// <param name="targetValue">目标数字</param>
    /// <returns>是否存在</returns>
    public bool HasValue(int targetValue)
    {
        for (int row = 0; row < Size; row++)
        {
            for (int column = 0; column < Size; column++)
            {
                if (cells[row, column] == targetValue)
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 判断当前棋盘是否还能继续移动
    /// </summary>
    /// <returns>是否存在可移动空间</returns>
    public bool CanMove()
    {
        //第一步：只要存在空格，就一定还能继续游戏
        for (int row = 0; row < Size; row++)
        {
            for (int column = 0; column < Size; column++)
            {
                if (cells[row, column] == 0)
                {
                    return true;
                }
            }
        }

        //第二步：检查左右相邻数字是否可以合并
        for (int row = 0; row < Size; row++)
        {
            for (int column = 0; column < Size - 1; column++)
            {
                if (cells[row, column] ==cells[row, column + 1])
                {
                    return true;
                }
            }
        }

        //第三步：检查上下相邻数字是否可以合并
        for (int column = 0; column < Size; column++)
        {
            for (int row = 0; row < Size - 1; row++)
            {
                if (cells[row, column] ==
                    cells[row + 1, column])
                {
                    return true;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// 判断游戏是否结束
    /// </summary>
    /// <returns>是否Game Over</returns>
    public bool IsGameOver()
    {
        return !CanMove();
    }
}