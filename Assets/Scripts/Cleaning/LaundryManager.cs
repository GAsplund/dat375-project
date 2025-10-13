using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaundryManager : MonoBehaviour
{
    [Tooltip("Total number of laundry items (t-shirts) to clean to win the game")]
    [SerializeField] private int totalLaundryItems = 3;
    [SerializeField] private TextMeshProUGUI approvedText;
    [SerializeField] private TextMeshProUGUI itemsRemainingText;
    [SerializeField] private string sceneToLoad = "InteractionScene";
    [SerializeField] private GameObject tshirtPrefab;

    void Awake()
    {
        if (totalLaundryItems <= 0)
        {
            Debug.LogError("Total laundry items must be greater than zero.");
            totalLaundryItems = 1; // Default value
        }

        if (approvedText != null)
        {
            approvedText.enabled = false;
        }

        if (itemsRemainingText != null)
        {
            itemsRemainingText.text = $"Items Remaining: {totalLaundryItems}";
        }

        SpawnLaundryItem();

    }

    public void OnLaundryItemCleaned()
    {
        totalLaundryItems--;
        if (totalLaundryItems <= 0)
        {
            StartCoroutine(OnJobDone());
            return;
        }

        // Remove existing t-shirt
        var existingTShirt = GameObject.FindWithTag("LaundryItem");
        if (existingTShirt != null)
        {
            Destroy(existingTShirt);
        }

        if (itemsRemainingText != null)
        {
            itemsRemainingText.text = $"Items Remaining: {totalLaundryItems}";
        }
        SpawnLaundryItem();
    }

    private void SpawnLaundryItem()
    {
        if (tshirtPrefab == null)
        {
            throw new NotSupportedException("T-shirt prefab is not assigned in the inspector.");
        }

        // Instantiate the t-shirt prefab in the middle of the screen
        Instantiate(tshirtPrefab, Vector3.zero, Quaternion.identity);
    }

    private IEnumerator OnJobDone()
    {
        if (approvedText != null)
        {
            approvedText.enabled = true;
            approvedText.text = "Job Done!";
        }

        if (itemsRemainingText != null)
        {
            itemsRemainingText.enabled = false;
        }

        yield return new WaitForSeconds(2f);


        SceneManager.LoadScene(sceneToLoad);
    }
}
