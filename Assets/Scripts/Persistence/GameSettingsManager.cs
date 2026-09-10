using UnityEngine;

/// <summary>
/// 为UI提供音效设置入口，实际状态由音频管理器统一持有。
/// </summary>
public sealed class GameSettingsManager : MonoBehaviour
{
    public bool SoundEnabled => GameAudioManager.Instance.SoundEnabled;

    public void SetSoundEnabled(bool enabled)
    {
        GameAudioManager.Instance.SetSoundEnabled(enabled);
    }

    public void ToggleSound()
    {
        SetSoundEnabled(!SoundEnabled);
    }
}
