using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    private CinemachineImpulseSource cinemachineImpulseSource;
    private void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }
    private void Start()
    {
        ShootAction.OnAnyShootAction += ShootAction_OnAnyShootAction;
        GrenadeProjectileAction.OnAnyGrenadeexploded += GrenadeProjectileAction_OnAnyGrenadeexploded;
    }

    private void GrenadeProjectileAction_OnAnyGrenadeexploded(object sender, EventArgs e)
    {
        Shake(6f);
    }

    private void ShootAction_OnAnyShootAction(object sender, System.EventArgs e)
    {
        Shake(5f);
    }

    private void Shake(float force)
    {
        cinemachineImpulseSource.GenerateImpulse(force);
    }
}
