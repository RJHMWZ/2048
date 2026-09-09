using TMPro;
using UnityEngine;

/// <summary>
/// 2048胜利与失败界面
/// </summary>
public class GameResultUI : MonoBehaviour
{
    [Header("面板")]
    [SerializeField]
    private GameObject victoryPanel;

    [SerializeField]
    private GameObject gameOverPanel;

    [SerializeField]
    private GameObject pausePanel;

    [Header("失败界面")]
    [SerializeField]
    private TMP_Text gameOverScoreText;

    /// <summary>
    /// 初始化结果界面
    /// </summary>
    public void ResetUI()
    {
        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    /// <summary>
    /// 显示暂停界面
    /// </summary>
    public void ShowPause()
    {
        if (pausePanel == null)
            return;

        pausePanel.SetActive(true);
    }

    /// <summary>
    /// 关闭暂停界面
    /// </summary>
    public void HidePause()
    {
        if (pausePanel == null)
            return;

        pausePanel.SetActive(false);
    }

    /// <summary>
    /// 显示胜利界面
    /// </summary>
    public void ShowVictory()
    {
        if (victoryPanel == null)
            return;

        victoryPanel.SetActive(true);
    }

    /// <summary>
    /// 关闭胜利界面
    /// </summary>
    public void HideVictory()
    {
        if (victoryPanel == null)
            return;

        victoryPanel.SetActive(false);
    }

    /// <summary>
    /// 显示失败界面
    /// </summary>
    /// <param name="score">最终分数</param>
    public void ShowGameOver(int score)
    {
        if (gameOverPanel == null)
            return;

        gameOverPanel.SetActive(true);

        if (gameOverScoreText != null)
        {
            gameOverScoreText.text = score.ToString();
        }
    }

    /// <summary>
    /// 关闭失败界面
    /// </summary>
    public void HideGameOver()
    {
        if (gameOverPanel == null)
            return;

        gameOverPanel.SetActive(false);
    }
}