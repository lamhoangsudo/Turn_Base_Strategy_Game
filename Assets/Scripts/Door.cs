using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Transform doorleft;
    [SerializeField] private Transform doorright;
    private GridPosition gridPosition;
    [SerializeField] private bool isOpen;
    [SerializeField] private Animator animator;
    private Action OnInteractComplete;
    private float timer;
    private bool isActive;
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddDoorAtGridPosition(gridPosition, this);
        if(isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
    private void Update()
    {
        if (!isActive) return;
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            isActive = false;
            OnInteractComplete();
        }
    }
    public void Interact(Action OnInteractComplete)
    {
        this.OnInteractComplete = OnInteractComplete;
        isActive = true;
        timer = 0.5f;
        isOpen = !isOpen;
        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
    private void OpenDoor()
    {
        isOpen =  true;
        animator.SetTrigger("Open");
        Pathfinding.instance.SetWalkable(gridPosition, isOpen);
    }
    private void CloseDoor()
    {
        isOpen = false;
        animator.SetTrigger("Close");
        Pathfinding.instance.SetWalkable(gridPosition, isOpen);
    }
}
