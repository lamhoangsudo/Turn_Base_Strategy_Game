using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ShootAction : BaseAction
{
    [SerializeField] private int MAX_DISTANCE;
    [SerializeField] private float rotationSpeed;
    public Unit targetUnit {  get; private set; }
    private bool canShootBullet;
    public event EventHandler<SetUpBulletProjectile> OnShootAction;
    public class SetUpBulletProjectile : EventArgs
    {
        public Unit tagetUnit;
        public Unit shootingUnit;
    }
    private enum State
    {
        Aiming,
        Shooting,
        Cooloff
    }
    private State state;
    private float stateTimer;
    private void Update()
    {
        if (!isActive) return;
        stateTimer -= Time.deltaTime;
        switch (state)
        {
            case State.Aiming:
                Aim();
                break;
            case State.Shooting:
                if (canShootBullet)
                {
                    Shoot();
                    canShootBullet = false;
                }
                break;
            case State.Cooloff:
                //Cooloff();
                break;
        }
        if (stateTimer < 0)
        {
            StateShootHandler();
        }
    }
    private void StateShootHandler()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                float timeStateShooting = 0.1f;
                stateTimer = timeStateShooting;
                break;
            case State.Shooting:
                state = State.Cooloff;
                float timeStateCooloff = 0.5f;
                stateTimer = timeStateCooloff;
                break;
            case State.Cooloff:
                isActive = false;
                base.ActionComplete();
                break;
        }
    }
    private void Shoot()
    {
        OnShootAction?.Invoke(this, new SetUpBulletProjectile
        {
            tagetUnit = targetUnit,
            shootingUnit = unit
        });
        targetUnit.DamageUnit(20);
    }
    private void Aim()
    {
        unit.transform.forward = Vector3.Slerp(unit.transform.forward, (targetUnit.transform.position - unit.transform.position).normalized, Time.deltaTime * rotationSpeed);
    }
    public override List<GridPosition> GetListValidGridPosition()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        return GetListValidGridPosition(unitGridPosition);
    }
    public List<GridPosition> GetListValidGridPosition(GridPosition unitGridPosition)
    {
        List<GridPosition> gridPositionsValid = new();
        
        for (int x = -MAX_DISTANCE; x <= MAX_DISTANCE; x++)
        {
            for (int z = -MAX_DISTANCE; z <= MAX_DISTANCE; z++)
            {
                GridPosition validGridPosition = unitGridPosition + new GridPosition(x, z);
                if (LevelGrid.Instance.IsValidGridPosition(validGridPosition)
                    && LevelGrid.Instance.IsUnitOnGridPosition(validGridPosition)
                    && validGridPosition != unitGridPosition
                    && unit.IsPlayer() != LevelGrid.Instance.GetUnitAtGridPosition(validGridPosition)[0].IsPlayer()
                    && InsideCircleRange(validGridPosition))
                {
                    gridPositionsValid.Add(validGridPosition);
                }
            }
        }
        return gridPositionsValid;
    }
    public override bool IsValidGridPosition(GridPosition gridPosition)
    {
        return GetListValidGridPosition().Contains(gridPosition);
    }
    public override void GetAction(Action onShootComplete, Unit unitAction)
    {
        unit = unitAction;
        if (unit.IsPlayer())
        {
            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetTagetPosititon()))[0];
        }
        else
        {
            Vector3 tagetPosition = LevelGrid.Instance.GetGridPosition(aIAction.gridPosition);
            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(LevelGrid.Instance.GetGridPosition(tagetPosition))[0];
        }
        state = State.Aiming;
        float timeStateAiming = 1f;
        stateTimer = timeStateAiming;
        canShootBullet = true;
        base.ActionStart(onShootComplete);
    }
    public override string GetNameAction()
    {
        return "Shoot";
    }
    private bool InsideCircleRange(GridPosition position)
    {
        return Mathf.Pow((unit.GetGridPosition().x - position.x), 2) + Mathf.Pow((unit.GetGridPosition().z - position.z), 2) <= Mathf.Pow(MAX_DISTANCE + 0.5f, 2);
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit tagetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition)[0];

        return new EnemyAIAction()
        {
            gridPosition = gridPosition,
            actionValue = 100 + Mathf.RoundToInt((1 - tagetUnit.GetHealthAmount()) * 100f)
        };
    }
    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetListValidGridPosition(gridPosition).Count;
    }
}
