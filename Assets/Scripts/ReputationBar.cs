using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReputationBar : MonoBehaviour
{
    [SerializeField] private List<MonoBehaviour> Lhearts = new List<MonoBehaviour>();
    [SerializeField] private List<MonoBehaviour> Rhearts = new List<MonoBehaviour>();

    public List<MonoBehaviour> GetLeftHearts()
    {
        return Lhearts;
    }

    public List<MonoBehaviour> GetRightHearts()
    {
        return Rhearts;
    }
}
