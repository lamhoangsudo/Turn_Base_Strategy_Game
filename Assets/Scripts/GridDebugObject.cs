using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    private GridObject gridObject;
    [SerializeField] private TextMeshPro text;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }
    private void Update()
    {
        text.SetText(gridObject.ToString());
    }
}
