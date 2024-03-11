using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelectVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;
    private void Awake()
    {

    }
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectUnitChange += Instance_OnSelectUnitChange;
        UpdateVisual(null);
    }

    private void Instance_OnSelectUnitChange(object sender, Unit selectUnit)
    {
        UpdateVisual(selectUnit);
    }
    private void UpdateVisual(Unit selectUnit)
    {
        if(unit == selectUnit)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectUnitChange -= Instance_OnSelectUnitChange;
    }
}
