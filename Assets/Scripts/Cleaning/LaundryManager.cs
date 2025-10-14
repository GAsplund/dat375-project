using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaundryManager : MonoBehaviour
{
    [Serializable]
    public struct ClothingPrefab
    {
        public ClothingType type;
        public GameObject prefab;
    }

    [SerializeField] private TextMeshProUGUI approvedText;
    [SerializeField] private TextMeshProUGUI itemsRemainingText;
    [SerializeField] private string sceneToLoad = "InteractionScene";
    [SerializeField] private GameObject defaultPrefab;
    [SerializeField] private ClothingPrefab[] clothingPrefabs;

    private Dictionary<ClothingType, GameObject> prefabMap;
    private ClothingType[] clothesToClean;

    // Track progress using the job's items
    private int currentItem = 0;
    private int totalItemsToClean = 0;

    void Awake()
    {
        if (approvedText != null)
        {
            approvedText.enabled = false;
        }

        if (!JobManager.HasJob())
        {
            StartCoroutine(NoJobAssigned());
            return;
        }

        prefabMap = BuildPrefabMap();

        // Use the job's clothes length to track progress
        clothesToClean = JobManager.GetCurrentJob().clothes;
        totalItemsToClean = clothesToClean.Length;
        currentItem = 0;

        if (itemsRemainingText != null)
        {
            itemsRemainingText.text = $"Items Remaining: {totalItemsToClean}";
        }

        SpawnLaundryItem();
    }

    public void OnLaundryItemCleaned()
    {
        // Advance to next item from the job
        currentItem++;

        if (currentItem >= totalItemsToClean)
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
            itemsRemainingText.text = $"Items Remaining: {totalItemsToClean - currentItem}";
        }
        SpawnLaundryItem();
    }

    private void SpawnLaundryItem()
    {
        if (defaultPrefab == null)
        {
            throw new NotSupportedException("T-shirt prefab is not assigned in the inspector.");
        }

        if (clothesToClean == null || currentItem >= clothesToClean.Length)
        {
            throw new Exception("No more clothes to clean or clothesToClean is not set.");
        }

        var clothingType = clothesToClean[currentItem];
        // Lookup in the map, fall back to inspector tshirtPrefab
        GameObject prefabToSpawn = null;
        if (prefabMap != null)
        {
            prefabToSpawn = prefabMap.GetValueOrDefault(clothingType, defaultPrefab);
        }

        // Instantiate the t-shirt prefab in the middle of the screen
        Instantiate(prefabToSpawn, Vector3.zero, Quaternion.identity);
    }

    private IEnumerator OnJobDone()
    {
        JobManager.CompleteCurrentJob();

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

    private IEnumerator NoJobAssigned()
    {
        if (approvedText != null)
        {
            approvedText.enabled = true;
            approvedText.text = "You do not yet have a job!";
        }

        if (itemsRemainingText != null)
        {
            itemsRemainingText.enabled = false;
        }

        yield return new WaitForSeconds(2f);

        SceneManager.LoadScene(sceneToLoad);
    }

    /// <summary>
    /// Builds a dictionary mapping ClothingType to their corresponding prefab GameObject.
    /// This allows for efficient lookup when spawning clothing items. If multiple prefabs
    /// are assigned to the same ClothingType, the first one in the array is used.
    /// </summary>
    /// <returns>A dictionary mapping ClothingType to GameObject prefabs.</returns>
    private Dictionary<ClothingType, GameObject> BuildPrefabMap()
    {
        var map = new Dictionary<ClothingType, GameObject>();
        if (clothingPrefabs != null)
        {
            foreach (var cp in clothingPrefabs)
            {
                if (cp.prefab != null && !map.ContainsKey(cp.type))
                {
                    map[cp.type] = cp.prefab;
                }
            }
        }
        return map;
    }
}
