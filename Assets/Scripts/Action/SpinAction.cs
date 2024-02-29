using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpin;
    private Action spinComplete;
    private void Update()
    {
        if (!isActive)
        {
            return;
        }
        float spinAmount = 360 * Time.deltaTime;
        totalSpin += spinAmount;
        transform.eulerAngles += new Vector3(0, spinAmount, 0);
        if (totalSpin >= 360)
        {
            isActive = false;
            spinComplete();
        }
    }
    public void Spin(Action onSpinComplete) 
    { 
        spinComplete = onSpinComplete;
        totalSpin = 0f;
        isActive = true;
    }
}
