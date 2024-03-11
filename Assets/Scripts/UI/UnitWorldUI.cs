using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI actionPointText;
    [SerializeField] private Image bar;
    [SerializeField] private Unit unit;
    [SerializeField] private HeathSystem heathSystem;
    private void Start()
    {
        unit.OnUnitActionPointChange += Unit_OnUnitActionPointChange;
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        heathSystem.OnDamage += HeathSystem_OnDamage;
        UpdateActionPointText();
        UpdateHealthBar();
    }

    private void HeathSystem_OnDamage(object sender, System.EventArgs e)
    {
        UpdateHealthBar();
    }

    private void TurnSystem_OnTurnChange(object sender, System.EventArgs e)
    {
        if (TurnSystem.Instance.turnNumber != 0)
        {
            if ((unit.IsPlayer() == true && TurnSystem.Instance.isPlayerTurn == true) || (unit.IsPlayer() == false && TurnSystem.Instance.isPlayerTurn == false))
            {
                Unit.OnAnyActionPointChange += Unit_OnAnyActionPointChange;
            }
            else
            {
                Unit.OnAnyActionPointChange -= Unit_OnAnyActionPointChange;
            }
        }
        else
        {
            UpdateActionPointText();
        }
    }

    private void Unit_OnAnyActionPointChange(object sender, int e)
    {
        UpdateActionPointText();
    }

    private void Unit_OnUnitActionPointChange(object sender, System.EventArgs e)
    {
        UpdateActionPointText();
    }

    private void UpdateActionPointText()
    {
        actionPointText.SetText(unit.GetActionPoint().ToString());
    }
    private void UpdateHealthBar()
    {
        bar.fillAmount = heathSystem.GetHealthNormalize();
    }
}
