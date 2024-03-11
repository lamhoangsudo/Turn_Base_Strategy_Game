using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    public int turnNumber {  get; private set; }
    public static TurnSystem Instance {  get; private set; }
    public bool isPlayerTurn {  get; private set; }
    public event EventHandler OnTurnChange;
    private void Awake()
    {
        turnNumber = 0;
        Instance = this;
    }
    private void Start()
    {
        isPlayerTurn = true;
        OnTurnChange?.Invoke(this, EventArgs.Empty);
    }
    public void IncreaseTurnNumber()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;
        OnTurnChange?.Invoke(this, EventArgs.Empty);
    }
    public int GetTurnNumber()
    {
        return turnNumber;
    }
}
