using System.Collections.Generic;

/// <summary>
/// 计算单行移动、合并及动画所需的位置信息。
/// </summary>
public static class LineMergeUtility
{
    public static LineMoveResult Merge(
        IReadOnlyList<LineTileData> tiles,
        int lineIndex,
        MoveDirection direction)
    {
        int[] line = new int[BoardModel.Size];
        List<TileMoveInfo> moveInfos = new List<TileMoveInfo>();
        int score = 0;
        int sourceIndex = 0;
        int targetOffset = 0;

        while (sourceIndex < tiles.Count)
        {
            LineTileData current = tiles[sourceIndex];
            bool canMerge = sourceIndex + 1 < tiles.Count &&
                            tiles[sourceIndex + 1].Value == current.Value;

            BoardModel.GetPosition(
                direction,
                lineIndex,
                targetOffset,
                out int targetRow,
                out int targetColumn);

            if (canMerge)
            {
                LineTileData next = tiles[sourceIndex + 1];
                int mergedValue = current.Value * 2;

                line[targetOffset] = mergedValue;
                score += mergedValue;
                moveInfos.Add(CreateMoveInfo(current, targetRow, targetColumn, true));
                moveInfos.Add(CreateMoveInfo(next, targetRow, targetColumn, true));
                sourceIndex += 2;
            }
            else
            {
                line[targetOffset] = current.Value;

                if (current.Row != targetRow || current.Column != targetColumn)
                    moveInfos.Add(CreateMoveInfo(current, targetRow, targetColumn, false));

                sourceIndex++;
            }

            targetOffset++;
        }

        return new LineMoveResult(line, score, moveInfos);
    }

    private static TileMoveInfo CreateMoveInfo(
        LineTileData source,
        int targetRow,
        int targetColumn,
        bool merged)
    {
        return new TileMoveInfo(
            source.Row,
            source.Column,
            targetRow,
            targetColumn,
            source.Value,
            merged);
    }
}
