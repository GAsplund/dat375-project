using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InfluenceStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float currentValue = 0.5f;
    [SerializeField] private float maxValue = 1f;

    public static event Action<float, float> OnValueChange;

    public float MaxValue => maxValue;
    public float CurrentValue => currentValue;

    void Start()
    {
        OnValueChange?.Invoke(currentValue, maxValue);
    }

    public void ModifyValue(float amount)
    {
        // Calculate the new value, clamped between 0 and max value.
        currentValue = Mathf.Clamp(currentValue + amount, 0f, maxValue);

        // Notify all subscribed listeners (e.g., the HealthBarUI script) about the change.
        OnValueChange?.Invoke(currentValue, maxValue);

        Debug.Log($"Influence: Value changed to {currentValue}. Notifying UI.");
    }
}
