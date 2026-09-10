using TMPro;
using UnityEngine;

/// <summary>
/// 管理暂停、胜利和失败面板。
/// </summary>
public sealed class GameResultUI : MonoBehaviour
{
    [Header("面板")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject pausePanel;

    [Header("失败界面")]
    [SerializeField] private TMP_Text gameOverScoreText;

    public void ResetUI()
    {
        SetActive(victoryPanel, false);
        SetActive(gameOverPanel, false);
        SetActive(pausePanel, false);
    }

    public void ShowPause() => SetActive(pausePanel, true);
    public void HidePause() => SetActive(pausePanel, false);
    public void ShowVictory() => SetActive(victoryPanel, true);
    public void HideVictory() => SetActive(victoryPanel, false);
    public void HideGameOver() => SetActive(gameOverPanel, false);

    public void ShowGameOver(int score)
    {
        SetActive(gameOverPanel, true);

        if (gameOverScoreText != null)
            gameOverScoreText.text = score.ToString();
    }

    private static void SetActive(GameObject panel, bool active)
    {
        if (panel != null)
            panel.SetActive(active);
    }
}
