using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 2048数字方块显示
/// </summary>
public class TileView : MonoBehaviour
{
    [SerializeField]
    private Image tileImage;

    [SerializeField]
    private TileSpriteData[] tileSprites;

    [Header("动画配置")]

    [SerializeField]
    private float spawnDuration = 0.12f;

    [SerializeField]
    private float mergeDuration = 0.12f;

    [SerializeField]
    private float mergeScale = 1.15f;

    /// <summary>
    /// 当前显示的数字
    /// </summary>
    private int currentValue;

    /// <summary>
    /// 当前动画协程
    /// </summary>
    private Coroutine animationCoroutine;

    /// <summary>
    /// 设置当前Tile数值
    /// </summary>
    public void SetValue(int value)
    {
        int previousValue = currentValue;

        currentValue = value;

        //空格
        if (value == 0)
        {
            StopCurrentAnimation();

            transform.localScale = Vector3.one;

            gameObject.SetActive(false);

            return;
        }

        Sprite sprite = GetSprite(value);

        if (sprite == null)
        {
            Debug.LogWarning(
                $"TileView没有配置数字 {value} 对应的Sprite"
            );

            gameObject.SetActive(false);

            return;
        }

        gameObject.SetActive(true);

        tileImage.sprite = sprite;

        //从0变成有数字：新方块生成
        if (previousValue == 0)
        {
            PlaySpawnAnimation();
        }
        //数值发生变化：认为发生了合并
        else if (previousValue != value)
        {
            PlayMergeAnimation();
        }
        else
        {
            transform.localScale = Vector3.one;
        }
    }

    /// <summary>
    /// 播放新方块生成动画
    /// </summary>
    private void PlaySpawnAnimation()
    {
        StopCurrentAnimation();

        animationCoroutine =
            StartCoroutine(SpawnAnimation());
    }

    /// <summary>
    /// 播放合并动画
    /// </summary>
    private void PlayMergeAnimation()
    {
        StopCurrentAnimation();

        animationCoroutine =
            StartCoroutine(MergeAnimation());
    }

    /// <summary>
    /// 新方块生成动画
    /// </summary>
    private IEnumerator SpawnAnimation()
    {
        float timer = 0f;

        transform.localScale = Vector3.zero;

        while (timer < spawnDuration)
        {
            timer += Time.deltaTime;

            float t =
                Mathf.Clamp01(timer / spawnDuration);

            transform.localScale =
                Vector3.Lerp(
                    Vector3.zero,
                    Vector3.one,
                    t
                );

            yield return null;
        }

        transform.localScale = Vector3.one;

        animationCoroutine = null;
    }

    /// <summary>
    /// 合并弹跳动画
    /// </summary>
    private IEnumerator MergeAnimation()
    {
        float halfDuration =
            mergeDuration * 0.5f;

        float timer = 0f;

        Vector3 startScale =
            Vector3.one;

        Vector3 targetScale =
            Vector3.one * mergeScale;

        //第一段：放大
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;

            float t =
                Mathf.Clamp01(timer / halfDuration);

            transform.localScale =
                Vector3.Lerp(
                    startScale,
                    targetScale,
                    t
                );

            yield return null;
        }

        timer = 0f;

        //第二段：缩回
        while (timer < halfDuration)
        {
            timer += Time.deltaTime;

            float t =
                Mathf.Clamp01(timer / halfDuration);

            transform.localScale =
                Vector3.Lerp(
                    targetScale,
                    Vector3.one,
                    t
                );

            yield return null;
        }

        transform.localScale =
            Vector3.one;

        animationCoroutine = null;
    }

    /// <summary>
    /// 停止当前动画
    /// </summary>
    private void StopCurrentAnimation()
    {
        if (animationCoroutine == null)
            return;

        StopCoroutine(animationCoroutine);

        animationCoroutine = null;
    }

    /// <summary>
    /// 重置方块显示状态
    /// </summary>
    public void ResetView()
    {
        StopCurrentAnimation();

        currentValue = 0;
        transform.localScale = Vector3.one;

        if (tileImage != null)
        {
            tileImage.sprite = null;
        }

        gameObject.SetActive(false);
    }

    /// <summary>
    /// 获取指定数字对应的Sprite
    /// </summary>
    public Sprite GetSpriteByValue(int value)
    {
        return GetSprite(value);
    }

    /// <summary>
    /// 获取数字对应的Sprite
    /// </summary>
    private Sprite GetSprite(int value)
    {
        for (int i = 0;
             i < tileSprites.Length;
             i++)
        {
            if (tileSprites[i].Value == value)
            {
                return tileSprites[i].Sprite;
            }
        }

        return null;
    }
}

/// <summary>
/// 数字与Sprite对应数据
/// </summary>
[Serializable]
public struct TileSpriteData
{
    public int Value;

    public Sprite Sprite;
}
