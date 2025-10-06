using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("Audio Sources")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource effectSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip walkSound;

    private void Awake()
    {
        instance ??= this;
    }
    
    public void PlayOneShot(AudioClip clip, Transform sourceTransform, float volume = 1f)
    {
        AudioSource source = Instantiate(effectSource, sourceTransform.position, Quaternion.identity);
        source.clip = clip;
        source.volume = volume;
        source.Play();
        Destroy(source.gameObject, source.clip.length);
    }
}
