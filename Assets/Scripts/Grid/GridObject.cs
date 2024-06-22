using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridPosition gridPosition;
    private GridSystem<GridObject> gridSystem;
    private List<Unit> listUnit;
    private IInteract interactObject;
    public GridObject(GridPosition gridPosition, GridSystem<GridObject> gridSystem)
    {
        this.gridPosition = gridPosition;
        this.gridSystem = gridSystem;
        listUnit = new List<Unit>();
    }
    public override string ToString()
    {
        string stringUnit = "";
        foreach (Unit unit in listUnit)
        {
            stringUnit += unit.ToString();
        }
        return gridPosition.ToString() + "\n" + stringUnit;
    }
    public void AddUnitAtGridObject(Unit unit)
    {
        listUnit.Add(unit);
    }
    public void RemoveUnitAtGridObject(Unit unit)
    {
        listUnit.Remove(unit);
    }
    public List<Unit> GetUnitAtGridObject()
    {
        return listUnit;
    }
    public bool HasAnyUnit()
    {
        return listUnit.Count > 0;
    }
    public IInteract GetInteractObjectAtGridObject()
    {
        return interactObject;
    }
    public void SetInteractObjectAtGridObject(IInteract interactObject)
    {
        this.interactObject = interactObject;
    }

}
