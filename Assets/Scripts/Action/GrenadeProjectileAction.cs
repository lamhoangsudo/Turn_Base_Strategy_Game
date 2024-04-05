using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GrenadeProjectileAction : MonoBehaviour
{
    private Vector3 tagetPosition;
    private Action OnThrowActionComplete;
    [SerializeField] private Transform explosionEffect;
    [SerializeField] private Transform grenadeTrail;
    [SerializeField] private AnimationCurve curve;
    private float distance;
    private float height;
    public static event EventHandler OnAnyGrenadeexploded;
    public void SetUp(Vector3 tagetPosition, Action OnThrowActionComplete)
    {
        this.tagetPosition = tagetPosition;
        this.OnThrowActionComplete = OnThrowActionComplete;
        distance = Vector3.Distance(transform.position, tagetPosition);
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, tagetPosition, 10 * Time.deltaTime);
        float maxHeight = distance / 4f;
        height = curve.Evaluate(1 - (Vector3.Distance(transform.position, tagetPosition) / distance));
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
        if (Vector3.Distance(transform.position, tagetPosition) <= .2f)
        {
            Physics.OverlapSphere(transform.position, 8f).ToList().ForEach(collider =>
            {
                if (collider.TryGetComponent(out Unit unit))
                {
                    unit.DamageUnit(30);
                }
                if (collider.TryGetComponent(out DestructibleCrate destructibleCrate))
                {
                    destructibleCrate.Damager();
                }
            });
            OnAnyGrenadeexploded?.Invoke(this, EventArgs.Empty);
            grenadeTrail.SetParent(null);
            Instantiate(explosionEffect, transform.position + Vector3.up * 1f, Quaternion.identity);
            OnThrowActionComplete();
            Destroy(gameObject);
        }
    }
}
