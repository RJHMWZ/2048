using TMPro;
using UnityEngine;

/// <summary>
/// 2048游戏HUD
/// </summary>
public class GameHUD : MonoBehaviour
{
    [Header("分数")]

    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private TMP_Text bestScoreText;

    /// <summary>
    /// 更新当前分数和最高分
    /// </summary>
    public void SetScore(
        int score,
        int bestScore)
    {
        if (scoreText != null)
        {
            scoreText.text =
                score.ToString();
        }

        if (bestScoreText != null)
        {
            bestScoreText.text =
                bestScore.ToString();
        }
    }
}