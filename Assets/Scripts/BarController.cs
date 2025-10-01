using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InfluenceBar : MonoBehaviour
{
    [Tooltip("Reference to the Unity UI Slider component.")]
    [SerializeField] private Slider influenceSlider;

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
    }
}
