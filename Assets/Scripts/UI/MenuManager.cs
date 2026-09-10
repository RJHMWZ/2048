using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 主菜单按钮绑定。
/// </summary>
public sealed class MenuManager : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;

    private void OnEnable()
    {
        if (playButton != null)
            playButton.onClick.AddListener(GameManager.PlayGame);

        if (exitButton != null)
            exitButton.onClick.AddListener(GameManager.ExitGame);
    }

    private void OnDisable()
    {
        if (playButton != null)
            playButton.onClick.RemoveListener(GameManager.PlayGame);

        if (exitButton != null)
            exitButton.onClick.RemoveListener(GameManager.ExitGame);
    }
}
