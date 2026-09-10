using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 将棋盘模型渲染到16个固定方块，并负责移动动画。
/// </summary>
public sealed class BoardView : MonoBehaviour
{
    [Header("基础引用")]
    [SerializeField] private Game2048Manager gameManager;
    [SerializeField] private TileView[] tiles;

    [Header("移动动画")]
    [SerializeField] private RectTransform[] cellRects;
    [SerializeField] private Transform animationLayer;
    [SerializeField] private TileMoveAnimator moveTilePrefab;
    [SerializeField, Min(0f)] private float moveDuration = 0.11f;

    private readonly List<TileMoveAnimator> activeAnimators = new List<TileMoveAnimator>();

    public void Refresh()
    {
        if (!HasValidTiles() || gameManager.Board == null)
            return;

        RefreshTiles(null, -1, true);
    }

    public void RefreshAfterMove(BoardMoveResult result, Vector2Int? spawnedPosition)
    {
        if (!HasValidTiles() || gameManager.Board == null)
            return;

        HashSet<int> mergedTargets = new HashSet<int>();
        IReadOnlyList<TileMoveInfo> moveInfos = result.MoveInfos;

        if (moveInfos != null)
        {
            for (int i = 0; i < moveInfos.Count; i++)
            {
                TileMoveInfo info = moveInfos[i];

                if (info.Merged)
                    mergedTargets.Add(GetIndex(info.ToRow, info.ToColumn));
            }
        }

        int spawnedIndex = spawnedPosition.HasValue
            ? GetIndex(spawnedPosition.Value.y, spawnedPosition.Value.x)
            : -1;

        RefreshTiles(mergedTargets, spawnedIndex, false);
    }

    private void RefreshTiles(HashSet<int> mergedTargets, int spawnedIndex, bool animateExisting)
    {
        for (int row = 0; row < BoardModel.Size; row++)
        {
            for (int column = 0; column < BoardModel.Size; column++)
            {
                int index = GetIndex(row, column);
                int value = gameManager.Board.GetValue(row, column);
                TileAnimation animation = TileAnimation.None;

                if (value != 0 && (animateExisting || index == spawnedIndex))
                    animation = TileAnimation.Spawn;
                else if (mergedTargets != null && mergedTargets.Contains(index))
                    animation = TileAnimation.Merge;

                tiles[index].SetValue(value, animation);
            }
        }
    }

    public IEnumerator PlayMoveAnimation(BoardMoveResult result)
    {
        IReadOnlyList<TileMoveInfo> moveInfos = result.MoveInfos;

        if (moveInfos == null || moveInfos.Count == 0 || !HasValidAnimationPositions())
            yield break;

        Transform targetLayer = GetAnimationLayer();

        if (targetLayer == null)
            yield break;

        int runningAnimations = 0;

        for (int i = 0; i < moveInfos.Count; i++)
        {
            TileMoveInfo info = moveInfos[i];
            int fromIndex = GetIndex(info.FromRow, info.FromColumn);
            int toIndex = GetIndex(info.ToRow, info.ToColumn);

            if (!IsValidIndex(fromIndex) || !IsValidIndex(toIndex))
                continue;

            Sprite sprite = tiles[fromIndex].GetSpriteByValue(info.Value);

            if (sprite == null)
                continue;

            TileMoveAnimator animator = CreateAnimator(targetLayer, cellRects[fromIndex]);

            if (animator == null || !animator.SetSprite(sprite))
            {
                if (animator != null)
                    Destroy(animator.gameObject);

                continue;
            }

            runningAnimations++;
            activeAnimators.Add(animator);
            StartCoroutine(RunMoveAnimation(
                animator,
                cellRects[fromIndex].position,
                cellRects[toIndex].position,
                () => runningAnimations--));
        }

        while (runningAnimations > 0)
            yield return null;
    }

    public void HideTiles()
    {
        if (tiles == null)
            return;

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null)
                tiles[i].gameObject.SetActive(false);
        }
    }

    public void ResetView()
    {
        StopAllCoroutines();
        ClearActiveAnimators();

        if (tiles == null)
            return;

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] != null)
                tiles[i].ResetView();
        }
    }

    private IEnumerator RunMoveAnimation(
        TileMoveAnimator animator,
        Vector3 startPosition,
        Vector3 targetPosition,
        Action onComplete)
    {
        yield return animator.Move(startPosition, targetPosition, moveDuration);

        activeAnimators.Remove(animator);

        if (animator != null)
            Destroy(animator.gameObject);

        onComplete?.Invoke();
    }

    private TileMoveAnimator CreateAnimator(Transform targetLayer, RectTransform sourceCell)
    {
        TileMoveAnimator animator;

        if (moveTilePrefab != null)
        {
            animator = Instantiate(moveTilePrefab, targetLayer);
        }
        else
        {
            GameObject animationObject = new GameObject(
                "Moving Tile",
                typeof(RectTransform),
                typeof(CanvasRenderer),
                typeof(Image),
                typeof(TileMoveAnimator));

            animationObject.transform.SetParent(targetLayer, false);
            Image image = animationObject.GetComponent<Image>();
            image.raycastTarget = false;
            image.preserveAspect = true;

            animator = animationObject.GetComponent<TileMoveAnimator>();
            animator.Initialize(image);
        }

        animator.SetSize(sourceCell.rect.size);
        return animator;
    }

    private Transform GetAnimationLayer()
    {
        if (animationLayer != null)
            return animationLayer;

        Canvas canvas = GetComponentInParent<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("BoardView找不到用于播放移动动画的Canvas。", this);
            return null;
        }

        animationLayer = canvas.transform;
        return animationLayer;
    }

    private bool HasValidTiles()
    {
        if (gameManager == null)
        {
            Debug.LogError("BoardView没有配置Game2048Manager。", this);
            return false;
        }

        int requiredCount = BoardModel.Size * BoardModel.Size;

        if (tiles == null || tiles.Length != requiredCount)
        {
            Debug.LogError("BoardView必须配置16个TileView。", this);
            return false;
        }

        for (int i = 0; i < tiles.Length; i++)
        {
            if (tiles[i] == null)
            {
                Debug.LogError($"BoardView的TileView索引 {i} 未配置。", this);
                return false;
            }
        }

        return true;
    }

    private bool HasValidAnimationPositions()
    {
        if (!HasValidTiles())
            return false;

        int requiredCount = BoardModel.Size * BoardModel.Size;

        if (cellRects == null || cellRects.Length != requiredCount)
        {
            Debug.LogWarning("BoardView未完整配置16个格子位置，将跳过移动动画。", this);
            return false;
        }

        for (int i = 0; i < cellRects.Length; i++)
        {
            if (cellRects[i] == null)
            {
                Debug.LogWarning($"BoardView的格子位置索引 {i} 未配置，将跳过移动动画。", this);
                return false;
            }
        }

        return true;
    }

    private void ClearActiveAnimators()
    {
        for (int i = 0; i < activeAnimators.Count; i++)
        {
            if (activeAnimators[i] != null)
                Destroy(activeAnimators[i].gameObject);
        }

        activeAnimators.Clear();
    }

    private static int GetIndex(int row, int column)
    {
        return row * BoardModel.Size + column;
    }

    private static bool IsValidIndex(int index)
    {
        return index >= 0 && index < BoardModel.Size * BoardModel.Size;
    }
}
