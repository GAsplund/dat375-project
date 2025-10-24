using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class Job
{
    public string forGang;
    public JobType jobType;
    public int reward;
    public ClothingType[] clothes;
    public string noteDescription;

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

    public void JobDone()
    {
        MoneyManager.Add(reward);
        if (forGang == "The Banditos")
        {
            ReputationManager.ChangeReputationL(reward);
        }
        else
        {
            ReputationManager.ChangeReputationR(reward);
        }
    }
    public void PartlyDone(uint completedItems)
    {
        float Completation = completedItems / clothes.Length;
        MoneyManager.Add((int)Completation * reward);
        if (forGang == "The Banditos")
        {
            ReputationManager.ChangeReputationL((int)Completation * reward);

        }
        else
        {
            ReputationManager.ChangeReputationR((int)Completation * reward);

        }
    }

}
