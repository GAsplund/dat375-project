using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashingTable : MonoBehaviour
{


    void Start()
    {
        var anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("No Animator component found on WashingTable object.");
            return;
        }

        anim.SetBool("JobSelected", JobManager.HasJob());
    }
}
