using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefap;
    [SerializeField] private Transform shootPosition;
    private void Awake()
    {
        if (TryGetComponent(out MoveAction moveAction))
        {
            moveAction.OnStartAction += MoveAction_OnStartAction;
            moveAction.OnStopAction += MoveAction_OnStopAction;
        }
        if (TryGetComponent(out ShootAction shootAction))
        {
            shootAction.OnShootAction += ShootAction_OnShootAction; ;
        }
    }

    private void ShootAction_OnShootAction(object sender, ShootAction.SetUpBulletProjectile e)
    {
        animator.SetTrigger("IsShooting");
        Transform bullet = Instantiate(bulletProjectilePrefap, shootPosition.position, Quaternion.identity);
        Vector3 tagetPosition = LevelGrid.Instance.GetGridPosition(e.tagetUnit.GetGridPosition());
        tagetPosition.y = bullet.transform.position.y;
        bullet.GetComponent<BullteProjectileAction>().SetUp(tagetPosition);
    }

    private void MoveAction_OnStopAction(object sender, System.EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void MoveAction_OnStartAction(object sender, System.EventArgs e)
    {
        animator.SetBool("IsWalking", true);        
    }
}
