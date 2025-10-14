using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    private Job currentJob;

    private float currentMoney = 0;
    
    // ONLY ONE SHOULD EXSIST 
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);

        Instance = this;
    }

    // Update is called once per frame
  public  void SetCurrentJob(Job job)
    { 
        currentJob = job; 
    }
  public  void CurrentJobDone(float precnetage)
    {
        if(currentJob==null)
        {
            Debug.LogError("calling CurrentJobDone without setting CurrentJob");
            return;
        }
        currentMoney += precnetage * currentJob.reward;

        currentJob=null;
    }
    public int GetCurrentMoney()
    {
        return (int)currentMoney;
    }

}
