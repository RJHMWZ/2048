using UnityEngine;

/// <summary>
/// 2048游戏设置
/// </summary>
public class GameSettingsManager : MonoBehaviour
{
    /// <summary>
    /// 音效是否开启
    /// </summary>
    private bool soundEnabled;

    public bool SoundEnabled =>
        soundEnabled;

    private void Awake()
    {
        soundEnabled =
            GameSaveManager
                .GetSoundEnabled();
    }

    /// <summary>
    /// 设置音效状态
    /// </summary>
    public void SetSoundEnabled(
        bool enabled)
    {
        soundEnabled = enabled;

        GameSaveManager
            .SaveSoundEnabled(
                soundEnabled
            );
    }

    /// <summary>
    /// 切换音效开关
    /// </summary>
    public void ToggleSound()
    {
        SetSoundEnabled(
            !soundEnabled
        );
    }
}