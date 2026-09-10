using UnityEngine;

/// <summary>
/// 2048本地数据保存入口。
/// </summary>
public static class GameSaveManager
{
    private const string BestScoreKey = "2048_BestScore";
    private const string SoundEnabledKey = "2048_SoundEnabled";

    public static int GetBestScore()
    {
        return PlayerPrefs.GetInt(BestScoreKey, 0);
    }

    public static void SaveBestScore(int score)
    {
        PlayerPrefs.SetInt(BestScoreKey, Mathf.Max(0, score));
        PlayerPrefs.Save();
    }

    public static bool GetSoundEnabled()
    {
        return PlayerPrefs.GetInt(SoundEnabledKey, 1) == 1;
    }

    public static void SaveSoundEnabled(bool enabled)
    {
        PlayerPrefs.SetInt(SoundEnabledKey, enabled ? 1 : 0);
        PlayerPrefs.Save();
    }

    public static void ClearBestScore()
    {
        PlayerPrefs.DeleteKey(BestScoreKey);
        PlayerPrefs.Save();
    }
}
