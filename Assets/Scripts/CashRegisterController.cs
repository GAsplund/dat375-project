using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashRegisterController : MonoBehaviour
{
    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void CanPay()
    {
        animator.SetBool("CanUpgrade", true);
    }

    public void CannotPay()
    {
        animator.SetBool("CanUpgrade", false);
    }

}
