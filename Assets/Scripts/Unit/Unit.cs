using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;
    public MoveAction moveAction { get; private set; }
    public SpinAction spinAction { get; private set; }
    public ShootAction shootAction { get; private set; }
    [SerializeField] private int actionPoin;
    [SerializeField] private bool isPlayer;
    public BaseAction selectAction;
    private const int MAX_ACTION_POINT = 2;
    private HeathSystem heathSystem;
    public BaseAction[] baseActions {  get; private set; }
    public static event EventHandler<int> OnAnyActionPointChange;
    public event EventHandler OnUnitActionPointChange;
    public static event EventHandler OnUnitSwapned;
    public static event EventHandler OnUnitDead;
    
    private void Start()
    {
        actionPoin = MAX_ACTION_POINT;
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        shootAction = GetComponent<ShootAction>();
        baseActions = GetComponents<BaseAction>();
        TurnSystem.Instance.OnTurnChange += Instance_OnTurnChange;
        heathSystem = GetComponent<HeathSystem>();
        heathSystem.OnDead += Unit_OnDead;
        OnUnitSwapned?.Invoke(this, EventArgs.Empty);
    }

    private void Unit_OnDead(object sender, EventArgs e)
    {
        OnUnitDead?.Invoke(this, EventArgs.Empty);
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(this.gameObject);
    }

    private void Instance_OnTurnChange(object sender, System.EventArgs e)
    {
        if ((isPlayer == true && TurnSystem.Instance.isPlayerTurn == true) || (isPlayer == false && TurnSystem.Instance.isPlayerTurn == false)) 
        {
            actionPoin = MAX_ACTION_POINT;
            OnAnyActionPointChange?.Invoke(this, MAX_ACTION_POINT);
        }
    }

    private void Update()
    {
        GridPosition currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (currentGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = currentGridPosition;
            LevelGrid.Instance.UnitMoveGridPosition(this, oldGridPosition, currentGridPosition);
        }
    }
    public GridPosition GetGridPosition() { return gridPosition; }
    public bool TryToSpendActionPoint(BaseAction selectAction, bool returnActionPoint)
    {
        if (actionPoin >= selectAction.GetActionPointCost() && returnActionPoint == false)
        {
            actionPoin -= selectAction.GetActionPointCost();
            OnUnitActionPointChange?.Invoke(this, EventArgs.Empty);
            return true;
        }
        else if (returnActionPoint == true)
        {
            actionPoin += selectAction.GetActionPointCost();
            return true;
        }
        else { return false; }
    }
    public int GetActionPoint() { return actionPoin; }
    public bool IsPlayer() { return isPlayer; }
    public void DamageUnit(int  damageAmount)
    {
        heathSystem.Damage(damageAmount);
    }
}
