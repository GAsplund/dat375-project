/// <summary>
/// Defines a clothing reward with a type and a range for the reward amount. Used in job generation.
/// </summary>
[System.Serializable]
public class ClothingReward
{
    public ClothingType type;
    public int minReward;
    public int maxReward;

    public int GetRandomReward()
    {
        return UnityEngine.Random.Range(minReward, maxReward + 1);
    }
}