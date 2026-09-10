using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 临时方块的移动动画组件。
/// </summary>
public sealed class TileMoveAnimator : MonoBehaviour
{
    [SerializeField] private Image image;

    public void Initialize(Image targetImage)
    {
        image = targetImage;
    }

    public bool SetSprite(Sprite sprite)
    {
        if (image == null || sprite == null)
            return false;

        image.sprite = sprite;
        return true;
    }

    public void SetSize(Vector2 size)
    {
        if (transform is RectTransform rectTransform)
            rectTransform.sizeDelta = size;
    }

    public IEnumerator Move(Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        transform.position = startPosition;

        if (duration <= 0f)
        {
            transform.position = targetPosition;
            yield break;
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration);
            transform.position = Vector3.Lerp(startPosition, targetPosition, progress);
            yield return null;
        }

        transform.position = targetPosition;
    }
}
