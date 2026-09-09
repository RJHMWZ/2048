using System;
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

    /// <summary>
    /// 设置当前方块数值
    /// </summary>
    /// <param name="value">方块数字，0表示空格</param>
    public void SetValue(int value)
    {
        if (value == 0)
        {
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
    }

    /// <summary>
    /// 根据数字获取对应Sprite
    /// </summary>
    private Sprite GetSprite(int value)
    {
        for (int i = 0; i < tileSprites.Length; i++)
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
/// 2048数字与Sprite对应数据
/// </summary>
[Serializable]
public struct TileSpriteData
{
    /// <summary>
    /// 2048数字
    /// </summary>
    public int Value;

    /// <summary>
    /// 数字对应的完整Tile图片
    /// </summary>
    public Sprite Sprite;
}