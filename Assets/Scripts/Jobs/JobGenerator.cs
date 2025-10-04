using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class JobGenerator : MonoBehaviour
{

    [Header("Gang Settings")]
    [Tooltip("List of gangs to assign jobs to.")]
    [SerializeAs("Gang Names")]
    [SerializeField] private string[] gangs = { "The Cowboy Hats", "The Banditos" };

    [Header("Job Settings")]
    [Tooltip("Minimum number of clothing items in a job.")]
    [SerializeAs("Min Quantity")]
    [SerializeField] private int minQuantity = 1;
    [Tooltip("Maximum number of clothing items in a job.")]
    [SerializeAs("Max Quantity")]
    [SerializeField] private int maxQuantity = 5;

    [Header("Reward Settings")]
    [Tooltip("Settings for clothing rewards. Each clothing type should have a corresponding reward range. If duplicates exist, the first match will be used.")]
    [SerializeAs("Clothing Reward Settings")]
    [SerializeField] private List<ClothingReward> clothingRewards;

    [Header("Board Settings")]
    [Tooltip("If true, the job board will be populated with jobs when the scene starts.")]
    [SerializeAs("Populate Board")]
    [SerializeField] private bool populateOnStart = true;
    [Tooltip("Number of jobs to generate on the job board.")]
    [SerializeAs("Jobs To Generate")]
    [SerializeField] private int jobsToGenerate = 5;
    [Tooltip("Prefab used for job notes. Must have a SpriteRenderer and JobNote component.")]
    [SerializeAs("Job Note Prefab")]
    [SerializeField] private GameObject JobNotePrefab;
    [Tooltip("Maximum attempts to find a non-overlapping position for a job note.")]
    [SerializeAs("Max Placement Attempts")]
    [SerializeField] private int maxPlacementAttempts = 50;
    [Tooltip("Minimum distance between job notes to avoid overlap.")]
    [SerializeAs("Min Distance Between Notes")]
    [SerializeField] private float minDistanceBetweenNotes = 0.5f;

    private List<Vector3> occupiedPositions = new List<Vector3>();
    private Vector2 noteSize;

    public void Start()
    {
        if (populateOnStart)
        {
            PopulateBoard();
        }
    }

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

    private void PopulateBoard()
    {
        if (JobNotePrefab == null)
        {
            throw new System.NotSupportedException("JobNotePrefab must be assigned in the inspector to populate the job board.");
        }

        occupiedPositions.Clear();
        noteSize = JobNotePrefab.GetComponent<SpriteRenderer>().bounds.size;

        for (int i = 0; i < jobsToGenerate; i++)
        {
            var job = GenerateJob();
            CreateJobNote(job);
        }
    }

    private void CreateJobNote(Job job)
    {
        Vector3? position = GetNonOverlappingPosition();

        if (!position.HasValue)
        {
            Debug.LogWarning("Could not find a non-overlapping position for job note after maximum attempts.");
            return;
        }

        var jobNoteInstance = Instantiate(JobNotePrefab, position.Value, Quaternion.identity);
        jobNoteInstance.GetComponent<JobNote>().job = job;
        occupiedPositions.Add(position.Value);
    }

    private Vector3? GetNonOverlappingPosition()
    {
        var camera = Camera.main;
        if (camera == null)
        {
            throw new System.NotSupportedException("Main Camera not found in the scene.");
        }

        float screenZ = Mathf.Abs(camera.transform.position.z);
        Vector3 bottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, screenZ));
        Vector3 topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, screenZ));

        for (int attempt = 0; attempt < maxPlacementAttempts; attempt++)
        {
            float x = Random.Range(bottomLeft.x + noteSize.x / 2, topRight.x - noteSize.x / 2);
            float y = Random.Range(bottomLeft.y + noteSize.y / 2, topRight.y - noteSize.y / 2);
            Vector3 candidatePosition = new Vector3(x, y, 0);

            if (IsPositionValid(candidatePosition))
            {
                return candidatePosition;
            }
        }

        return null;
    }

    private bool IsPositionValid(Vector3 position)
    {
        foreach (var occupiedPos in occupiedPositions)
        {
            float distance = Vector3.Distance(position, occupiedPos);
            float minDistance = (noteSize.x + noteSize.y) / 2 + minDistanceBetweenNotes;

            if (distance < minDistance)
            {
                return false;
            }
        }

        return true;
    }
}
