using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance;
    private Unit selectUnit;
    [SerializeField] private LayerMask mouseLayer;
    public event EventHandler<Unit> OnSelectUnitChange;
    public bool isBusy {  get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Update()
    {
        if (isBusy) return;
        if (Input.GetMouseButtonDown(0))
        {
            if (HandleUnitSelection()) return;
            if (selectUnit != null)
            {
                GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.Instance.GetTagetPosititon());
                if (selectUnit.MoveAction().IsValidGridPosition(gridPosition))
                {
                    isBusy = true;
                    selectUnit.MoveAction().SetTagetPosition(() => { isBusy = false; },gridPosition);
                }
            };
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (selectUnit != null)
            {
                isBusy = true;
                selectUnit.SpinAction().Spin(() => { isBusy = false; });
            };
        }
    }
    private bool HandleUnitSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, float.MaxValue, mouseLayer.value))
        {
            if (hitInfo.transform.TryGetComponent<Unit>(out Unit unit))
            {
                this.selectUnit = unit;
                OnSelectUnitChange?.Invoke(this, selectUnit);
                return true;
            }
        }
        return false;
    }
}
