using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class InfluenceBar : MonoBehaviour
{
    [Tooltip("Reference to the Unity UI Slider component.")]
    [SerializeField] private Slider influenceSlider;

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Coroutine fadeRoutine;

    [Header("Bar Display Settings")]
    [Tooltip("Duration for fade in/out animations.")]
    [SerializeField] private float fadeDuration = 0.3f;
    [Tooltip("Delay before starting to fade out.")]
    [SerializeField] private float fadeOutDelay = 2.0f;

    void Awake()
    {
        if (influenceSlider == null)
        {
            influenceSlider = GetComponent<Slider>();
            if (influenceSlider == null)
            {
                Debug.LogError("HealthBarUI requires a Slider component attached or assigned.");
            }
        }

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 0f; // Start hidden
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }

    void OnEnable()
    {
        InfluenceStats.OnValueChange += UpdateBar;
    }

    void OnDisable()
    {
        InfluenceStats.OnValueChange -= UpdateBar;
    }

    private void UpdateBar(float current, float max)
    {
        if (influenceSlider != null)
        {
            // Set the Slider's max value (useful if max stats can change)
            influenceSlider.maxValue = max;

            // Set the Slider's current value to visually reflect the data
            influenceSlider.value = current;
        }

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = StartCoroutine(FadeBarRoutine());
    }

    private IEnumerator FadeBarRoutine()
    {
        float startAlpha = canvasGroup.alpha;
        float timeElapsed = 0f;
        while (canvasGroup.alpha < 1f)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, timeElapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 1f; // Ensure it's fully visible

        yield return new WaitForSeconds(fadeOutDelay);

        timeElapsed = 0f;
        while (canvasGroup.alpha > 0f)
        {
            timeElapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timeElapsed / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = 0f; // Ensure it's fully hidden

        fadeRoutine = null;
    }
}
