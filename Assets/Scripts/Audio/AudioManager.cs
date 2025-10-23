using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton AudioManager to handle playing sound effects
/// Must not be instantiated more than once per scene
/// </summary>
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] AudioSource effectSource;
    [SerializeField] AudioSource sfxSource;

    private Dictionary<string, float> lastPlayTimes = new Dictionary<string, float>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayOneShotEffect(AudioClip clip, float volume = 1f)
    {
        if (clip == null || effectSource == null) return;
        effectSource.PlayOneShot(clip, volume);
    }
    
    public void PlayOneShotSfx(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;
        sfxSource.PlayOneShot(clip, volume);
    }

    public void PlayRandomOneShotEffect(AudioClip[] clips, float volume = 1f)
    {
        if (clips.Length == 0) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clip == null) return;
        PlayOneShotEffect(clip, volume);
    }

    public void PlayRandomOneShotSfx(AudioClip[] clips, float volume = 1f)
    {
        if (clips.Length == 0) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clip == null) return;
        PlayOneShotSfx(clip, volume);
    }

    public void PlayRandomOneShotEffectDebounced(AudioClip[] clips, string key, float cooldown = 0.5f, float volume = 1f)
    {
        if (clips == null || clips.Length == 0 || effectSource == null) return;
        if (string.IsNullOrEmpty(key)) key = "default";

        float last;
        lastPlayTimes.TryGetValue(key, out last);

        if (Time.time - last < cooldown) return;

        lastPlayTimes[key] = Time.time;
        PlayRandomOneShotEffect(clips, volume);
    }
}
