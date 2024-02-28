using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpin;
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
        }
    }
    public void Spin() { totalSpin = 0f; isActive = true; }
}
