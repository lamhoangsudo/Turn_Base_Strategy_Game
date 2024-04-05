using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingUpdate : MonoBehaviour
{
    public void Start()
    {
        DestructibleCrate.OnAnyDestructibleCrate += UpdatePathfinding; 
    }

    private void UpdatePathfinding(object sender, GridPosition gp)
    {
        Pathfinding.instance.SetWalkable(gp);
    }
}
