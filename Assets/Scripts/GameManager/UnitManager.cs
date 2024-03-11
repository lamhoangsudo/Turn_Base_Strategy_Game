using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public List<Unit> unitlist {  get; private set; }
    public List<Unit> friendlyUnitlist {  get; private set; }
    public List<Unit> enemyUnitlist {  get; private set; }
    public static UnitManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        unitlist = new List<Unit>();
        friendlyUnitlist = new List<Unit>();
        enemyUnitlist = new List<Unit>();
    }
    private void Start()
    {
        Unit.OnUnitSwapned += Unit_OnUnitSwapned;
        Unit.OnUnitDead += Unit_OnUnitDead;
    }

    private void Unit_OnUnitDead(object sender, System.EventArgs e)
    {
        Unit unit = sender as Unit;
        unitlist.Remove(unit);
        if (unit.IsPlayer())
        {
            friendlyUnitlist.Remove(unit);
        }
        else
        {
            enemyUnitlist.Remove(unit);
        }
    }

    private void Unit_OnUnitSwapned(object sender, System.EventArgs e)
    {
        Unit unit = sender as Unit;
        unitlist.Add(unit);
        if(unit.IsPlayer())
        {
            friendlyUnitlist.Add(unit);
        }
        else
        {
            enemyUnitlist.Add(unit);
        }
    }
}
