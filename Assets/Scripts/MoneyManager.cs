using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

/// <summary>
/// This class is responsible for controlling the money counter in the game.
/// It acts as a singleton and provides methods to add or subtract money.
/// </summary>
public class MoneyManager : MonoBehaviour
{
    public static event Action<int> OnValueChange;
    private static MoneyManager Instance;

    private int currentMoney = 0;

    public int CurrentMoney => currentMoney;

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
        OnValueChange?.Invoke(currentMoney);
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

    /** Instance Methods **/

    private void AddMoney(int amount)
    {
        currentMoney += amount;
        OnValueChange?.Invoke(currentMoney);
    }

    private void SubtractMoney(int amount)
    {
        currentMoney -= amount;
        OnValueChange?.Invoke(currentMoney);
    }
}
