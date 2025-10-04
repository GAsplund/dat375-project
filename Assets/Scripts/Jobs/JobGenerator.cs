using System.Collections.Generic;
using UnityEngine;

public class JobGenerator : MonoBehaviour
{

    [Header("Gang Settings")]
    [SerializeField] private string[] gangs = { "The Cowboy Hats", "The Banditos" };

    [Header("Job Settings")]
    [SerializeField] private int minQuantity = 1;
    [SerializeField] private int maxQuantity = 5;

    [Header("Reward Settings")]
    [SerializeField] private List<ClothingReward> clothingRewards;

    [Header("Board Settings")]
    [SerializeField] private bool populateOnStart = true;
    [SerializeField] private int jobsToGenerate = 5;
    [SerializeField] private GameObject JobNotePrefab;

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

    private void PopulateBoard()
    {
        if (JobNotePrefab == null)
        {
            throw new System.NotSupportedException("JobNotePrefab must be assigned in the inspector to populate the job board.");
        }

        for (int i = 0; i < jobsToGenerate; i++)
        {
            var job = GenerateJob();
            CreateJobNote(job);
        }
    }

    private void CreateJobNote(Job job)
    {
        var position = GetRandomJobNotePosition();
        var jobNoteInstance = Instantiate(JobNotePrefab, position, Quaternion.identity);
        jobNoteInstance.GetComponent<JobNote>().job = job;
    }

    private Vector3 GetRandomJobNotePosition()
    {
        var camera = Camera.main;
        if (camera == null)
        {
            throw new System.NotSupportedException("Main Camera not found in the scene.");
        }

        float screenZ = Mathf.Abs(camera.transform.position.z); // Distance from camera to z=0 plane

        Vector3 bottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, screenZ));
        Vector3 topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, screenZ));

        Vector2 noteSize = JobNotePrefab.GetComponent<SpriteRenderer>().bounds.size;

        float x = Random.Range(bottomLeft.x + noteSize.x / 2, topRight.x - noteSize.x / 2);
        float y = Random.Range(bottomLeft.y + noteSize.y / 2, topRight.y - noteSize.y / 2);

        return new Vector3(x, y, 0);
    }

}
