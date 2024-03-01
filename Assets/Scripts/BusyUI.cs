using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusyUI : MonoBehaviour
{
    private void Start()
    {
        gameObject.SetActive(false);
        UnitActionSystem.Instance.OnBusyChange += Instance_OnBusyChange;
    }

    private void Instance_OnBusyChange(object sender, bool isBusy)
    {
        gameObject.SetActive(isBusy);
    }
}
