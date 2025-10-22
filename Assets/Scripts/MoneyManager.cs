using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Unity.Burst.CompilerServices;

/// <summary>
/// This class is responsible for controlling the money counter in the game.
/// It acts as a singleton and provides methods to add or subtract money.
/// </summary>
public class MoneyManager : MonoBehaviour
{
    public static event Action<int> OnValueChange;
    private static MoneyManager Instance;

    public int CurrentMoney { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Subscribe to scene load events to update money display
        // if it is present in the new scene
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OnValueChange?.Invoke(CurrentMoney);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public static void Add(int amount)
    {
        if (Instance == null)
        {
            throw new NotSupportedException("MoneyManager instance does not exist in the scene. Cannot add money.");
        }

        Instance.AddMoney(amount);

    }

    public static void Subtract(int amount)
    {
        if (Instance == null)
        {
            throw new NotSupportedException("MoneyManager instance does not exist in the scene. Cannot subtract money.");
        }

        Instance.SubtractMoney(amount);
    }

    public static int getCurrentMoney()
    {
        if (Instance == null)
        {
            throw new NotSupportedException("MoneyManager instance does not exist in the scene. Cannot get current money.");
        }

        return Instance.CurrentMoney;
    }

    /** Instance Methods **/

    private void AddMoney(int amount)
    {
        CurrentMoney += amount;
        OnValueChange?.Invoke(CurrentMoney);
    }

    private void SubtractMoney(int amount)
    {
        CurrentMoney -= amount;
        OnValueChange?.Invoke(CurrentMoney);
    }
}
