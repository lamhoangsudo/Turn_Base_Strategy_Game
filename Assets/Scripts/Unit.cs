using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    [SerializeField] private int actionPoin;
    public BaseAction[] baseActions {  get; private set; }
    private void Start()
    {
        actionPoin = 2;
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActions = GetComponents<BaseAction>();
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
    public MoveAction MoveAction()
    {
        return moveAction;
    }
    public SpinAction SpinAction()
    {
        return spinAction;
    }
    public GridPosition GetGridPosition() { return gridPosition; }
    public bool TryToSpendActionPoint(BaseAction selectAction)
    {
        if(actionPoin >= selectAction.GetActionPointCost())
        {
            actionPoin -= selectAction.GetActionPointCost();
            return true;
        }
        else
        {
            return false;
        }
    }
    public int GetActionPoint() { return actionPoin; }
}
