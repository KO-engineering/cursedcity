using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NPCRagdoll : MonoBehaviour
{
    public UnityEvent OnActivate;
    public Rigidbody rb;
    public float force = 0.1f;

    Transform player;
    
    void Start() {
        player = PlayerControllerTesting.Instance.transform;
    }
    
    public void StartRagdoll()
    {
        OnActivate?.Invoke();
        int NPCdead = LayerMask.NameToLayer("NPCDead");
        gameObject.layer = NPCdead;
        Vector3 directiontoTarget = player.position - transform.position;
        directiontoTarget.Normalize();

        rb.isKinematic = true;
        rb.useGravity = true;
        rb.AddForce(-directiontoTarget * force, ForceMode.Impulse);

        foreach (Transform t in transform)
        {
            if(t.TryGetComponent<Rigidbody>(out Rigidbody r))
            {
                r.velocity = Vector3.zero;
            }
        }
    }
}
