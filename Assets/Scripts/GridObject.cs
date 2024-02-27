using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridPosition gridPosition;
    private GridSystem gridSystem;
    private List<Unit> listUnit;

    public GridObject(GridPosition gridPosition, GridSystem gridSystem)
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
}
