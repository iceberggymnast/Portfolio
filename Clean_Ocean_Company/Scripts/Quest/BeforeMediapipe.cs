using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeforeMediapipe : MonoBehaviour
{
    CapsuleCollider capsuleCollider;

    public Interaction_FishingNets interaction_FishingNets;

    private void Start()
    {
        interaction_FishingNets = transform.parent.GetComponent<Interaction_FishingNets>();
        capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !interaction_FishingNets.cleanStart)
        {
            interaction_FishingNets.InteractionEvent();
            capsuleCollider.enabled = false;
        }
    }
}
