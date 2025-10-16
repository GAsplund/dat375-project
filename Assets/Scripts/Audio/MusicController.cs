using System.Collections;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip[] backgroundMusicClips;
    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource ??= gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
    }

    private void Start()
    {
        StartCoroutine(PlayRandomMusicLoop());
    }

    private IEnumerator PlayRandomMusicLoop()
    {
        while (true)
        {
            if (backgroundMusicClips.Length == 0)
                yield break;

            AudioClip clip = backgroundMusicClips[Random.Range(0, backgroundMusicClips.Length)];
            audioSource.clip = clip;
            audioSource.Play();

            yield return new WaitForSeconds(clip.length);
        }
    }
}
