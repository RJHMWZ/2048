using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum TileAnimation
{
    None,
    Spawn,
    Merge
}

/// <summary>
/// 显示一个数字方块及其生成、合并动画。
/// </summary>
public sealed class TileView : MonoBehaviour
{
    [SerializeField] private Image tileImage;
    [SerializeField] private TileSpriteData[] tileSprites;

    [Header("动画配置")]
    [SerializeField, Min(0f)] private float spawnDuration = 0.12f;
    [SerializeField, Min(0f)] private float mergeDuration = 0.12f;
    [SerializeField, Min(1f)] private float mergeScale = 1.15f;

    private Coroutine animationCoroutine;

    public void SetValue(int value, TileAnimation animation = TileAnimation.None)
    {
        StopCurrentAnimation();

        if (value == 0)
        {
            transform.localScale = Vector3.one;
            gameObject.SetActive(false);
            return;
        }

        Sprite sprite = GetSprite(value);

        if (tileImage == null || sprite == null)
        {
            Debug.LogWarning($"TileView没有配置数字 {value} 对应的显示资源。", this);
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        tileImage.sprite = sprite;

        switch (animation)
        {
            case TileAnimation.Spawn:
                animationCoroutine = StartCoroutine(SpawnAnimation());
                break;
            case TileAnimation.Merge:
                animationCoroutine = StartCoroutine(MergeAnimation());
                break;
            default:
                transform.localScale = Vector3.one;
                break;
        }
    }

    public void ResetView()
    {
        StopCurrentAnimation();
        transform.localScale = Vector3.one;

        if (tileImage != null)
            tileImage.sprite = null;

        gameObject.SetActive(false);
    }

    public Sprite GetSpriteByValue(int value)
    {
        return GetSprite(value);
    }

    private IEnumerator SpawnAnimation()
    {
        if (spawnDuration <= 0f)
        {
            transform.localScale = Vector3.one;
            animationCoroutine = null;
            yield break;
        }

        float elapsed = 0f;
        transform.localScale = Vector3.zero;

        while (elapsed < spawnDuration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(
                Vector3.zero,
                Vector3.one,
                Mathf.Clamp01(elapsed / spawnDuration));
            yield return null;
        }

        transform.localScale = Vector3.one;
        animationCoroutine = null;
    }

    private IEnumerator MergeAnimation()
    {
        if (mergeDuration <= 0f)
        {
            transform.localScale = Vector3.one;
            animationCoroutine = null;
            yield break;
        }

        float halfDuration = mergeDuration * 0.5f;
        Vector3 expandedScale = Vector3.one * mergeScale;

        yield return AnimateScale(Vector3.one, expandedScale, halfDuration);
        yield return AnimateScale(expandedScale, Vector3.one, halfDuration);

        transform.localScale = Vector3.one;
        animationCoroutine = null;
    }

    private IEnumerator AnimateScale(Vector3 from, Vector3 to, float duration)
    {
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            transform.localScale = Vector3.Lerp(from, to, Mathf.Clamp01(elapsed / duration));
            yield return null;
        }
    }

    private void StopCurrentAnimation()
    {
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
    }

    private Sprite GetSprite(int value)
    {
        if (tileSprites == null)
            return null;

        for (int i = 0; i < tileSprites.Length; i++)
        {
            if (tileSprites[i].Value == value)
                return tileSprites[i].Sprite;
        }

        return null;
    }
}

[Serializable]
public struct TileSpriteData
{
    public int Value;
    public Sprite Sprite;
}
