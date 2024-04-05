using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleCrate : MonoBehaviour
{
    public static EventHandler<GridPosition> OnAnyDestructibleCrate;
    public void Damager()
    {
        OnAnyDestructibleCrate?.Invoke(this, LevelGrid.Instance.GetGridPosition(transform.position));
        Destroy(gameObject);
    }
}
