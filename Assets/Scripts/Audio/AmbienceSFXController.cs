using System.Collections;
using UnityEngine;

public class AmbienceController : MonoBehaviour
{
    [SerializeField] private AudioClip[] backgroundAmbienceClips;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float minDelay = 5f;
    [SerializeField] private float maxDelay = 15f;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource ??= gameObject.AddComponent<AudioSource>();
        audioSource.loop = false;
    }

    private void Start()
    {
        StartCoroutine(PlayRandomAmbienceSound());
    }

    private IEnumerator PlayRandomAmbienceSound()
    {
        while (true)
        {
            if (backgroundAmbienceClips.Length == 0)
                yield break;

            // Wait for random amount of time
            float delay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(delay);

            // Play a random ambience clip
            AudioClip clip = backgroundAmbienceClips[Random.Range(0, backgroundAmbienceClips.Length)];
            audioSource.clip = clip;
            audioSource.Play();

            yield return new WaitForSeconds(clip.length);
        }
    }
}
