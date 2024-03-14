using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    private const int STRAIGHT_COST = 10;
    private const int DIAGONALLY_COST = 14;
    [SerializeField] private Transform gridDebugPrefab;
    private GridSystem<PathNode> gridSystem;
    private int width;
    private int height;
    private float cellSize;
    public static Pathfinding instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        gridSystem = new GridSystem<PathNode>(10, 10, 2f, (GridPosition p, GridSystem<PathNode> s) => new PathNode(p));
        gridSystem.CreateGridDebugObject(gridDebugPrefab);
    }
    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        List<PathNode> openNode = new();
        List<PathNode> closeNode = new();
        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openNode.Add(startNode);
        for (int x = 0; x < gridSystem.width; x++)
        {
            for (int z = 0; z < gridSystem.height; z++)
            {
                PathNode pathNode = gridSystem.GetGridObject(new GridPosition(x, z));
                pathNode.gCost = int.MaxValue;
                pathNode.hCost = 0;
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }
        startNode.gCost = 0;
        startNode.hCost = CalculateDistance(startGridPosition, endGridPosition);
        startNode.CalculateFCost();
        while (openNode.Count > 0)
        {
            PathNode currentNode = GetLowestFcostPathNode(openNode);
            if (currentNode == endNode)
            {
                return CalculatePath(endNode);
            }
            else
            {
                openNode.Remove(currentNode);
                closeNode.Add(currentNode);
                foreach (PathNode neighbourNode in GetNeighborList(currentNode))
                {
                    if(closeNode.Contains(neighbourNode))
                    {
                        continue;
                    }
                    int tentativeGCost = currentNode.gCost + CalculateDistance(currentNode.gridPosition, endGridPosition);
                    if(tentativeGCost < neighbourNode.gCost)
                    {
                        neighbourNode.cameFromPathNode = currentNode;
                        neighbourNode.gCost = tentativeGCost;
                        neighbourNode.hCost = CalculateDistance(neighbourNode.gridPosition, endGridPosition);
                        neighbourNode.CalculateFCost();
                        if (!openNode.Contains(neighbourNode))
                        {
                            openNode.Add(neighbourNode);
                        }
                    }
                }
            }
        }
        //no path found
        return null;
    }
    public int CalculateDistance(GridPosition a, GridPosition b)
    {
        GridPosition gridPositionDistance = a - b;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remaining = Mathf.Abs(xDistance - zDistance);
        return DIAGONALLY_COST * Mathf.Min(xDistance, zDistance) + STRAIGHT_COST * remaining;
    }
    private PathNode GetLowestFcostPathNode(List<PathNode> nodeList)
    {
        PathNode lowestFCostPathNode = nodeList[0];
        foreach (PathNode pathNode in nodeList)
        {
            if (pathNode.fCost < lowestFCostPathNode.fCost)
            {
                lowestFCostPathNode = pathNode;
            }
        }
        return lowestFCostPathNode;
    }
    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }
    private List<PathNode> GetNeighborList(PathNode currentNode)
    {
        List<PathNode> neighborList = new();
        GridPosition currentPosition = currentNode.gridPosition;
        if (currentPosition.x >= 0)
        {
            //Left
            neighborList.Add(GetNode(currentPosition.x - 1, currentPosition.z + 0));
            if (currentPosition.z >= 0)
            {
                //LeftDown
                neighborList.Add(GetNode(currentPosition.x - 1, currentPosition.z - 1));
            }
            if (currentPosition.z < gridSystem.height)
            {
                //LeftUp
                neighborList.Add(GetNode(currentPosition.x - 1, currentPosition.z + 1));
            }
        }
        if (currentPosition.x < gridSystem.width)
        {
            //Right
            neighborList.Add(GetNode(currentPosition.x + 1, currentPosition.z + 0));
            if (currentPosition.z >= 0)
            {
                //RightDown
                neighborList.Add(GetNode(currentPosition.x + 1, currentPosition.z - 1));
            }
            if (currentPosition.z < gridSystem.height)
            {
                //RightUp
                neighborList.Add(GetNode(currentPosition.x + 1, currentPosition.z + 1));
            }

        }
        return neighborList;
    }
    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.cameFromPathNode != null)
        {
            pathNodeList.Add(currentNode.cameFromPathNode);
            currentNode = currentNode.cameFromPathNode;
        }
        pathNodeList.Reverse();
        List<GridPosition> pathList = new();
        foreach (PathNode pathNode in pathNodeList)
        {
            pathList.Add(pathNode.gridPosition);
        }
        return pathList;
    }
}
