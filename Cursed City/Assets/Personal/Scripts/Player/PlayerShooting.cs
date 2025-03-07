using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerShooting : Singleton<PlayerShooting>
{
    [SerializeField] Rig aimRig;
    [SerializeField] Transform gunEnd;
    [SerializeField] Transform targetPoint;
    [SerializeField] Transform targetSpacePoint;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Camera cam;
    
    RaycastHit hit;

    void Start()
    {
        ActivateAimRig(false);
    }

    public void ActivateAimRig(bool activate)
    {
        aimRig.weight = activate? 1 : 0;
    }

    void Update()
    {
        Ray screenRay = cam.ScreenPointToRay(Input.mousePosition);

        Vector3 gunEndPos = gunEnd.position;

        Vector3 direction  = (screenRay.origin + screenRay.direction * 1000) - gunEndPos;

        if(Physics.Raycast(gunEndPos, direction, out hit, 1000, layerMask))
        {
            targetPoint.position = hit.point;
        }
        else
        {
            targetPoint.position = targetSpacePoint.position;
        }
    }
}
