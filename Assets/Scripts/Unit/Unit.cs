using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;
    [SerializeField] private int actionPoin;
    [SerializeField] private bool isPlayer;
    [SerializeField] private int grenadeAmount;
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
        baseActions = GetComponents<BaseAction>();
        TurnSystem.Instance.OnTurnChange += Instance_OnTurnChange;
        heathSystem = GetComponent<HeathSystem>();
        heathSystem.OnDead += Unit_OnDead;
        OnUnitSwapned?.Invoke(this, EventArgs.Empty);
    }
    public T GetAction<T> () where T : BaseAction
    {
        foreach (var baseAction in baseActions) 
        { 
            if(baseAction is T t)
            {
                return t;
            }
        }
        return null;
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
    public bool TryToSpendActionPoint(BaseAction selectAction, bool returnActionPointOrCheckBestAIAction)
    {
        if (isPlayer)
        {
            if (actionPoin >= selectAction.GetActionPointCost() && returnActionPointOrCheckBestAIAction == false)
            {
                actionPoin -= selectAction.GetActionPointCost();
                OnUnitActionPointChange?.Invoke(this, EventArgs.Empty);
                return true;
            }
            else if (returnActionPointOrCheckBestAIAction == true)
            {
                actionPoin += selectAction.GetActionPointCost();
                return true;
            }
        }
        else
        {
            int actionPoinCalculated = actionPoin - selectAction.GetActionPointCost();
            if (actionPoinCalculated >= 0 && returnActionPointOrCheckBestAIAction == true)
            {
                return true;
            }
            else if(returnActionPointOrCheckBestAIAction == false)
            {
                actionPoin = actionPoinCalculated;
                OnUnitActionPointChange?.Invoke(this, EventArgs.Empty);
                return true;
            }
        }
        return false;
    }
    public int GetActionPoint() { return actionPoin; }
    public bool IsPlayer() { return isPlayer; }
    public void DamageUnit(int  damageAmount)
    {
        heathSystem.Damage(damageAmount);
    }
    public float GetHealthAmount()
    {
        return heathSystem.GetHealthNormalize();
    }
    public int GetGrenadeAmount()
    {
        return grenadeAmount;
    }
}
