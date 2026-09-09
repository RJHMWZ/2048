using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 2048方块移动动画对象
/// </summary>
public class TileMoveAnimator : MonoBehaviour
{
    [SerializeField]
    private Image image;

    /// <summary>
    /// 设置Sprite
    /// </summary>
    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }

    /// <summary>
    /// 从起点移动到终点
    /// </summary>
    public IEnumerator Move(
        Vector3 startPosition,
        Vector3 targetPosition,
        float duration)
    {
        transform.position =
            startPosition;

        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float t =
                Mathf.Clamp01(
                    timer / duration
                );

            transform.position =
                Vector3.Lerp(
                    startPosition,
                    targetPosition,
                    t
                );

            yield return null;
        }

        transform.position =
            targetPosition;
    }
}