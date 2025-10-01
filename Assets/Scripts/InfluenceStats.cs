using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InfluenceStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float currentValue = 0.5f;
    [SerializeField] private float maxValue = 1f;

    // A static Action is the event that UI scripts will subscribe to.
    // It passes the new current and max values.
    public static event Action<float, float> OnValueChange;

    // Public getters for read-only access
    public float MaxValue => maxValue;
    public float CurrentValue => currentValue;

    void Start()
    {
        // Set the initial value on Start so the UI is correctly initialized.
        // The '?' checks if anyone is subscribed before invoking the event.
        OnValueChange?.Invoke(currentValue, maxValue);
    }

    /// <summary>
    /// This is the public method *other* scripts (like enemies, items, or controllers)
    /// should use to safely update the player's value.
    /// </summary>
    /// <param name="amount">The amount to change the value by (positive or negative).</param>
    public void ModifyValue(float amount)
    {
        // Calculate the new value, clamped between 0 and max value.
        currentValue = Mathf.Clamp(currentValue + amount, 0f, maxValue);

        // Notify all subscribed listeners (e.g., the HealthBarUI script) about the change.
        OnValueChange?.Invoke(currentValue, maxValue);

        Debug.Log($"Influence: Value changed to {currentValue}. Notifying UI.");
    }
}
