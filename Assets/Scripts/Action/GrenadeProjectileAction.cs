using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrenadeProjectileAction : MonoBehaviour
{
    private Vector3 tagetPosition;
    private Action OnThrowActionComplete;
    public void SetUp(Vector3 tagetPosition, Action OnThrowActionComplete)
    {
        this.tagetPosition = tagetPosition;
        this.OnThrowActionComplete = OnThrowActionComplete;
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, tagetPosition, 10 * Time.deltaTime);
        if (Vector3.Distance(transform.position, tagetPosition) <= .2f)
        {
            Physics.OverlapSphere(transform.position, 8f).ToList().ForEach(collider =>
            {
                if (collider.TryGetComponent(out Unit unit))
                {
                    unit.DamageUnit(30);
                }
            });
            OnThrowActionComplete();
            Destroy(gameObject);
        }
    }
}
