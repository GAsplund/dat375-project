using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private AudioClip upgradeSound;

    public static UpgradeManager Instance;


    public int baseUpgradeCost = 5;

    public int costMultiplier = 2;

    public GameObject[] floors;
    public GameObject[] stars;

    private static int savedStep = 0;
    public int CurrentStep => savedStep;
    public int MaxStep => floors.Length - 1;

    private void Awake()
    {
        Instance = this;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        TryPopulateArraysIfNeeded();
        UpdateVisuals();
        CanAffordUpgrades();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryPopulateArraysIfNeeded();
        UpdateVisuals();
        CanAffordUpgrades();
    }


    // Upgrades to the next floor and updates visuals.
    public void Upgrade()
    {
        if (savedStep < floors.Length - 1)
        {
            savedStep++;
            if (upgradeSound != null)
            {
                AudioManager.instance.PlayOneShotSfx(upgradeSound);
            }
            UpdateVisuals();
            CanAffordUpgrades();
        }
    }

    // Calculates the upgrade cost based on current level and multiplier.
    public int GetUpgradeCost()
    {
        return baseUpgradeCost * (int)Mathf.Pow(costMultiplier, savedStep);
    }

    // Activates the correct floor and star objects based on the current step.
    private void UpdateVisuals()
    {
        // Floors — only one active
        for (int i = 0; i < floors.Length; i++)
            floors[i]?.SetActive(i == savedStep);

        // Stars — only one active (not cumulative)
        for (int i = 0; i < stars.Length; i++)
            stars[i]?.SetActive(i == savedStep);
    }

    // Automatically finds floor and star objects if arrays are empty.
    private void TryPopulateArraysIfNeeded()
    {
        // Handle floors
        if (floors == null || floors.Length == 0)
        {
            var floorsParent = GameObject.Find("Floors");
            if (floorsParent != null)
                floors = floorsParent.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
        }

        // Handle stars (even if inactive)
        if (stars == null || stars.Length == 0)
        {
            var starsParent = GameObject.Find("Stars");
            if (starsParent != null)
                stars = starsParent.GetComponentsInChildren<Transform>(true)
                                   .Where(t => t != starsParent.transform)
                                   .Select(t => t.gameObject)
                                   .ToArray();
        }
    }

    private void CanAffordUpgrades()
    {
        if (MoneyManager.getCurrentMoney() > GetUpgradeCost() && savedStep < MaxStep)
        {
            FindObjectOfType<CashRegisterController>()?.CanPay();
        }
        else
        {
            FindObjectOfType<CashRegisterController>()?.CannotPay();
        }
    }
}




