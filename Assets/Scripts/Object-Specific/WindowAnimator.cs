using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WindowAnimator : MonoBehaviour
{
    [Tooltip("Animator parameter name for the WalkBy trigger")]
    public string walkByParam = "RandomWalkBy";

    [Tooltip("Minimum seconds between possible WalkBy triggers")]
    public float minInterval = 10f;

    [Tooltip("Maximum seconds between possible WalkBy triggers")]
    public float maxInterval = 60f;


    [Tooltip("Enable random triggering (useful to disable in editor/playtests)")]
    public bool enableRandom = true;

    Animator animator;
    Coroutine runningRoutine;

    void Awake()
    {
        animator = GetComponent<Animator>();
        minInterval = Mathf.Max(0f, minInterval);
        maxInterval = Mathf.Max(minInterval, maxInterval);
    }

    void OnEnable()
    {
        if (enableRandom && runningRoutine == null)
            runningRoutine = StartCoroutine(RandomWalkByLoop());
    }

    void OnDisable()
    {
        if (runningRoutine != null)
        {
            StopCoroutine(runningRoutine);
            runningRoutine = null;
        }
    }

    IEnumerator RandomWalkByLoop()
    {
        // small initial delay so scene can settle
        yield return new WaitForSeconds(Random.Range(0f, Mathf.Min(2f, minInterval)));

        while (true)
        {
            float wait = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(wait);

            if (!enableRandom || animator == null || !gameObject.activeInHierarchy)
                continue;

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
        // trigger immediately
        animator.SetTrigger(walkByParam);
    }
}
