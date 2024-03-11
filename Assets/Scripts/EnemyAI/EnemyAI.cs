using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private State state;
    private float timer;
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }
    private void Start()
    {
        state = State.WaitingForEnemyTurn;
        TurnSystem.Instance.OnTurnChange += Instance_OnTurnChange;
    }

    private void Instance_OnTurnChange(object sender, System.EventArgs e)
    {
        if (!TurnSystem.Instance.isPlayerTurn)
        {
            state = State.TakingTurn;
            timer = 2f;
        }
        else
        {
            state = State.WaitingForEnemyTurn;
        }
    }

    private void Update()
    {
        switch (state)
        {
            case State.WaitingForEnemyTurn: break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (TryTakeEnemyAIAction(SetStateTakeTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.IncreaseTurnNumber();
                        
                    }
                }
                break;
            case State.Busy: break;
        }
    }
    private void SetStateTakeTurn()
    {
        timer = 1f;
        state = State.TakingTurn;
    }
    private bool TryTakeEnemyAIAction(Action OnEnemyAIActionComplete)
    {
        foreach (Unit enemy in UnitManager.Instance.enemyUnitlist)
        {
            if (TryTakeEnemyAIAction(OnEnemyAIActionComplete, enemy))
            {
                return true;
            }
        }
        return false;
    }

    private bool TryTakeEnemyAIAction(Action OnEnemyAIActionComplete, Unit enemy)
    {
        EnemyAIAction enemyAIAction = null;
        BaseAction action = null;
        foreach(BaseAction baseAction in enemy.baseActions)
        {
            if (enemy.TryToSpendActionPoint(baseAction, false)) 
            {
                EnemyAIAction checkEnemyAIAction = baseAction.GetBestEnemyAIAction();
                if (enemyAIAction == null)
                {
                    enemyAIAction = checkEnemyAIAction;
                    action = baseAction;
                    enemy.TryToSpendActionPoint(baseAction, true);
                }
                else
                {
                    if(checkEnemyAIAction !=  null && checkEnemyAIAction.actionValue > enemyAIAction.actionValue)
                    {
                        enemyAIAction = checkEnemyAIAction;
                        action = baseAction;
                        enemy.TryToSpendActionPoint(baseAction, true);
                    }
                }
            }
        }
        enemy.selectAction = action;
        if(enemyAIAction != null 
            && action != null
            && enemy.TryToSpendActionPoint(enemy.selectAction, false))
        {
            enemy.selectAction.GetAction(OnEnemyAIActionComplete, enemy);
            return true;
        }
        else
        {
            return false;
        }
    }
}
