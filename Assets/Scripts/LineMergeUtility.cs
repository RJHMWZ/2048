using System.Collections.Generic;

/// <summary>
/// 2048单行移动与合并工具
/// </summary>
public static class LineMergeUtility
{
    /// <summary>
    /// 普通单行左合并
    /// </summary>
    public static LineMergeResult MergeLeft(int[] line)
    {
        if (line == null)
        {
            return new LineMergeResult(null, 0);
        }

        List<int> numbers = new List<int>();

        for (int i = 0; i < line.Length; i++)
        {
            if (line[i] != 0)
            {
                numbers.Add(line[i]);
            }
        }

        int score = 0;

        for (int i = 0; i < numbers.Count - 1; i++)
        {
            if (numbers[i] == numbers[i + 1])
            {
                numbers[i] *= 2;

                score += numbers[i];

                numbers.RemoveAt(i + 1);
            }
        }

        int[] result = new int[line.Length];

        for (int i = 0; i < numbers.Count; i++)
        {
            result[i] = numbers[i];
        }

        return new LineMergeResult(result, score);
    }

    /// <summary>
    /// 计算向左移动，并记录每个Tile的移动信息
    /// </summary>
    public static LineMoveResult MergeLeftWithMoveInfo(
        List<LineTileData> tiles,
        int fixedIndex,
        bool horizontal,
        bool reversed)
    {
        int[] result = new int[BoardModel.Size];

        List<TileMoveInfo> moveInfos =
            new List<TileMoveInfo>();

        int score = 0;

        int targetIndex = 0;

        int i = 0;

        while (i < tiles.Count)
        {
            LineTileData current = tiles[i];

            bool canMerge =
                i + 1 < tiles.Count &&
                tiles[i + 1].Value == current.Value;

            if (canMerge)
            {
                LineTileData next =
                    tiles[i + 1];

                int mergedValue =
                    current.Value * 2;

                result[targetIndex] =
                    mergedValue;

                score += mergedValue;

                GetTargetPosition(
                    fixedIndex,
                    targetIndex,
                    horizontal,
                    reversed,
                    out int targetRow,
                    out int targetColumn
                );

                moveInfos.Add(
                    new TileMoveInfo(
                        current.Row,
                        current.Column,
                        targetRow,
                        targetColumn,
                        current.Value,
                        true
                    )
                );

                moveInfos.Add(
                    new TileMoveInfo(
                        next.Row,
                        next.Column,
                        targetRow,
                        targetColumn,
                        next.Value,
                        true
                    )
                );

                i += 2;
            }
            else
            {
                result[targetIndex] =
                    current.Value;

                GetTargetPosition(
                    fixedIndex,
                    targetIndex,
                    horizontal,
                    reversed,
                    out int targetRow,
                    out int targetColumn
                );

                //只有位置发生变化才记录移动
                if (current.Row != targetRow ||
                    current.Column != targetColumn)
                {
                    moveInfos.Add(
                        new TileMoveInfo(
                            current.Row,
                            current.Column,
                            targetRow,
                            targetColumn,
                            current.Value,
                            false
                        )
                    );
                }

                i++;
            }

            targetIndex++;
        }

        return new LineMoveResult(
            result,
            score,
            moveInfos
        );
    }

    /// <summary>
    /// 根据方向计算最终棋盘坐标
    /// </summary>
    private static void GetTargetPosition(
        int fixedIndex,
        int targetIndex,
        bool horizontal,
        bool reversed,
        out int row,
        out int column)
    {
        int index = reversed
            ? BoardModel.Size - 1 - targetIndex
            : targetIndex;

        if (horizontal)
        {
            row = fixedIndex;
            column = index;
        }
        else
        {
            row = index;
            column = fixedIndex;
        }
    }
}