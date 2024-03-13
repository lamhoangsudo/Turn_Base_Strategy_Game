using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    [SerializeField] private Transform gridDebugPrefab;
    private GridSystem<PathNode> gridSystem;
    private int width;
    private int height;
    private float cellSize;
    private void Awake()
    {
        gridSystem = new GridSystem<PathNode> (10, 10, 2f, (GridPosition p, GridSystem<PathNode> s) => new PathNode(p));
        gridSystem.CreateGridDebugObject(gridDebugPrefab);
    }
}
