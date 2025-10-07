using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource effectSource;

    private void Awake()
    {
        instance ??= this;
    }

    public void PlayOneShotEffect(AudioClip clip, Transform sourceTransform, float volume = 1f)
    {
        AudioSource source = Instantiate(effectSource, sourceTransform.position, Quaternion.identity);
        source.clip = clip;
        source.volume = volume;
        source.Play();
        Destroy(source.gameObject, source.clip.length);
    }
    
    public void PlayRandomOneShotEffect(AudioClip[] clips, Transform sourceTransform, float volume = 1f)
    {
        if (clips.Length == 0) return;
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        PlayOneShotEffect(clip, sourceTransform, volume);
    }
}
