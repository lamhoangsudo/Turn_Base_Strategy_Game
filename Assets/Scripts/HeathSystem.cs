using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeathSystem : MonoBehaviour
{
    [SerializeField] private int heathMax = 100;
    private int heath;
    public event EventHandler OnDamage;
    public event EventHandler OnHeal;
    public event EventHandler OnDead;
    private void Awake()
    {
        heath = heathMax;
    }
    public void Damage(int damageAmount)
    {
        heath -= damageAmount;
        if (heath <= 0)
        {
            Dead();
        }
    }
    public void Heal(int healAmount)
    {
        heath = Mathf.Clamp(heath += healAmount, 0, heathMax);
    }
    public void Dead()
    {
        OnDead?.Invoke(this, EventArgs.Empty);
    }
}
