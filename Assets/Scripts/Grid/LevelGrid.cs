using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    [SerializeField] private Transform gridDebugPrefab;
    private GridSystem<GridObject> gridSystem;
    public static LevelGrid Instance;
    public event EventHandler OnUnitMoveGripPositonUpdate;
    private void Awake()
    {
        Instance = this;
        gridSystem = new (10, 10, 2f, (GridPosition p, GridSystem<GridObject> s) => new GridObject(p, s));
        gridSystem.CreateGridDebugObject(gridDebugPrefab);
    }
    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.AddUnitAtGridObject(unit);
    }
    public List<Unit> GetUnitAtGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).GetUnitAtGridObject();
    }
    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        GridObject gridObject = gridSystem.GetGridObject(gridPosition);
        gridObject.RemoveUnitAtGridObject(unit);
    }
    public GridPosition GetGridPosition(Vector3 position)
    {
        return gridSystem.GetGridPosition(position);
    }
    public Vector3 GetGridPosition(GridPosition position)
    {
        return gridSystem.GetWorldPosition(position);
    }
    public void UnitMoveGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
        OnUnitMoveGripPositonUpdate?.Invoke(this, EventArgs.Empty);
    }
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridSystem.IsValidGridPosition(gridPosition);
    }
    public bool IsUnitOnGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).HasAnyUnit();
    }
    public void GetWidthAndHeight(out int width, out int height)
    {
        width = gridSystem.width;
        height = gridSystem.height;
    }
}
