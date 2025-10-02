using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobGenerator : MonoBehaviour
{

    [SerializeField] private string[] gangs = { "The Cowboy Hats", "The Banditos" };

    [SerializeField] private int minQuantity = 1;
    [SerializeField] private int maxQuantity = 5;

    [Header("Reward Settings")]
    [SerializeField] private List<ClothingReward> clothingRewards;

    public Job GenerateJob()
    {
        Job newJob = new Job();

        // Randomly select a gang
        newJob.forGang = gangs[Random.Range(0, gangs.Length)];

        // Randomly select a job type
        newJob.jobType = (JobType)Random.Range(0, System.Enum.GetValues(typeof(JobType)).Length);

        // Randomly determine the number of clothes
        int quantity = Random.Range(minQuantity, maxQuantity + 1);

        // Generate random clothes
        newJob.clothes = new ClothingType[quantity];

        for (int i = 0; i < quantity; i++)
        {
            newJob.clothes[i] = (ClothingType)Random.Range(0, System.Enum.GetValues(typeof(ClothingType)).Length);
        }

        // Set a reward based on the number of clothes, and a slight random factor for each item
        newJob.reward = 0;
        for (int i = 0; i < quantity; i++)
        {
            var cloth = (ClothingType)Random.Range(0, System.Enum.GetValues(typeof(ClothingType)).Length);
            newJob.clothes[i] = cloth;

            // Find matching reward settings
            ClothingReward rewardConfig = clothingRewards.Find(r => r.type == cloth);
            if (rewardConfig != null)
                newJob.reward += rewardConfig.GetRandomReward();
        }

        return newJob;
    }

}
