using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public int baseUpgradeCost = 5;
    public int costMultiplier = 2;

    public GameObject[] floors;
    public GameObject[] stars;

    // Keeps the player's upgrade progress even after scene reloads
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
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TryPopulateArraysIfNeeded();
        UpdateVisuals();
    }


    // Upgrades to the next floor and updates visuals.
    public void Upgrade()
    {
        if (savedStep < floors.Length - 1)
        {
            savedStep++;
            UpdateVisuals();
        }
    }

    // Calculates the upgrade cost based on current level and multiplier.
    public int GetUpgradeCost()
    {
        return baseUpgradeCost * (int)Mathf.Pow(costMultiplier, savedStep);
    }

    //Activates the correct floor and star objects based on the current step.
    private void UpdateVisuals()
    {
        for (int i = 0; i < floors.Length; i++)
            floors[i]?.SetActive(i == savedStep);

        for (int i = 0; i < stars.Length; i++)
            stars[i]?.SetActive(i <= savedStep);
    }

 
    //Automatically fills in floor and star references if they’re missing.
    private void TryPopulateArraysIfNeeded()
    {
        if (floors == null || floors.Length == 0)
        {
            var floorsParent = GameObject.Find("Floors");
            if (floorsParent != null)
                floors = floorsParent.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
        }

        if (stars == null || stars.Length == 0)
        {
            var starsParent = GameObject.Find("Stars");
            if (starsParent != null)
                stars = starsParent.transform.Cast<Transform>().Select(t => t.gameObject).ToArray();
        }
    }
}




