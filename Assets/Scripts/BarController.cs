using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InfluenceBar : MonoBehaviour
{
    [Tooltip("Reference to the Unity UI Slider component.")]
    [SerializeField] private Slider influenceSlider;

    // Automatically finds the Slider if it hasn't been assigned in the Inspector.
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
    }

    void OnEnable()
    {
        // IMPORTANT: Subscribe to the data source event when this component becomes active.
        InfluenceStats.OnValueChange += UpdateBar;
    }

    void OnDisable()
    {
        // CRITICAL: Unsubscribe when the component is disabled to prevent memory leaks!
        InfluenceStats.OnValueChange -= UpdateBar;
    }

    /// <summary>
    /// This method is the event handler. It executes every time PlayerStats.OnValueChange fires.
    /// </summary>
    /// <param name="current">The new current value from InfluenceStats.</param>
    /// <param name="max">The max value from InfluenceStats.</param>
    private void UpdateBar(float current, float max)
    {
        if (influenceSlider != null)
        {
            // Set the Slider's max value (useful if max stats can change)
            influenceSlider.maxValue = max;

            // Set the Slider's current value to visually reflect the data
            influenceSlider.value = current;
        }
    }
}
