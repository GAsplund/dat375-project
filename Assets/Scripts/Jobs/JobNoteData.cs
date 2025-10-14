using System;
using UnityEngine;

[Serializable]
public class JobNoteData
{
    public Job job;
    public Vector3 position;

    public JobNoteData(Job job, Vector3 position)
    {
        this.job = job;
        this.position = position;
    }
}
