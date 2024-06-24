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
        if (InputManager.Instance.IsMouseButtonDown())
        {
            if (selectUnit != null
                && selectAction != null
                && selectAction.IsValidGridPosition(LevelGrid.Instance.GetGridPosition(InputManager.Instance.GetMouseWorldPosition(out _))))
            {
                if (selectUnit.TryToSpendActionPoint(selectAction, false))
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
                        selectUnit.TryToSpendActionPoint(selectAction, true);
                    }
                }
            }
        }
    }
    private bool HandleUnitSelection()
    {
        if (InputManager.Instance.IsMouseButtonDown())
        {
            InputManager.Instance.GetMouseWorldPosition(out RaycastHit hitInfo);
            try
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
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
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
