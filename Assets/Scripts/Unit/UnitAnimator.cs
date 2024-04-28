using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefap;
    [SerializeField] private Transform shootPosition;
    [SerializeField] private Transform rifle;
    [SerializeField] private Transform sword;
    private enum EquipmentType
    {
        Rifle,
        Sword
    }
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
        if (TryGetComponent(out SwordAction swordAction))
        {
            swordAction.OnSwordActionStart += SwordAction_OnSwordActionStart;
            swordAction.OnSwordActionComplete += SwordAction_OnSwordActionComplete;
        }
    }
    private void Start()
    {
        setEquipmentType(EquipmentType.Rifle);
    }
    private void SwordAction_OnSwordActionComplete(object sender, EventArgs e)
    {
        setEquipmentType(EquipmentType.Rifle);
    }

    private void SwordAction_OnSwordActionStart(object sender, EventArgs e)
    {
        setEquipmentType(EquipmentType.Sword);
        animator.SetTrigger("IsSlash");
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
    private void setEquipmentType(EquipmentType equipment)
    {
        switch (equipment)
        {
            case EquipmentType.Rifle:
                rifle.gameObject.SetActive(true);
                sword.gameObject.SetActive(false);
                break;
            case EquipmentType.Sword:
                rifle.gameObject.SetActive(false);
                sword.gameObject.SetActive(true);
                break;
        }
    }
}
