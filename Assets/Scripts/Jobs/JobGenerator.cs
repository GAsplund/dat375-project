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

    [Tooltip("Note descriptions to randomly assign to jobs."), SerializeAs("Note Descriptions")]
    [SerializeField]
    private List<string> noteDescriptions = new List<string>
    {
        "Red shirts from last night's skirmish need cleaning. Wash out the iron scent before dawn. Payment is swift as always.",
        "Coats torn in the alley brawl need cleaning. Scrub them clean without questions. We're building strength and your silence helps.",
        "Trousers soaked in rival blood need cleaning. Erase the evidence as tensions rise. Loyalty now means more than coin.",
        "Vests punched with blade marks need cleaning. Mend and purify them quietly. The Blades encroach so stay neutral for now.",
        "Hats dirtied from a failed ambush need cleaning. Restore them to spotless condition. Fists grow bold and we need your discretion.",
        "Gloves gripped in betrayal's fight need cleaning. Cleanse the traces. Blades whisper plots and your work buys time.",
        "Suits slashed in midnight revenge need cleaning. Iron out the damage. War brews so choose sides with care.",
        "Scarves knotted from choked alliances need cleaning. Untangle and launder them. Blades strike first and retaliation is imminent.",
        "Collars stained by final pleas need cleaning. Loosen the grime. Fists close in and your balance tips the scales.",
        "Capes heavy with fallen brothers need cleaning. Revive and return them. The feud ends soon so whose laundry is next?",
        "Shirts ripped in warehouse raids need cleaning. Wash away the chaos. Fists demand tribute and your neutrality frays.",
        "Ties bound with desperate oaths need cleaning. Clean and reinforce them. Blades sabotage our lines and vigilance is required.",
        "Boots caked in territorial mud need cleaning. Polish them to perfection. Fists rally forces and alliances shift underfoot.",
        "Jackets pierced by warning shots need cleaning. Patch and purify them. Blades encircle our turf and your aid could turn tides.",
        "Belts buckled in hasty retreats need cleaning. Straighten the stains. Fists forge new pacts and betrayal looms larger.",
        "Socks soaked from flooded hideouts need cleaning. Dry and deliver them. Blades flood the streets so hold the line with us.",
        "Handkerchiefs marked by sealed deals need cleaning. Wipe the ink clean. Fists break old vows and war's edge sharpens.",
        "Pants torn in escalating clashes need cleaning. Mend the rifts quietly. Blades prepare the final push so choose wisely now.",
        "Vests burdened by heavy secrets need cleaning. Lighten and launder them. Fists muster for battle and your role defines the end.",
        "Coats cloaked in impending doom need cleaning. Scrub them for survival. The clash erupts soon so whose colors will you clean?"
    };

    [Header("Reward Settings")]
    [Tooltip("Settings for clothing rewards. Each clothing type should have a corresponding reward range. If duplicates exist, the first match will be used.")]
    [SerializeAs("Clothing Reward Settings")]
    [SerializeField] private List<ClothingReward> clothingRewards;

    [Header("Board Settings")]
    [Tooltip("If true, the job board will be populated with jobs when the scene starts.")]
    [SerializeAs("Populate Board")]
    [SerializeField] private bool populateOnStart = true;
    [Tooltip("Minimum number of job notes to keep on the board.")]
    [SerializeAs("Jobs Min")]
    [SerializeField] private int jobsMin = 3;
    [Tooltip("Maximum number of job notes to keep on the board.")]
    [SerializeAs("Jobs Max")]
    [SerializeField] private int jobsMax = 6;
    [Tooltip("Prefab used for job notes. Must have a SpriteRenderer and JobNote component.")]
    [SerializeAs("Job Note Prefab")]
    [SerializeField] private GameObject JobNotePrefab;
    [Tooltip("Maximum attempts to find a non-overlapping position for a job note.")]
    [SerializeAs("Max Placement Attempts")]
    [SerializeField] private int maxPlacementAttempts = 50;
    [Tooltip("Minimum distance between job notes to avoid overlap.")]
    [SerializeAs("Min Distance Between Notes")]
    [SerializeField] private float minDistanceBetweenNotes = 0.5f;

    [Header("Spawn Area Settings")]
    [Tooltip("If true, uses a custom spawn area instead of the entire screen.")]
    [SerializeField] private bool useCustomSpawnArea = false;
    [Tooltip("The minimum bounds of the spawn area (bottom-left corner in world coordinates).")]
    [SerializeField] private Vector2 spawnAreaMin = new Vector2(-5f, -5f);
    [Tooltip("The maximum bounds of the spawn area (top-right corner in world coordinates).")]
    [SerializeField] private Vector2 spawnAreaMax = new Vector2(5f, 5f);

    private List<Vector3> occupiedPositions = new List<Vector3>();
    private Vector2 noteSize;
    private List<JobNoteData> generatedNotesData = new List<JobNoteData>();

    public void Start()
    {
        if (!populateOnStart)
        {
            return;
        }

        var stored = JobManager.GetGeneratedJobs();
        if (stored != null && stored.Count > 0)
        {
            InstantiateFromStored(stored);
            return;
        }

        PopulateBoard();
        if (generatedNotesData.Count > 0)
        {
            JobManager.StoreGeneratedJobs(generatedNotesData);
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

        // Randomly select a note description
        newJob.noteDescription = noteDescriptions[Random.Range(0, noteDescriptions.Count)];

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

        // Generate up to the maximum initially, then store them.
        for (int i = 0; i < jobsMax; i++)
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
        generatedNotesData.Add(new JobNoteData(job, position.Value));
    }

    private void InstantiateFromStored(List<JobNoteData> stored)
    {
        if (JobNotePrefab == null)
        {
            throw new System.NotSupportedException("JobNotePrefab must be assigned in the inspector to populate the job board.");
        }

        occupiedPositions.Clear();
        noteSize = JobNotePrefab.GetComponent<SpriteRenderer>().bounds.size;

        foreach (var data in stored)
        {
            var jobNoteInstance = Instantiate(JobNotePrefab, data.position, Quaternion.identity);
            jobNoteInstance.GetComponent<JobNote>().job = data.job;
            occupiedPositions.Add(data.position);
            generatedNotesData.Add(data);
        }

        ReplenishIfNeeded();
    }

    /// <summary>
    /// Call this to evaluate the current number of job notes and replenish according to min/max & probability rules.
    /// This method will create up to `jobsMax - currentCount` new notes, with higher chance when the board is further below max.
    /// </summary>
    public void ReplenishIfNeeded()
    {
        int current = generatedNotesData.Count;
        if (current >= jobsMax) return;

        // If below minimum, generate at least one (or up to fill min)
        if (current < jobsMin)
        {
            int toCreate = Mathf.Min(jobsMax - current, Mathf.Max(1, jobsMin - current));
            for (int i = 0; i < toCreate; i++)
            {
                var job = GenerateJob();
                CreateJobNote(job);
            }
            current = generatedNotesData.Count;
        }

        // For remaining slots up to max, use probability per slot
        for (int slot = current + 1; slot <= jobsMax; slot++)
        {
            float deficit = jobsMax - (slot - 1);
            float probability = Mathf.Clamp01(deficit / jobsMax);
            if (Random.value < probability)
            {
                var job = GenerateJob();
                CreateJobNote(job);
            }
        }

        JobManager.StoreGeneratedJobs(generatedNotesData);
    }

    /// <summary>
    /// Removes an existing job from the board and updates the stored job list.
    /// </summary>
    public void RemoveJob(Job job)
    {
        if (job == null)
        {
            return;
        }

        // We have to go backwards to safely remove while iterating
        for (int i = generatedNotesData.Count - 1; i >= 0; i--)
        {
            if (generatedNotesData[i].job == job)
            {
                occupiedPositions.Remove(generatedNotesData[i].position);
                generatedNotesData.RemoveAt(i);
                break;
            }
        }

        foreach (var note in FindObjectsOfType<JobNote>())
        {
            if (note != null && note.job == job)
            {
                Destroy(note.gameObject);
                break;
            }
        }

        JobManager.StoreGeneratedJobs(generatedNotesData);
        ReplenishIfNeeded();
    }

    private Vector3? GetNonOverlappingPosition()
    {
        Vector3 bottomLeft, topRight;

        if (useCustomSpawnArea)
        {
            // Use custom spawn area with min/max bounds
            bottomLeft = new Vector3(spawnAreaMin.x, spawnAreaMin.y, 0);
            topRight = new Vector3(spawnAreaMax.x, spawnAreaMax.y, 0);
        }
        else
        {
            // Use entire screen (original behavior)
            var camera = Camera.main;
            if (camera == null)
            {
                throw new System.NotSupportedException("Main Camera not found in the scene.");
            }

            float screenZ = Mathf.Abs(camera.transform.position.z);
            bottomLeft = camera.ScreenToWorldPoint(new Vector3(0, 0, screenZ));
            topRight = camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, screenZ));
        }

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

    // Draw the spawn area in the Unity Editor for visualization
    private void OnDrawGizmosSelected()
    {
        if (useCustomSpawnArea)
        {
            Vector3 center = new Vector3(
                (spawnAreaMin.x + spawnAreaMax.x) / 2,
                (spawnAreaMin.y + spawnAreaMax.y) / 2,
                0
            );
            Vector3 size = new Vector3(
                spawnAreaMax.x - spawnAreaMin.x,
                spawnAreaMax.y - spawnAreaMin.y,
                0.1f
            );

            Gizmos.color = new Color(0, 1, 0, 0.3f); // Semi-transparent green
            Gizmos.DrawCube(center, size);

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(center, size);
        }
    }
}
