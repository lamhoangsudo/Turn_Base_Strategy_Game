using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveAction : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private int maxMoveDistance;
    private Vector3 tagetPosition;
    private const float tagetPositionDistance = 0.1f;
    private Unit unit;
    public bool unitMove {  get; private set; }
    private void Awake()
    {
        tagetPosition = transform.position;
    }
    private void Start()
    {
        unit = GetComponent<Unit>();    
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
            unitMove = true;
            this.transform.position += moveSpeed * Time.deltaTime * moveDir;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * rotationSpeed);
            transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
        }
        else
        {
            unitAnimator.SetBool("IsWalking", false);
            unitMove = false;
        }
    }
    public void SetTagetPosition(GridPosition tagetPosition)
    {
        this.tagetPosition = LevelGrid.Instance.GetGridPosition(tagetPosition);
    }
    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return GetListValidGridPosition().Contains(gridPosition);
    }
    public List<GridPosition> GetListValidGridPosition()
    {
        List<GridPosition> gridPositionsValid = new List<GridPosition>();
        GridPosition unitGridPosition = unit.GetGridPosition();
        for(int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition validGridPosition = unitGridPosition + new GridPosition(x, z);
                if (LevelGrid.Instance.IsValidGridPosition(validGridPosition) 
                    && validGridPosition != unitGridPosition 
                    && !LevelGrid.Instance.IsUnitOnGridPosition(validGridPosition))
                {
                    gridPositionsValid.Add(validGridPosition);
                };
            }
        }
        return gridPositionsValid;
    }
}
