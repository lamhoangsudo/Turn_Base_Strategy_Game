using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform btnActionPrefab;
    [SerializeField] private Transform btnActionContainer;
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectUnitChange += Instance_OnSelectUnitChange;
    }

    private void Instance_OnSelectUnitChange(object sender, Unit selectUnit)
    {
        ResetActionButton();
        CreateActionButton(selectUnit.baseActions);
    }

    private void CreateActionButton(BaseAction[] baseActions)
    {
        foreach (BaseAction baseAction in baseActions)
        {
            Instantiate(btnActionPrefab, btnActionContainer).GetComponent<ButtonActionUI>().SetBaseAction(baseAction);
        }
    }
    private void ResetActionButton()
    {
        foreach(Transform btnAction in btnActionContainer)
        {
            Destroy(btnAction.gameObject);
        }
    }
}
