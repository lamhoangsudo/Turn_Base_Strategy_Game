using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance;
    private Unit selectUnit;
    private BaseAction selectAction;
    [SerializeField] private LayerMask mouseLayer;
    public event EventHandler<Unit> OnSelectUnitChange;
    public bool isBusy { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Update()
    {
        if (isBusy) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (HandleUnitSelection()) return;
        HandleActionSelection();
    }
    private void HandleActionSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            /*if (unit != null)
            {
                GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetTagetPosititon());
                if (unit.MoveAction().IsValidGridPosition(gridPosition))
                {
                    isBusy = true;
                    unit.MoveAction().SetTagetPosition(() => { isBusy = false; }, gridPosition);
                }
            };*/
            if (selectUnit != null && selectAction != null)
            {
                selectAction.GetAction(() => { isBusy = false; }, selectUnit);
            }
        }
        /*if (Input.GetMouseButtonDown(1))
        {
            if (unit != null)
            {
                isBusy = true;
                unit.SpinAction().Spin(() => { isBusy = false; });
            };
        }*/
    }
    private bool HandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, mouseLayer.value))
            {
                if (hitInfo.transform.TryGetComponent<Unit>(out Unit unit))
                {
                    this.selectUnit = unit;
                    this.selectAction = unit.MoveAction();
                    OnSelectUnitChange?.Invoke(this, selectUnit);
                    return true;
                }
            }
        }
        return false;
    }
    public void SetSelectAction(BaseAction action)
    {
        if (selectUnit != null)
        {
            selectAction = action;
        }
    }
}
