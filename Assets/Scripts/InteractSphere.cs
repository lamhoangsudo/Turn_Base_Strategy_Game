using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class InteractSphere : MonoBehaviour, IInteract
{
    [SerializeField] private MeshRenderer sphere;
    [SerializeField] private Material green;
    [SerializeField] private Material red;
    private bool isGreen;
    private Action OnInteractComplete;
    private float timer;
    private bool isActive;
    private GridPosition gridPosition;
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddInteractObjectAtGridPosition(gridPosition, this);
        isGreen = false;
    }
    private void Update()
    {
        if (!isActive) return;
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            isActive = false;
            OnInteractComplete();
        }
    }
    public void Interact(Action OnInteractionComplete)
    {
        this.OnInteractComplete = OnInteractionComplete;
        isActive = true;
        timer = 0.5f;
        if (isGreen)
        {
            SetRed();
        }
        else
        {
            SetGreen();
        }
    }

    private void SetGreen()
    {
        isGreen = true;
        sphere.material = green;
    }
    private void SetRed() 
    {
        isGreen = false;
        sphere.material = red;
    }
}
