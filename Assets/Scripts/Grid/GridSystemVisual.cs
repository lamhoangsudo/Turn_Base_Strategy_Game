using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static GridSystemVisual;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform prefabVisual;
    private Dictionary<GridPosition, GridSystemVisualSingle> gridVisualPositions;
    public static GridSystemVisual Instance;
    private List<GridPosition> gridPositions;
    private Unit selectUnit;
    [SerializeField] private List<GridVisualMaterial> materials;
    private Material gridMaterial;
    [Serializable]
    public struct GridVisualMaterial
    {
        public GridVisualType type;
        public Material material;
    }
    public enum GridVisualType
    {
        White,
        Blue,
        Red,
        RedSoft,
        Yellow,
    }
    private void Awake()
    {
        Instance = this;
        gridVisualPositions = new Dictionary<GridPosition, GridSystemVisualSingle>();
    }
    private void Start()
    {
        LevelGrid.Instance.GetWidthAndHeight(out int width, out int height);
        for (int x = 0; x < width; x++)
        {
            for (int z = 0; z < height; z++)
            {
                GridPosition gridPosition = new(x, z);
                GridSystemVisualSingle gridVisual = Instantiate(prefabVisual, LevelGrid.Instance.GetGridPosition(gridPosition), Quaternion.identity).GetComponent<GridSystemVisualSingle>();
                gridVisual.Hide();
                gridVisualPositions.Add(gridPosition, gridVisual);
            }
        }
        UnitActionSystem.Instance.OnSelectUnitChange += Instance_OnSelectUnitChange;
        UnitActionSystem.Instance.OnSelectActionChange += Instance_OnSelectActionChange;
        LevelGrid.Instance.OnUnitMoveGripPositonUpdate += Instance_OnUnitMoveGripPositonUpdate;
    }

    private void Instance_OnUnitMoveGripPositonUpdate(object sender, System.EventArgs e)
    {
        if (selectUnit != null)
        {
            UpdateVisual(selectUnit.selectAction.GetListValidGridPosition());
        }
    }

    private void Instance_OnSelectActionChange(object sender, BaseAction e)
    {
        SetMaterial();
        UpdateVisual(selectUnit.selectAction.GetListValidGridPosition());
    }

    private void Instance_OnSelectUnitChange(object sender, Unit selectUnit)
    {
        this.selectUnit = selectUnit;
    }
    private void HideAllGridVisual()
    {
        foreach (var gridVisualPosition in gridVisualPositions)
        {
            if (gridPositions.Count > 0)
            {
                foreach (GridPosition gridPosition in gridPositions)
                {
                    if (gridVisualPosition.Key != gridPosition && gridVisualPosition.Value.IsActive() == true)
                    {
                        gridVisualPosition.Value.Hide();
                    }
                }
            }
            else
            {
                gridVisualPosition.Value.Hide();
            }
        }
    }
    private void ShowListGridVisual()
    {
        foreach (GridPosition gridPosition in gridPositions)
        {
            gridVisualPositions[gridPosition].Show(gridMaterial);
        }
    }
    public void UpdateVisual(List<GridPosition> gridPositions)
    {
        this.gridPositions = gridPositions;
        HideAllGridVisual();
        if (gridPositions.Count > 0)
        {
            ShowListGridVisual();
        }
    }
    private Material GetMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualMaterial gridVisualMaterial in materials)
        {
            if (gridVisualMaterial.type == gridVisualType)
            {
                return gridVisualMaterial.material;
            }
        }
        return null;
    }
    private void SetMaterial()
    {
        GridVisualType gridVisualType;
        switch (selectUnit.selectAction)
        {
            default:
            case MoveAction:
                gridVisualType = GridVisualType.White;
                break;
            case ShootAction:
                gridVisualType = GridVisualType.Red;
                break;
            case SpinAction:
                gridVisualType = GridVisualType.Blue;
                break;
        }
        gridMaterial = GetMaterial(gridVisualType);
    }
}
