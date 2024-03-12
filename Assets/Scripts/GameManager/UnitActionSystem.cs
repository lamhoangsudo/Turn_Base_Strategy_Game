using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance;
    private Unit selectUnit;
    private BaseAction selectAction;
    [SerializeField] private LayerMask mouseLayer;
    public event EventHandler<Unit> OnSelectUnitChange;
    public event EventHandler<BaseAction> OnSelectActionChange;
    public event EventHandler<bool> OnBusyChange;
    public event EventHandler<int> OnActionPointChange;
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
            if (selectUnit != null 
                && selectAction != null 
                && selectUnit.TryToSpendActionPoint(selectAction, false )
                && selectAction.IsValidGridPosition(LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetTagetPosititon())))
            {
                selectUnit.selectAction = selectAction;
                selectUnit.selectAction.GetAction(() => { OnBusyChange?.Invoke(this, false); isBusy = false; }, selectUnit);
                if (selectAction.IsActive())
                {
                    OnActionPointChange?.Invoke(this, selectUnit.GetActionPoint());
                    OnBusyChange?.Invoke(this, true);
                }
                else
                {
                    selectUnit.TryToSpendActionPoint(selectAction, false);
                }
            }
        }
    }
    private bool HandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, mouseLayer.value))
            {
                if (hitInfo.transform.TryGetComponent<Unit>(out Unit unit) && unit.IsPlayer() == true && TurnSystem.Instance.isPlayerTurn == true)
                {
                    this.selectUnit = unit;
                    this.selectAction = unit.GetAction<MoveAction>();
                    unit.selectAction = this.selectAction;
                    OnSelectUnitChange?.Invoke(this, selectUnit);
                    OnSelectActionChange?.Invoke(this, selectAction);
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
            selectUnit.selectAction = this.selectAction;
            OnSelectActionChange?.Invoke(this, action);
        }
    }
}
