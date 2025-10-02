using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InfluenceStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int currentValue = 50;
    [SerializeField] private int maxValue = 100;

    public static event Action<int, int> OnValueChange;

    public int MaxValue => maxValue;
    public int CurrentValue => currentValue;

    public enum InfluenceDirection
    {
        Left = -1,
        Right = 1
    }

    void Start()
    {
        OnValueChange?.Invoke(currentValue, maxValue);
    }

    /// <summary>
    /// Add influence to the current value based on the direction.
    /// </summary>
    /// <param name="addInfluence">The amount of influence points to add, must be a value between 0-100, representing total percentage-point change.</param>
    /// <param name="direction">The direction to adjust influence direction in.</param>
    public void AddInfluence(int addInfluence, InfluenceDirection direction)
    {
        currentValue += addInfluence * (int)direction;
        currentValue = Math.Clamp(currentValue, 0, maxValue);
        OnValueChange?.Invoke(currentValue, maxValue);
    }
}
