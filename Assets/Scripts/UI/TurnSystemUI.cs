using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private Button endTurnBtn;
    private void Start()
    {
        UpdateTurnNumberText();
        endTurnBtn.onClick.AddListener(() =>
        {
            TurnSystem.Instance.IncreaseTurnNumber();
            UpdateTurnNumberText();
        });
    }
    private void UpdateTurnNumberText()
    {
        turnNumberText.SetText("TURN " + TurnSystem.Instance.GetTurnNumber().ToString());
    }
}
