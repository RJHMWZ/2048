using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 全局音频入口，负责加载资源、播放音乐和绑定按钮反馈。
/// </summary>
[RequireComponent(typeof(AudioSource))]
public sealed class GameAudioManager : MonoBehaviour
{
    private static GameAudioManager instance;

    private AudioSource soundSource;
    private AudioSource musicSource;
    private AudioClip musicClip;
    private AudioClip moveClip;
    private AudioClip mergeClip;
    private AudioClip spawnClip;
    private AudioClip victoryClip;
    private AudioClip gameOverClip;
    private AudioClip buttonClip;
    private bool soundEnabled;

    public static GameAudioManager Instance
    {
        get
        {
            EnsureInstance();
            return instance;
        }
    }

    public bool SoundEnabled => soundEnabled;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Bootstrap()
    {
        EnsureInstance();
    }

    private static void EnsureInstance()
    {
        if (instance != null)
            return;

        instance = FindObjectOfType<GameAudioManager>();

        if (instance == null)
            instance = new GameObject(nameof(GameAudioManager)).AddComponent<GameAudioManager>();
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        ConfigureSources();
        LoadClips();
        SetSoundEnabled(GameSaveManager.GetSoundEnabled(), false);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayBackgroundMusic();
        BindButtonSounds();
    }

    private void OnDestroy()
    {
        if (instance != this)
            return;

        SceneManager.sceneLoaded -= OnSceneLoaded;
        instance = null;
    }

    public void SetSoundEnabled(bool enabled)
    {
        SetSoundEnabled(enabled, true);
    }

    public void ToggleSound()
    {
        SetSoundEnabled(!soundEnabled);
    }

    public void PlayMove() => Play(moveClip);
    public void PlayMerge() => Play(mergeClip);
    public void PlaySpawn() => Play(spawnClip);
    public void PlayVictory() => Play(victoryClip);
    public void PlayGameOver() => Play(gameOverClip);
    public void PlayButtonClick() => Play(buttonClip);

    private void ConfigureSources()
    {
        AudioSource[] sources = GetComponents<AudioSource>();
        soundSource = sources[0];
        musicSource = sources.Length > 1 ? sources[1] : gameObject.AddComponent<AudioSource>();

        ConfigureSource(soundSource, 0.8f, false);
        ConfigureSource(musicSource, 0.35f, true);
    }

    private static void ConfigureSource(AudioSource source, float volume, bool loop)
    {
        source.playOnAwake = false;
        source.spatialBlend = 0f;
        source.volume = volume;
        source.loop = loop;
    }

    private void SetSoundEnabled(bool enabled, bool persist)
    {
        soundEnabled = enabled;
        soundSource.mute = !enabled;
        musicSource.mute = !enabled;

        if (persist)
            GameSaveManager.SaveSoundEnabled(enabled);
    }

    private void Play(AudioClip clip)
    {
        if (soundEnabled && soundSource != null && clip != null)
            soundSource.PlayOneShot(clip);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindButtonSounds();
    }

    private static void BindButtonSounds()
    {
        Button[] buttons = FindObjectsOfType<Button>(true);

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetComponent<ButtonSoundFeedback>() == null)
                buttons[i].gameObject.AddComponent<ButtonSoundFeedback>();
        }
    }

    private void LoadClips()
    {
        musicClip = LoadClip("Audio/背景音乐", "Audio/Music");
        moveClip = LoadClip("Audio/Move") ??
                   ProceduralAudioFactory.CreateSweep("Move", 240f, 170f, 0.07f, 0.16f);
        mergeClip = LoadClip("Audio/消除", "Audio/Merge") ??
                    ProceduralAudioFactory.CreateToneSequence(
                        "Merge", new[] { 330f, 520f }, 0.065f, 0.2f);
        spawnClip = LoadClip("Audio/Spawn") ??
                    ProceduralAudioFactory.CreateSweep("Spawn", 520f, 780f, 0.09f, 0.14f);
        victoryClip = LoadClip("Audio/胜利", "Audio/Victory") ??
                      ProceduralAudioFactory.CreateToneSequence(
                          "Victory", new[] { 523.25f, 659.25f, 783.99f, 1046.5f }, 0.13f, 0.18f);
        gameOverClip = LoadClip("Audio/失败", "Audio/GameOver") ??
                       ProceduralAudioFactory.CreateToneSequence(
                           "GameOver", new[] { 392f, 329.63f, 261.63f, 196f }, 0.14f, 0.17f);
        buttonClip = LoadClip("Audio/按钮按下", "Audio/Button") ??
                     ProceduralAudioFactory.CreateSweep("Button", 720f, 620f, 0.045f, 0.12f);
    }

    private void PlayBackgroundMusic()
    {
        if (musicClip == null)
            return;

        musicSource.clip = musicClip;
        musicSource.Play();
    }

    private static AudioClip LoadClip(params string[] resourcePaths)
    {
        for (int i = 0; i < resourcePaths.Length; i++)
        {
            AudioClip clip = Resources.Load<AudioClip>(resourcePaths[i]);

            if (clip != null)
                return clip;
        }

        return null;
    }
}
