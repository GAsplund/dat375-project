using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This class is responsible for controlling the money counter in the game.
/// It handles the display and receives updates to the player's money.
/// </summary>
public class MoneyManager : MonoBehaviour
{
    public static event Action<int> OnValueChange;

    private int currentMoney = 50;

    public int CurrentMoney => currentMoney;

    // Start is called before the first frame update
    void Start()
    {
        OnValueChange?.Invoke(currentMoney);
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        OnValueChange?.Invoke(currentMoney);
    }

    public void SubtractMoney(int amount)
    {
        currentMoney -= amount;
        OnValueChange?.Invoke(currentMoney);
    }
}
