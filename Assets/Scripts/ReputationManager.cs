using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReputationManager : MonoBehaviour
{
    private static ReputationManager Instance;

    // Event fired whenever a heart is lost. Parameter: string side ("L" or "R").
    public static event Action<string> OnHeartLost;

    [SerializeField] private string GameOverScene = "GameOverScene";
    [SerializeField] private int MaxReputation = 100;

    private MonoBehaviour Lheart1;
    private MonoBehaviour Lheart2;
    private MonoBehaviour Lheart3;
    private Slider LSlider;

    private MonoBehaviour Rheart1;
    private MonoBehaviour Rheart2;
    private MonoBehaviour Rheart3;
    private Slider RSlider;

    public string[] BarShouldActive;

    private int reputationL = 0;
    private int reputationLBuffer = 0;
    private int reputationR = 0;
    private int reputationRBuffer = 0;

    private int Lhearts = 3;
    private int Rhearts = 3;

    private string currentScene = "InteractionScene";

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (BarShouldActive.Contains(scene.name))
        {
            gameObject.SetActive(true);

            RegisterHearts();

            reputationL += reputationLBuffer;
            reputationR += reputationRBuffer;

            reputationLBuffer = 0;
            reputationRBuffer = 0;

            OnReputationChanged();
        }
        else
        {
            gameObject.SetActive(false);
        }
        currentScene = scene.name;
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
        if (!BarShouldActive.Contains(currentScene))
        {
            reputationLBuffer += rptn;
            reputationRBuffer -= rptn;
            return;
        }
        reputationL += rptn;
        reputationR -= rptn;
        OnReputationChanged();
    }
    private void changeR(int rptn)
    {
        if (!BarShouldActive.Contains(currentScene))
        {
            reputationRBuffer += rptn;
            reputationLBuffer -= rptn;
            return;
        }
        reputationR += rptn;
        reputationL -= rptn;
        OnReputationChanged();
    }

    private void OnReputationChanged()
    {
        Debug.Log("Reputation Changed: L=" + reputationL + ", R=" + reputationR);
        if (reputationL >= MaxReputation)
        {
            Lhearts--;
            reputationL = 0;
            OnHeartLost?.Invoke("L");
        }
        if (reputationR >= MaxReputation)
        {
            Rhearts--;
            reputationR = 0;
            OnHeartLost?.Invoke("R");
        }

        LSlider.value = (float)reputationL / MaxReputation;
        RSlider.value = (float)reputationR / MaxReputation;

        Debug.Log("Hearts Left: L=" + Lhearts + ", R=" + Rhearts);

        switch (Lhearts)
        {
            case 0:
                SceneManager.LoadScene(GameOverScene);
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
                SceneManager.LoadScene(GameOverScene);
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

    private void RegisterHearts()
    {
        var reputationBar = FindObjectOfType<ReputationBar>();

        var lHearts = reputationBar.GetLeftHearts();
        if (lHearts.Count >= 3)
        {
            if (lHearts.Count > 3)
            {
                Debug.LogWarning("More than 3 left hearts found in ReputationBar. Only the first 3 will be used.");
            }
            Lheart1 = lHearts[0];
            Lheart2 = lHearts[1];
            Lheart3 = lHearts[2];
        }
        else
        {
            throw new System.NotSupportedException("Not enough left hearts found in ReputationBar. At least 3 are required.");
        }
        LSlider = reputationBar.GetLeftReputationBar();

        var rHearts = reputationBar.GetRightHearts();
        if (rHearts.Count >= 3)
        {
            if (rHearts.Count > 3)
            {
                Debug.LogWarning("More than 3 right hearts found in ReputationBar. Only the first 3 will be used.");
            }
            Rheart1 = rHearts[0];
            Rheart2 = rHearts[1];
            Rheart3 = rHearts[2];
        }
        else
        {
            throw new System.NotSupportedException("Not enough right hearts found in ReputationBar. At least 3 are required.");
        }
        RSlider = reputationBar.GetRightReputationBar();

    }
}
