using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    private Vector3 tagetPosition;
    private const float tagetPositionDistance = 0.1f;
    private GridPosition gridPosition;
    private void Awake()
    {
        tagetPosition = transform.position;
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);
    }

    private void Update()
    {
        HandleMovement();
    }
    private void HandleMovement()
    {
        
        Vector3 moveDir = (tagetPosition - this.transform.position).normalized;
        if (Vector3.Distance(tagetPosition, this.transform.position) > tagetPositionDistance)
        {
            unitAnimator.SetBool("IsWalking", true);
            this.transform.position += moveSpeed * Time.deltaTime * moveDir;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * rotationSpeed);
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
        }
        GridPosition currentGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if(currentGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMoveGridPosition(this, gridPosition, currentGridPosition);
            gridPosition = currentGridPosition;
        }
    }
    public void SetTagetPosition(Vector3 tagetPosition)
    {
        this.tagetPosition = tagetPosition;
    }
}
