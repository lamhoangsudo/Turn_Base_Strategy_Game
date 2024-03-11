using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BullteProjectileAction : MonoBehaviour
{
    private Vector3 tagetPosition;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private Transform trail;
    [SerializeField] private Transform bulletHit;
    private void Update()
    {
        BullteMove();
    }
    private void BullteMove()
    {
        Vector3 moveDir = (tagetPosition - transform.position).normalized;
        float distanceBeforeMoving = Vector3.Distance(tagetPosition, transform.position);
        transform.position += moveDir * bulletSpeed * Time.deltaTime;
        float distanceAfterMoving = Vector3.Distance(tagetPosition, transform.position);
        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = tagetPosition;
            trail.transform.parent = null;
            Instantiate(bulletHit, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
    public void SetUp(Vector3 tagetPosition)
    {
        this.tagetPosition = tagetPosition;
    }
}
