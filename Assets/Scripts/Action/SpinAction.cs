using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinAction : BaseAction
{
    private float totalSpin;
    private const string actionName = "Spin";
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
            base.ActionComplete();
        }
    }
    public override string GetNameAction()
    {
        return actionName;
    }

    public override void GetAction(Action onSpinComplete, Unit unitAction)
    {
        unit = unitAction;
        totalSpin = 0f;
        base.ActionStart(onSpinComplete);
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction()
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }
}
