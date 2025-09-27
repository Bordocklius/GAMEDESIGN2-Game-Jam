using UnityEngine;
using System.Collections.Generic;

public class ForceFieldZone : MonoBehaviour
{
    [SerializeField] private Transform forceOrigin; // Usually cannon or force field center
    [SerializeField] private float forceStrength = 15f;
    [SerializeField] private LayerMask affectedLayers;

    private List<Rigidbody> trackedRigidbodies = new List<Rigidbody>();

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & affectedLayers) != 0)
        {
            Rigidbody rb = other.attachedRigidbody;
            if (rb != null && !trackedRigidbodies.Contains(rb))
            {
                trackedRigidbodies.Add(rb);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rb = other.attachedRigidbody;
        if (rb != null && trackedRigidbodies.Contains(rb))
        {
            trackedRigidbodies.Remove(rb);
        }
    }

    public void PushObjects()
    {
        foreach (Rigidbody rb in trackedRigidbodies)
        {
            if (rb != null)
            {
                Vector3 dir = (rb.position - forceOrigin.position).normalized;
                rb.AddForce(dir * forceStrength, ForceMode.Impulse);
            }
        }
    }
}