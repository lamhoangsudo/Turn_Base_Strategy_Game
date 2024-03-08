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
    [SerializeField] private int actionPoin;
    [SerializeField] private bool isPlayer;
    public BaseAction selectAction;
    private const int MAX_ACTION_POINT = 2;
    private HeathSystem heathSystem;
    public BaseAction[] baseActions {  get; private set; }
    public static event EventHandler<int> OnAnyActionPointChange;
    
    private void Start()
    {
        actionPoin = MAX_ACTION_POINT;
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActions = GetComponents<BaseAction>();
        TurnSystem.Instance.OnTurnNumberChange += Instance_OnTurnNumberChange;
        heathSystem = GetComponent<HeathSystem>();
        heathSystem.OnDead += Unit_OnDead;
    }

    private void Unit_OnDead(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(this.gameObject);
    }

    private void Instance_OnTurnNumberChange(object sender, System.EventArgs e)
    {
        if ((isPlayer == true && TurnSystem.Instance.isPlayerTurn == true) || (isPlayer == false && TurnSystem.Instance.isPlayerTurn == false)) 
        {
            actionPoin = MAX_ACTION_POINT;
            OnAnyActionPointChange(this, MAX_ACTION_POINT);
        }
    }

    private void Update()
    {
        GridPosition currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (currentGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMoveGridPosition(this, gridPosition, currentGridPosition);
            gridPosition = currentGridPosition;
        }
    }
    public GridPosition GetGridPosition() { return gridPosition; }
    public bool TryToSpendActionPoint(BaseAction selectAction, bool returnActionPoint)
    {
        if (actionPoin >= selectAction.GetActionPointCost() && returnActionPoint == false)
        {
            actionPoin -= selectAction.GetActionPointCost();
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
