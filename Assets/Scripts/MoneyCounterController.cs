using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is responsible for controlling the money counter UI in the game.
/// It handles the display and receives updates to the player's money.
/// </summary>
public class MoneyCounterController : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI moneyText;

    void Awake()
    {
        if (moneyText == null)
        {
            moneyText = GetComponent<TMPro.TextMeshProUGUI>();
            if (moneyText == null)
            {
                Debug.LogError("Money Counter requires a TextMeshProUGUI component attached or assigned.");
            }
            else
            {
                UpdateMoneyDisplay(0);
            }
        }
    }

    void OnEnable()
    {
        MoneyManager.OnValueChange += UpdateMoneyDisplay;
    }

    void OnDisable()
    {
        MoneyManager.OnValueChange -= UpdateMoneyDisplay;
    }

    private void UpdateMoneyDisplay(int currentMoney)
    {
        moneyText.text = $"Money: {currentMoney}";
    }
}
