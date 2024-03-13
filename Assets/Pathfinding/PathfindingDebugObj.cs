using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PathfindingDebugObj : GridDebugObject
{
    [SerializeField] private TextMeshPro gcost;
    [SerializeField] private TextMeshPro fcost;
    [SerializeField] private TextMeshPro hcost;
    private PathNode pathNode;
    public override void SetGridObject(object gridObject)
    {
        base.SetGridObject(gridObject);
        pathNode = (PathNode)gridObject;
    }
    protected override void Update()
    {
        base.Update();
        gcost.text = pathNode.gCost.ToString();
        fcost.text = pathNode.fCost.ToString();
        hcost.text = pathNode.hCost.ToString();
    }
}
