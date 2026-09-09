using TMPro;
using UnityEngine;

/// <summary>
/// 2048游戏HUD
/// </summary>
public class GameHUD : MonoBehaviour
{
    [SerializeField]
    private TMP_Text scoreText;

    /// <summary>
    /// 更新分数
    /// </summary>
    public void SetScore(int score)
    {
        scoreText.text = score.ToString();
    }
}