using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform btnActionPrefab;
    [SerializeField] private Transform btnActionContainer;
    [SerializeField] private TextMeshProUGUI actionPointText;
    private List<BaseAction> baseActions;
    private List<ButtonActionUI> btnActionUIS;
    private void Start()
    {
        actionPointText.gameObject.SetActive(false);
        baseActions = new List<BaseAction>();
        btnActionUIS = new List<ButtonActionUI>();
        UnitActionSystem.Instance.OnSelectUnitChange += Instance_OnSelectUnitChange;
        UnitActionSystem.Instance.OnSelectActionChange += Instance_OnSelectActionChange;
        UnitActionSystem.Instance.OnActionPointChange += Instance_OnActionPointChange;
        Unit.OnAnyActionPointChange += Unit_OnAnyActionPointChange;
    }

    private void Unit_OnAnyActionPointChange(object sender, int maxActionPoint)
    {
        UpdateActionPointText(maxActionPoint);
    }

    private void Instance_OnActionPointChange(object sender, int actionPoint)
    {
        UpdateActionPointText(actionPoint);
    }

    private void Instance_OnSelectActionChange(object sender, BaseAction selectAction)
    {
        UpdateSelectVisual(selectAction);
    }

    private void Instance_OnSelectUnitChange(object sender, Unit selectUnit)
    {
        ResetActionButton();
        baseActions = selectUnit.baseActions.ToList();
        CreateActionButton();
        UpdateActionPointText(selectUnit.GetActionPoint());
    }

    private void CreateActionButton()
    {
        foreach (BaseAction baseAction in baseActions)
        {
            Transform button = Instantiate(btnActionPrefab, btnActionContainer);
            ButtonActionUI btnUI =  button.GetComponent<ButtonActionUI>();
            btnUI.SetBaseAction(baseAction);
            btnActionUIS.Add(btnUI);
        }
    }
    private void ResetActionButton()
    {
        foreach(Transform btnAction in btnActionContainer)
        {
            Destroy(btnAction.gameObject);
        }
        baseActions.Clear();
        btnActionUIS.Clear();
    }
    private void UpdateSelectVisual(BaseAction selectAction)
    {
        foreach(ButtonActionUI btnUI in btnActionUIS)
        {
            btnUI.UpdateBtnSelectVisual(selectAction);
        }
    }
    private void UpdateActionPointText(int actionPoint)
    {
        if(actionPointText.gameObject.activeSelf == false)
        {
            actionPointText.gameObject.SetActive(true);
        }
        actionPointText.SetText("Action Point: " + actionPoint.ToString());
    }
}
