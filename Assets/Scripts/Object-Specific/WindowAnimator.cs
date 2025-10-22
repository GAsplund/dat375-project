using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Animator))]
public class WindowAnimator : MonoBehaviour
{
    [Header("Random WalkBy Animation Settings")]
    [Tooltip("Animator parameter name for the WalkBy trigger")]
    public string walkByParam = "RandomWalkBy";
    [Tooltip("Minimum seconds between possible WalkBy triggers")]
    public float minInterval = 10f;
    [Tooltip("Maximum seconds between possible WalkBy triggers")]
    public float maxInterval = 60f;
    [Tooltip("Enable random triggering (useful to disable in editor/playtests)")]
    public bool enableRandom = true;

    [Header("Shooting Animation Settings")]
    [Tooltip("Animator parameter name for the shot trigger to play on scene entry")]
    public string shotTrigger = "Shot";
    [Tooltip("Animator layer index where the shot state exists (used to detect if shot is playing)")]
    public int shotStateLayer = 0;
    [Tooltip("Animator state name for the shot animation (used to prevent random triggers while shot plays)")]
    public string shotStateName = "Shot";
    [Tooltip("Sound to play when the shot animation is triggered")]
    public AudioClip shotSound;
    [Tooltip("Delay in seconds before playing the shot sound after the shot is triggered")]
    public float shotSoundDelay = 0f;

    Animator animator;
    Coroutine runningRoutine;
    Coroutine startupRoutine;

    void Awake()
    {
        animator = GetComponent<Animator>();
        minInterval = Mathf.Max(0f, minInterval);
        maxInterval = Mathf.Max(minInterval, maxInterval);
    }

    void OnEnable()
    {
        ReputationManager.OnHeartLost += OnHeartLost;

        if (enableRandom && runningRoutine == null)
        {
            runningRoutine = StartCoroutine(RandomWalkByLoop());
        }
    }

    void OnDisable()
    {
        ReputationManager.OnHeartLost -= OnHeartLost;

        if (runningRoutine != null)
        {
            StopCoroutine(runningRoutine);
            runningRoutine = null;
        }
        if (startupRoutine != null)
        {
            StopCoroutine(startupRoutine);
            startupRoutine = null;
        }
    }

    void OnHeartLost(string side)
    {
        TriggerShotOnce();
    }

    IEnumerator RandomWalkByLoop()
    {
        // small initial delay so scene can settle
        yield return new WaitForSeconds(UnityEngine.Random.Range(0f, Mathf.Min(2f, minInterval)));

        while (true)
        {
            float wait = UnityEngine.Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(wait);

            if (!enableRandom || animator == null || !gameObject.activeInHierarchy)
                continue;

            if (!string.IsNullOrEmpty(shotStateName) && animator.layerCount > 0)
            {
                int layer = Mathf.Clamp(shotStateLayer, 0, animator.layerCount - 1);
                // wait while shot state is active or transitioning
                while (animator != null && gameObject.activeInHierarchy)
                {
                    var state = animator.GetCurrentAnimatorStateInfo(layer);
                    if (!state.IsName(shotStateName) && !animator.IsInTransition(layer))
                        break;
                    yield return null;
                }
            }

            animator.SetTrigger(walkByParam);
        }
    }

    public void TriggerWalkByOnce()
    {
        if (animator == null)
        {
            Debug.LogWarning("WindowAnimator: Cannot TriggerWalkByOnce, no Animator found.");
            return;
        }
        animator.SetTrigger(walkByParam);
    }

    [ContextMenu("Trigger Shot (Context)")]
    public void TriggerShotOnce()
    {
        if (animator == null)
        {
            Debug.LogWarning("WindowAnimator: Cannot TriggerShotOnce, no Animator found.");
            return;
        }
        animator.SetTrigger(shotTrigger);

        // Play configured shot sound after optional delay using AudioManager
        if (shotSound != null && AudioManager.instance != null)
        {
            // Start a coroutine to play the shot sound after the configured delay.
            StartCoroutine(PlayShotWithDelay(shotSoundDelay));
        }
    }

    IEnumerator PlayShotWithDelay(float delay)
    {
        if (delay > 0f)
            yield return new WaitForSeconds(delay);

        if (shotSound == null) yield break;
        if (AudioManager.instance == null) yield break;

        AudioManager.instance.PlayOneShotEffect(shotSound);
    }
}
