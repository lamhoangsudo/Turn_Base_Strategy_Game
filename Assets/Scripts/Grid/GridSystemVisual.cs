using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    [SerializeField] private Transform prefabVisual;
    private Dictionary<GridPosition, GridSystemVisualSingle> gridVisualPositions;
    public static GridSystemVisual Instance;
    private List<GridPosition> gridPositions;
    private Unit selectUnit;
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
    }

    private void Instance_OnUpdateGripVisual(object sender, System.EventArgs e)
    {
        UpdateVisual(selectUnit.selectAction.GetListValidGridPosition());
    }

    private void Instance_OnSelectActionChange(object sender, BaseAction e)
    {
        UpdateVisual(selectUnit.selectAction.GetListValidGridPosition());
    }

    private void Instance_OnSelectUnitChange(object sender, Unit selectUnit)
    {
        this.selectUnit = selectUnit;
    }
    private void Update()
    {
        if (selectUnit != null && selectUnit.selectAction != null && selectUnit.selectAction.IsActive() == true)
        {
            UpdateVisual(selectUnit.selectAction.GetListValidGridPosition());
        }
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
            gridVisualPositions[gridPosition].Show();
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
}
