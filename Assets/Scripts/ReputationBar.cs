using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReputationBar : MonoBehaviour
{
    [SerializeField] private List<SpriteRenderer> Lhearts = new List<SpriteRenderer>();
    [SerializeField] private List<SpriteRenderer> Rhearts = new List<SpriteRenderer>();

    public List<SpriteRenderer> GetLeftHearts()
    {
        return Lhearts;
    }

    public List<SpriteRenderer> GetRightHearts()
    {
        return Rhearts;
    }
}
