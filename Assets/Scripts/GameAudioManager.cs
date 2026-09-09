using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// 2048 game sound effects.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public sealed class GameAudioManager : MonoBehaviour
{
    private const int SampleRate = 44100;

    private static GameAudioManager instance;

    private AudioSource audioSource;
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

        if (instance != null)
            return;

        GameObject audioObject =
            new GameObject(nameof(GameAudioManager));

        instance = audioObject.AddComponent<GameAudioManager>();
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

        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
        audioSource.volume = 0.8f;

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.playOnAwake = false;
        musicSource.loop = true;
        musicSource.spatialBlend = 0f;
        musicSource.volume = 0.35f;

        soundEnabled = GameSaveManager.GetSoundEnabled();
        audioSource.mute = !soundEnabled;
        musicSource.mute = !soundEnabled;

        LoadClips();

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
        soundEnabled = enabled;

        if (audioSource != null)
        {
            audioSource.mute = !enabled;
        }

        if (musicSource != null)
        {
            musicSource.mute = !enabled;
        }

        GameSaveManager.SaveSoundEnabled(enabled);
    }

    public void ToggleSound()
    {
        SetSoundEnabled(!soundEnabled);
    }

    public void PlayMove()
    {
        Play(moveClip);
    }

    public void PlayMerge()
    {
        Play(mergeClip);
    }

    public void PlaySpawn()
    {
        Play(spawnClip);
    }

    public void PlayVictory()
    {
        Play(victoryClip);
    }

    public void PlayGameOver()
    {
        Play(gameOverClip);
    }

    public void PlayButtonClick()
    {
        Play(buttonClip);
    }

    private void Play(AudioClip clip)
    {
        if (!soundEnabled || audioSource == null || clip == null)
            return;

        audioSource.PlayOneShot(clip);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindButtonSounds();
    }

    private void BindButtonSounds()
    {
        Button[] buttons = FindObjectsOfType<Button>(true);

        for (int i = 0; i < buttons.Length; i++)
        {
            if (buttons[i].GetComponent<ButtonSoundFeedback>() == null)
            {
                buttons[i].gameObject.AddComponent<ButtonSoundFeedback>();
            }
        }
    }

    private void LoadClips()
    {
        musicClip = LoadClip(
            "Audio/背景音乐",
            "Audio/Music"
        );

        moveClip = LoadClip("Audio/Move") ??
                   CreateSweep("Move", 240f, 170f, 0.07f, 0.16f);

        mergeClip = LoadClip(
                        "Audio/消除",
                        "Audio/Merge"
                    ) ??
                    CreateToneSequence(
                        "Merge",
                        new[] { 330f, 520f },
                        0.065f,
                        0.2f
                    );

        spawnClip = LoadClip("Audio/Spawn") ??
                    CreateSweep("Spawn", 520f, 780f, 0.09f, 0.14f);

        victoryClip = LoadClip(
                          "Audio/胜利",
                          "Audio/Victory"
                      ) ??
                      CreateToneSequence(
                          "Victory",
                          new[] { 523.25f, 659.25f, 783.99f, 1046.5f },
                          0.13f,
                          0.18f
                      );

        gameOverClip = LoadClip(
                           "Audio/失败",
                           "Audio/GameOver"
                       ) ??
                       CreateToneSequence(
                           "GameOver",
                           new[] { 392f, 329.63f, 261.63f, 196f },
                           0.14f,
                           0.17f
                       );

        buttonClip = LoadClip(
                         "Audio/按钮按下",
                         "Audio/Button"
                     ) ??
                     CreateSweep("Button", 720f, 620f, 0.045f, 0.12f);
    }

    private void PlayBackgroundMusic()
    {
        if (musicSource == null || musicClip == null)
            return;

        musicSource.clip = musicClip;
        musicSource.Play();
    }

    private static AudioClip LoadClip(params string[] resourcePaths)
    {
        for (int i = 0; i < resourcePaths.Length; i++)
        {
            AudioClip clip =
                Resources.Load<AudioClip>(resourcePaths[i]);

            if (clip != null)
            {
                return clip;
            }
        }

        return null;
    }

    private static AudioClip CreateSweep(
        string clipName,
        float startFrequency,
        float endFrequency,
        float duration,
        float volume)
    {
        int sampleCount =
            Mathf.Max(1, Mathf.RoundToInt(SampleRate * duration));

        float[] samples = new float[sampleCount];
        float phase = 0f;

        for (int i = 0; i < sampleCount; i++)
        {
            float progress = i / (float)sampleCount;
            float frequency =
                Mathf.Lerp(startFrequency, endFrequency, progress);

            phase += 2f * Mathf.PI * frequency / SampleRate;

            samples[i] =
                Mathf.Sin(phase) * volume * GetEnvelope(progress);
        }

        return CreateClip(clipName, samples);
    }

    private static AudioClip CreateToneSequence(
        string clipName,
        float[] frequencies,
        float toneDuration,
        float volume)
    {
        int samplesPerTone =
            Mathf.Max(1, Mathf.RoundToInt(SampleRate * toneDuration));

        float[] samples =
            new float[samplesPerTone * frequencies.Length];

        float phase = 0f;

        for (int toneIndex = 0;
             toneIndex < frequencies.Length;
             toneIndex++)
        {
            for (int i = 0; i < samplesPerTone; i++)
            {
                float progress = i / (float)samplesPerTone;

                phase +=
                    2f * Mathf.PI * frequencies[toneIndex] / SampleRate;

                int sampleIndex =
                    toneIndex * samplesPerTone + i;

                samples[sampleIndex] =
                    Mathf.Sin(phase) * volume * GetEnvelope(progress);
            }
        }

        return CreateClip(clipName, samples);
    }

    private static float GetEnvelope(float progress)
    {
        float attack = Mathf.Clamp01(progress / 0.08f);
        float release = Mathf.Clamp01((1f - progress) / 0.25f);

        return attack * release;
    }

    private static AudioClip CreateClip(string clipName, float[] samples)
    {
        AudioClip clip = AudioClip.Create(
            clipName,
            samples.Length,
            1,
            SampleRate,
            false
        );

        clip.SetData(samples, 0);
        return clip;
    }
}
