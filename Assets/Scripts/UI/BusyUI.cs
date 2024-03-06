using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BusyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Color[] color;
    private void Start()
    {
        gameObject.SetActive(false);
        UnitActionSystem.Instance.OnBusyChange += Instance_OnBusyChange;
        TurnSystem.Instance.OnTurnChange += Instance_OnTurnChange;
    }

    private void Instance_OnTurnChange(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.isPlayerTurn)
        {
            text.SetText("Enemy Turn");
            text.color = color[1];
        }
        else
        {
            text.SetText("BUSY");
            text.color = color[0];
        }
        gameObject.SetActive(!TurnSystem.Instance.isPlayerTurn);
    }

        private void Instance_OnBusyChange(object sender, bool isBusy)
    {
        gameObject.SetActive(isBusy);
    }
}
