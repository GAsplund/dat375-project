using UnityEngine;
using TMPro;

public class BusinessUpgradeInteractable : MonoBehaviour
{
    public KeyCode interactKey = KeyCode.E;
    public TextMeshProUGUI interactText;

    private bool playerInRange = false;
    private int currentMoney = 0;

    void OnEnable() => MoneyManager.OnValueChange += UpdateMoney;
    void OnDisable() => MoneyManager.OnValueChange -= UpdateMoney;

    void UpdateMoney(int money) => currentMoney = money;

    void Start()
    {
        if (interactText != null)
            interactText.enabled = false;
    }

    void Update()
    {
        if (!playerInRange || !Input.GetKeyDown(interactKey))
            return;

        var manager = UpgradeManager.Instance;
        if (manager == null)
            return;

        //  Check if we have reached the maximum level
        if (manager.CurrentStep >= manager.MaxStep)
        {
            Debug.Log("Max Level reached!");
            return;
        }

        int upgradeCost = manager.GetUpgradeCost();

        // Check if the player can afford the upgrade
        if (currentMoney >= upgradeCost)
        {
            MoneyManager.Subtract(upgradeCost);
            manager.Upgrade();

            Debug.Log($"Upgraded! Cost: {upgradeCost}");

            UpdateInteractText(); // Update UI text after upgrading
        }
        else
        {
            Debug.Log($"Not enough money! Cost: {upgradeCost}, You have: {currentMoney}");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            UpdateInteractText();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactText != null)
                interactText.enabled = false;
        }
    }

    //Updates the interaction text shown when the player is near the upgrade object.
    private void UpdateInteractText()
    {
        if (interactText == null)
            return;

        var manager = UpgradeManager.Instance;
        if (manager == null)
            return;

        interactText.enabled = true;

        if (manager.CurrentStep >= manager.MaxStep)
            interactText.text = "Max Level";
        else
            interactText.text = $"Press {interactKey} to Upgrade ({manager.GetUpgradeCost()}$)";
    }
}
