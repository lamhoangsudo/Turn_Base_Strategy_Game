using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
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
}
