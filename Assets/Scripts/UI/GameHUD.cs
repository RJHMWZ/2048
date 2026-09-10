using TMPro;
using UnityEngine;

/// <summary>
/// 显示当前分数和历史最高分。
/// </summary>
public sealed class GameHUD : MonoBehaviour
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text bestScoreText;

    public void SetScore(int score, int bestScore)
    {
        if (scoreText != null)
            scoreText.text = score.ToString();

        if (bestScoreText != null)
            bestScoreText.text = bestScore.ToString();
    }
}
