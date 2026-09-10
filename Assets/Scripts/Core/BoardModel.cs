using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 2048棋盘数据模型。
/// </summary>
public sealed class BoardModel
{
    public const int Size = 4;

    private readonly int[,] cells = new int[Size, Size];

    public int GetValue(int row, int column)
    {
        ValidatePosition(row, column);
        return cells[row, column];
    }

    public void SetValue(int row, int column, int value)
    {
        ValidatePosition(row, column);

        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Tile value cannot be negative.");

        cells[row, column] = value;
    }

    public void Clear()
    {
        Array.Clear(cells, 0, cells.Length);
    }

    public bool GenerateRandomTile()
    {
        return TryGenerateRandomTile(out _);
    }

    public bool TryGenerateRandomTile(out Vector2Int position)
    {
        List<Vector2Int> emptyCells = GetEmptyCells();

        if (emptyCells.Count == 0)
        {
            position = default;
            return false;
        }

        position = emptyCells[UnityEngine.Random.Range(0, emptyCells.Count)];
        cells[position.y, position.x] = UnityEngine.Random.value < 0.9f ? 2 : 4;
        return true;
    }

    public BoardMoveResult Move(MoveDirection direction)
    {
        if (!Enum.IsDefined(typeof(MoveDirection), direction))
            return BoardMoveResult.Unchanged;

        bool moved = false;
        int score = 0;
        List<TileMoveInfo> moveInfos = new List<TileMoveInfo>();

        for (int lineIndex = 0; lineIndex < Size; lineIndex++)
        {
            List<LineTileData> tiles = ReadLine(direction, lineIndex);
            LineMoveResult result = LineMergeUtility.Merge(tiles, lineIndex, direction);

            score += result.Score;
            moveInfos.AddRange(result.MoveInfos);

            for (int offset = 0; offset < Size; offset++)
            {
                GetPosition(direction, lineIndex, offset, out int row, out int column);

                if (cells[row, column] != result.Line[offset])
                    moved = true;

                cells[row, column] = result.Line[offset];
            }
        }

        return new BoardMoveResult(moved, score, moveInfos);
    }

    public bool HasValue(int targetValue)
    {
        for (int row = 0; row < Size; row++)
        {
            for (int column = 0; column < Size; column++)
            {
                if (cells[row, column] == targetValue)
                    return true;
            }
        }

        return false;
    }

    public bool CanMove()
    {
        for (int row = 0; row < Size; row++)
        {
            for (int column = 0; column < Size; column++)
            {
                int value = cells[row, column];

                if (value == 0 ||
                    column + 1 < Size && cells[row, column + 1] == value ||
                    row + 1 < Size && cells[row + 1, column] == value)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsGameOver()
    {
        return !CanMove();
    }

    private List<Vector2Int> GetEmptyCells()
    {
        List<Vector2Int> emptyCells = new List<Vector2Int>();

        for (int row = 0; row < Size; row++)
        {
            for (int column = 0; column < Size; column++)
            {
                if (cells[row, column] == 0)
                    emptyCells.Add(new Vector2Int(column, row));
            }
        }

        return emptyCells;
    }

    private List<LineTileData> ReadLine(MoveDirection direction, int lineIndex)
    {
        List<LineTileData> tiles = new List<LineTileData>(Size);

        for (int offset = 0; offset < Size; offset++)
        {
            GetPosition(direction, lineIndex, offset, out int row, out int column);
            int value = cells[row, column];

            if (value != 0)
                tiles.Add(new LineTileData(value, row, column));
        }

        return tiles;
    }

    internal static void GetPosition(
        MoveDirection direction,
        int lineIndex,
        int offset,
        out int row,
        out int column)
    {
        switch (direction)
        {
            case MoveDirection.Left:
                row = lineIndex;
                column = offset;
                return;
            case MoveDirection.Right:
                row = lineIndex;
                column = Size - 1 - offset;
                return;
            case MoveDirection.Up:
                row = offset;
                column = lineIndex;
                return;
            case MoveDirection.Down:
                row = Size - 1 - offset;
                column = lineIndex;
                return;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }

    private static void ValidatePosition(int row, int column)
    {
        if (row < 0 || row >= Size)
            throw new ArgumentOutOfRangeException(nameof(row));

        if (column < 0 || column >= Size)
            throw new ArgumentOutOfRangeException(nameof(column));
    }
}
