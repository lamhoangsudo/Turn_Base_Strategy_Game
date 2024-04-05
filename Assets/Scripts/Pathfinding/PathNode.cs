using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode
{
    public GridPosition gridPosition {  get; private set; }
    public int gCost;
    public int hCost;
    public int fCost {  get; private set; }
    public bool isWalkable = true;
    public PathNode cameFromPathNode;

    public PathNode(GridPosition gridPosition)
    {
        this.gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return gridPosition.ToString();
    }
    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }
    public void ResetCameFromPathNode()
    {
        cameFromPathNode = null;
    }
}
