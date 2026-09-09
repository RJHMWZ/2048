using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// 2048棋盘显示控制
/// </summary>
public class BoardView : MonoBehaviour
{
    [Header("基础引用")]

    [SerializeField]
    private Game2048Manager gameManager;

    /// <summary>
    /// 16个固定Tile显示对象
    /// 顺序：
    /// 0  1  2  3
    /// 4  5  6  7
    /// 8  9 10 11
    /// 12 13 14 15
    /// </summary>
    [SerializeField]
    private TileView[] tiles;

    [Header("移动动画")]

    /// <summary>
    /// 16个棋盘格子的RectTransform
    /// 顺序必须和tiles完全一致
    /// </summary>
    [SerializeField]
    private RectTransform[] cellRects;

    /// <summary>
    /// 临时移动Tile的父节点
    /// </summary>
    [SerializeField]
    private Transform animationLayer;

    /// <summary>
    /// 移动动画使用的临时Tile Prefab
    /// </summary>
    [SerializeField]
    private TileMoveAnimator moveTilePrefab;

    /// <summary>
    /// Tile移动动画持续时间
    /// </summary>
    [SerializeField]
    private float moveDuration = 0.11f;

    /// <summary>
    /// 刷新整个棋盘
    /// </summary>
    public void Refresh()
    {
        if (!CheckReferences())
            return;

        BoardModel board = gameManager.Board;

        if (board == null)
            return;

        int index = 0;

        for (int row = 0;
             row < BoardModel.Size;
             row++)
        {
            for (int column = 0;
                 column < BoardModel.Size;
                 column++)
            {
                int value =
                    board.GetValue(row, column);

                tiles[index].SetValue(value);

                index++;
            }
        }
    }

    /// <summary>
    /// 播放本次棋盘移动动画
    /// </summary>
    /// <param name="result">棋盘移动结果</param>
    public IEnumerator PlayMoveAnimation(
        BoardMoveResult result)
    {
        if (!CheckReferences())
            yield break;

        if (moveTilePrefab == null)
        {
            Debug.LogError(
                "BoardView没有配置Move Tile Prefab"
            );

            yield break;
        }

        if (animationLayer == null)
        {
            Debug.LogError(
                "BoardView没有配置Animation Layer"
            );

            yield break;
        }

        if (result.MoveInfos == null ||
            result.MoveInfos.Count == 0)
        {
            yield break;
        }

        int runningAnimations =
            result.MoveInfos.Count;

        for (int i = 0;
             i < result.MoveInfos.Count;
             i++)
        {
            TileMoveInfo info =
                result.MoveInfos[i];

            int fromIndex =
                GetIndex(
                    info.FromRow,
                    info.FromColumn
                );

            int toIndex =
                GetIndex(
                    info.ToRow,
                    info.ToColumn
                );

            //防止索引异常
            if (!IsValidIndex(fromIndex) ||
                !IsValidIndex(toIndex))
            {
                runningAnimations--;

                continue;
            }

            TileMoveAnimator animator =
                Instantiate(
                    moveTilePrefab,
                    animationLayer
                );

            if (animator == null)
            {
                runningAnimations--;

                continue;
            }

            Sprite sprite =
                tiles[fromIndex]
                    .GetSpriteByValue(
                        info.Value
                    );

            animator.SetSprite(sprite);

            Vector3 startPosition =
                cellRects[fromIndex].position;

            Vector3 targetPosition =
                cellRects[toIndex].position;

            StartCoroutine(
                RunMoveAnimation(
                    animator,
                    startPosition,
                    targetPosition,
                    () =>
                    {
                        runningAnimations--;
                    }
                )
            );
        }

        //等待所有Tile移动完成
        while (runningAnimations > 0)
        {
            yield return null;
        }
    }

    /// <summary>
    /// 播放单个Tile移动动画
    /// </summary>
    private IEnumerator RunMoveAnimation(
        TileMoveAnimator animator,
        Vector3 startPosition,
        Vector3 targetPosition,
        Action onComplete)
    {
        if (animator != null)
        {
            yield return animator.Move(
                startPosition,
                targetPosition,
                moveDuration
            );

            Destroy(animator.gameObject);
        }

        onComplete?.Invoke();
    }

    /// <summary>
    /// 暂时隐藏所有固定Tile
    /// </summary>
    public void HideTiles()
    {
        if (tiles == null)
            return;

        for (int i = 0;
             i < tiles.Length;
             i++)
        {
            if (tiles[i] != null)
            {
                tiles[i].gameObject
                    .SetActive(false);
            }
        }
    }

    /// <summary>
    /// 重置棋盘显示状态
    /// </summary>
    public void ResetView()
    {
        if (tiles == null)
            return;

        for (int i = 0;
             i < tiles.Length;
             i++)
        {
            if (tiles[i] != null)
            {
                tiles[i].ResetView();
            }
        }
    }

    /// <summary>
    /// 根据行列获取Tile数组下标
    /// </summary>
    private int GetIndex(
        int row,
        int column)
    {
        return row * BoardModel.Size
               + column;
    }

    /// <summary>
    /// 判断下标是否合法
    /// </summary>
    private bool IsValidIndex(int index)
    {
        return index >= 0 &&
               index <
               BoardModel.Size *
               BoardModel.Size;
    }

    /// <summary>
    /// 检查BoardView基础配置
    /// </summary>
    private bool CheckReferences()
    {
        if (gameManager == null)
        {
            Debug.LogError(
                "BoardView没有配置Game2048Manager"
            );

            return false;
        }

        int requiredCount =
            BoardModel.Size *
            BoardModel.Size;

        if (tiles == null ||
            tiles.Length != requiredCount)
        {
            Debug.LogError(
                "BoardView必须配置16个TileView"
            );

            return false;
        }

        if (cellRects == null ||
            cellRects.Length != requiredCount)
        {
            Debug.LogError(
                "BoardView必须配置16个Cell RectTransform"
            );

            return false;
        }

        return true;
    }
}