using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSpawn : MonoBehaviour
{
    [SerializeField] private Transform ragdollPrefap;
    [SerializeField] private Transform unitRootBone;
    private HeathSystem heathSystem;
    private void Start()
    {
        heathSystem = GetComponent<HeathSystem>();
        heathSystem.OnDead += HeathSystem_OnDead;
    }

    private void HeathSystem_OnDead(object sender, System.EventArgs e)
    {
        Transform ragdoll = Instantiate(ragdollPrefap, transform.position, transform.rotation);
        ragdoll.GetComponent<RagdollAction>().SetUp(unitRootBone);
    }
}
