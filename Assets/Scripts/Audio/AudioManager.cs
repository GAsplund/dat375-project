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

    public void PlayOneShotEffect(AudioClip clip, Transform sourceTransform, float volume = 1f)
    {
        if (clip == null||effectSource==null) return;
        effectSource.PlayOneShot(clip, volume);
    }

    public void PlayRandomOneShotEffect(AudioClip[] clips, Transform sourceTransform, float volume = 1f)
    {
        if (clips.Length == 0) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clip == null) return;
        PlayOneShotEffect(clip, sourceTransform, volume);
    }
}
