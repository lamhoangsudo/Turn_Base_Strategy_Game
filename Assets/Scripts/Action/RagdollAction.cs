using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class RagdollAction : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;
    public void SetUp(Transform unitRootBone)
    {
        MatchAllChildTranform(unitRootBone, ragdollRootBone);
        Vector3 ramdomDir = new(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f));
        ApplyExplosionToRagdoll(ragdollRootBone, 300f, transform.position + ramdomDir, 10f, 300f, ForceMode.Force);
        transform.GetComponent<Rigidbody>().AddForce(transform.forward * 2f, ForceMode.Force);
    }
    private void MatchAllChildTranform(Transform unitRootBone, Transform ragdollRootBone)
    {
        foreach(Transform childUnitRootBone in unitRootBone)
        {
            Transform childRagdollRootBone = ragdollRootBone.Find(childUnitRootBone.name);
            if (childRagdollRootBone != null) 
            {
                childRagdollRootBone.position = childUnitRootBone.position;
                childRagdollRootBone.rotation = childUnitRootBone.rotation;
                MatchAllChildTranform(childUnitRootBone, childRagdollRootBone);
            };
        }
    }
    private void ApplyExplosionToRagdoll(Transform root, float force, Vector3 position, float radius, float upwardsModifier, ForceMode mode)
    {
        foreach(Transform child in root)
        {
            if(child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddExplosionForce(force, position, radius, upwardsModifier, mode);
            }
            ApplyExplosionToRagdoll(child, force, position, radius, upwardsModifier, mode);
        }
    }
}
