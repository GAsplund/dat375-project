using UnityEngine;

public class JobManager : MonoBehaviour
{
    private static JobManager Instance;

    private Job CurrentJob;

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
}
