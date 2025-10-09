using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

[System.Serializable]
public class Job
{
    public string forGang;
    public JobType jobType;
    public int reward;
    public ClothingType[] clothes;

    public string Description()
    {
        string description = "Job for " + forGang + ": \n";
        description += " - Type: " + jobType.ToString() + "\n";
        description += " - Reward: " + reward + " gold\n";
        description += " - Number of clothes: " + NumberOfClothes() + "\n";
        return description;
    }

    public int NumberOfClothes()
    {
        return clothes.Length;
    }
}
