using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonActionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;
    [SerializeField] private Transform visualSelect;
    private BaseAction baseAction;
    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
    }
    private void Awake()
    {
        visualSelect.gameObject.SetActive(false);
    }
    private void Start()
    {
        if (baseAction != null)
        {
            UpdateText();
            button.onClick.AddListener(() =>
            {
                UnitActionSystem.Instance.SetSelectAction(baseAction);
            });
        }
    }
    private void UpdateText()
    {
        text.SetText(baseAction.GetNameAction());
    }
    public void UpdateBtnSelectVisual(BaseAction selectAction)
    {
        visualSelect.gameObject.SetActive(baseAction == selectAction);
    }
}
