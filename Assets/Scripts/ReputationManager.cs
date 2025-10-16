using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReputationManager : MonoBehaviour
{
    private static ReputationManager Instance;

    public int MaxReputation;
    private int reputationL = 0;
    private int reputationR = 0;

    private int Lhearts = 3;
    private int Rhearts = 3;

    public SpriteRenderer Lheart1;
    public SpriteRenderer Lheart2;
    public SpriteRenderer Lheart3;

    public SpriteRenderer Rheart1;
    public SpriteRenderer Rheart2;
    public SpriteRenderer Rheart3;

    public string[] BarShouldActive;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
       if(BarShouldActive.Contains(scene.name))
        {
            gameObject.SetActive(true);
        }else
        {
            gameObject.SetActive(false);
        }
    }
    public static void ChangeReputationL(int rptn)
    {
        Instance.changeL(rptn);
    }

    public static void ChangeReputationR(int rptn)
    {
        Instance.changeR(rptn);
    }

    private void changeL(int rptn)
    {
        reputationL += rptn;
        OnReputationChanged();
    }
    private void changeR(int rptn)
    {
        reputationR += rptn;
        OnReputationChanged();
    }

    private void OnReputationChanged()
    {
        if (reputationL >= MaxReputation)
        {
            Lhearts--;
            reputationL = 0;
        }
        if (reputationR >= MaxReputation)
        {
            Rhearts--;
            reputationR = 0;
        }

        switch (Lhearts)
        {
            case 0:
                Debug.Log("game ends since Lhearts ritches 0");
                break;
            case 1:
                Lheart1.enabled = true;
                Lheart2.enabled = false;
                Lheart3.enabled = false;
                break;
            case 2:
                Lheart1.enabled = true;
                Lheart2.enabled = true;
                Lheart3.enabled = false;
                break;
            case 3:
                Lheart1.enabled = true;
                Lheart2.enabled = true;
                Lheart3.enabled = true;
                break;
        }

        switch (Rhearts)
        {
            case 0:
                Debug.Log("game ends since Rhearts ritches 0");
                break;
            case 1:
                Rheart1.enabled = true;
                Rheart2.enabled = false;
                Rheart3.enabled = false;
                break;
            case 2:
               Rheart1.enabled = true;
               Rheart2.enabled = true;
               Rheart3.enabled = false;
                break;
            case 3:
                Rheart1.enabled = true;
                Rheart2.enabled = true;
                Rheart3.enabled = true;
                break;
        }
    }

}
