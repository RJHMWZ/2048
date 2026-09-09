using UnityEngine;

/// <summary>
/// 2048本地数据保存
/// </summary>
public static class GameSaveManager
{
    private const string BestScoreKey =
        "2048_BestScore";

    private const string SoundEnabledKey =
        "2048_SoundEnabled";

    /// <summary>
    /// 获取历史最高分
    /// </summary>
    public static int GetBestScore()
    {
        return PlayerPrefs.GetInt(
            BestScoreKey,
            0
        );
    }

    /// <summary>
    /// 保存历史最高分
    /// </summary>
    public static void SaveBestScore(
        int score)
    {
        PlayerPrefs.SetInt(
            BestScoreKey,
            score
        );

        PlayerPrefs.Save();
    }

    /// <summary>
    /// 获取音效开关
    /// </summary>
    public static bool GetSoundEnabled()
    {
        return PlayerPrefs.GetInt(
            SoundEnabledKey,
            1
        ) == 1;
    }

    /// <summary>
    /// 保存音效开关
    /// </summary>
    public static void SaveSoundEnabled(
        bool enabled)
    {
        PlayerPrefs.SetInt(
            SoundEnabledKey,
            enabled ? 1 : 0
        );

        PlayerPrefs.Save();
    }

    /// <summary>
    /// 清除最高分
    /// </summary>
    public static void ClearBestScore()
    {
        PlayerPrefs.DeleteKey(
            BestScoreKey
        );

        PlayerPrefs.Save();
    }
}