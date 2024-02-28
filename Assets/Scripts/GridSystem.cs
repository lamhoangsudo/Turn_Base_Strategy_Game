using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    public int width {  get; private set; }
    public int height { get; private set; }
    private float cellSize;
    private GridObject[,] gridObjects;

    public GridSystem(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        gridObjects = new GridObject[width, height];
        for(int x = 0; x < width; x++)
        {
            for(int z = 0; z < height; z++)
            {
                gridObjects[x, z] = new GridObject(new GridPosition(x, z), this);
            }
        }
    }
    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition.x, 0, gridPosition.z) * cellSize;
    }
    public GridPosition GetGridPosition(Vector3 position)
    {
        return new GridPosition(Mathf.RoundToInt(position.x / cellSize), Mathf.RoundToInt(position.z / cellSize));
    }
    public void CreateGridDebugObject(Transform gridDebugObj)
    {
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Transform gridDebugTransform = GameObject.Instantiate(gridDebugObj, GetWorldPosition(gridPosition), Quaternion.identity);
                gridDebugTransform.GetComponent<GridDebugObject>().SetGridObject(GetGridObject(gridPosition));
            }
        }
    }
    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjects[gridPosition.x, gridPosition.z];
    }
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition.x >= 0 
            && gridPosition.z >= 0 
            && gridPosition.x < width 
            && gridPosition.z < height;
    }
}
