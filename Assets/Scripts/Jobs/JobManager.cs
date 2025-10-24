using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    private static JobManager Instance;

    private Job CurrentJob;
    // Store serializable data for job notes so they can persist between scenes
    private List<JobNoteData> jobNotesData = new List<JobNoteData>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // ========== Static Convenience Methods ==========

    /// <summary>
    /// Static method to set the current job. Ensures JobManager exists.
    /// </summary>
    /// <param name="job">The job to store</param>
    public static void SetJob(Job job)
    {
        if (Instance == null)
        {
            throw new System.NotSupportedException("JobManager instance does not exist in the scene. Cannot set job.");
        }

        Instance.SetCurrentJob(job);
    }

    /// <summary>
    /// Static method to get the current job.
    /// </summary>
    /// <returns>The current job, or null if no job is set or JobManager doesn't exist</returns>
    public static Job GetCurrentJob()
    {
        if (Instance == null)
        {
            throw new System.NotSupportedException("JobManager instance does not exist in the scene. Cannot get job.");
        }

        return Instance.CurrentJob;
    }

    /// <summary>
    /// Static method to mark the current job as completed and clear it.
    /// </summary>
    /// <remarks> This also removes the job from the stored job notes. </remarks>
    public static void CompleteCurrentJob()
    {
        if (Instance == null)
        {
            throw new System.NotSupportedException("JobManager instance does not exist in the scene. Cannot complete job.");
        }

        if (Instance.CurrentJob == null)
        {
            return; // No job to complete
        }

        Instance.jobNotesData.RemoveAll(note => note.job == Instance.CurrentJob);
        Debug.Log($"JobManager: Job for {Instance.CurrentJob.forGang} completed! Reward: {Instance.CurrentJob.reward} gold");
        Instance.CurrentJob.JobDone();
        Instance.ClearCurrentJob();

        // If there's a JobGenerator in the scene, notify it to replenish the board if needed
        var generator = FindObjectOfType<JobGenerator>();
        if (generator != null)
        {
            generator.ReplenishIfNeeded();
        }
    }

    public static void PartlyCompleteCurrentJob(uint CompletedItems)
    {
        if (Instance == null)
        {
            throw new System.NotSupportedException("JobManager instance does not exist in the scene. Cannot complete job.");
        }
        if (Instance.CurrentJob == null)
        {
            return; // No job to complete
        }
        Instance.CurrentJob.PartlyDone(CompletedItems);
        Instance.jobNotesData.RemoveAll(note => note.job == Instance.CurrentJob);
        Debug.Log($"JobManager: Job for {Instance.CurrentJob.forGang} partlt completed!");
        Instance.ClearCurrentJob();
        var generator = FindObjectOfType<JobGenerator>();
        if (generator != null)
        {
            generator.ReplenishIfNeeded();
        }
    }

    /// <summary>
    /// Static method to clear the current job.
    /// </summary>
    public static void ClearJob()
    {
        if (Instance == null)
        {
            throw new System.NotSupportedException("JobManager instance does not exist in the scene. Cannot clear job.");
        }

        Instance.ClearCurrentJob();
    }

    /// <summary>
    /// Static method to check if a job is currently set.
    /// </summary>
    /// <returns>True if a job is set, false otherwise</returns>
    public static bool HasJob()
    {
        return Instance != null && Instance.CurrentJob != null;
    }

    /// <summary>
    /// Static method to get jobs that have already been generated and stored.
    /// </summary>
    public static List<JobNoteData> GetGeneratedJobs()
    {
        if (Instance == null)
        {
            throw new System.NotSupportedException("JobManager instance does not exist in the scene. Cannot get generated jobs.");
        }

        return Instance.jobNotesData;
    }

    /// <summary>
    /// Static method to store generated job notes.
    /// </summary>
    /// <param name="jobs">The job notes to store</param>
    public static void StoreGeneratedJobs(List<JobNoteData> jobs)
    {
        if (Instance == null)
        {
            throw new System.NotSupportedException("JobManager instance does not exist in the scene. Cannot store generated jobs.");
        }

        Instance.SetGeneratedJobs(jobs);
    }

    // ========== Instance Methods ==========

    /// <summary>
    /// Instance method to set the current job.
    /// </summary>
    /// <param name="job">The job to set as current.</param>
    private void SetCurrentJob(Job job)
    {
        CurrentJob = job;
        Debug.Log($"JobManager: Job set for {job.forGang}, reward: {job.reward} gold");
    }

    /// <summary>
    /// Instance method to clear the current job.
    /// </summary>
    private void ClearCurrentJob()
    {
        CurrentJob = null;
        Debug.Log("JobManager: Current job cleared");
    }

    /// <summary>
    /// Instance method to store generated job notes.
    /// </summary>
    /// <param name="jobs">The job notes to store</param>
    private void SetGeneratedJobs(List<JobNoteData> jobs)
    {
        jobNotesData = jobs ?? new List<JobNoteData>();
        Debug.Log($"JobManager: Stored {jobNotesData.Count} generated jobs");
    }
}
