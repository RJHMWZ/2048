using UnityEngine;

/// <summary>
/// 在音频资源缺失时创建轻量级备用音效。
/// </summary>
public static class ProceduralAudioFactory
{
    private const int SampleRate = 44100;

    public static AudioClip CreateSweep(
        string clipName,
        float startFrequency,
        float endFrequency,
        float duration,
        float volume)
    {
        int sampleCount = Mathf.Max(1, Mathf.RoundToInt(SampleRate * duration));
        float[] samples = new float[sampleCount];
        float phase = 0f;

        for (int i = 0; i < sampleCount; i++)
        {
            float progress = i / (float)sampleCount;
            float frequency = Mathf.Lerp(startFrequency, endFrequency, progress);
            phase += 2f * Mathf.PI * frequency / SampleRate;
            samples[i] = Mathf.Sin(phase) * volume * GetEnvelope(progress);
        }

        return CreateClip(clipName, samples);
    }

    public static AudioClip CreateToneSequence(
        string clipName,
        float[] frequencies,
        float toneDuration,
        float volume)
    {
        int samplesPerTone = Mathf.Max(1, Mathf.RoundToInt(SampleRate * toneDuration));
        float[] samples = new float[samplesPerTone * frequencies.Length];
        float phase = 0f;

        for (int toneIndex = 0; toneIndex < frequencies.Length; toneIndex++)
        {
            for (int i = 0; i < samplesPerTone; i++)
            {
                float progress = i / (float)samplesPerTone;
                phase += 2f * Mathf.PI * frequencies[toneIndex] / SampleRate;
                int sampleIndex = toneIndex * samplesPerTone + i;
                samples[sampleIndex] = Mathf.Sin(phase) * volume * GetEnvelope(progress);
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
        AudioClip clip = AudioClip.Create(clipName, samples.Length, 1, SampleRate, false);
        clip.SetData(samples, 0);
        return clip;
    }
}
