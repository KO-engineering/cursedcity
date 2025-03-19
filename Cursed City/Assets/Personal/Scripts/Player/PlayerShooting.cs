using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerShooting : Singleton<PlayerShooting>
{
    [SerializeField] float fireRate = 0.1f;
    [SerializeField] GameObject decalPrefab;
    [SerializeField] GameObject bulletTrail;
    [SerializeField] ParticleSystem gunShotFX;

    [HorizontalLine]

    [SerializeField] Rig aimRig;
    [SerializeField] Transform gunEnd;
    [SerializeField] Transform targetPoint;
    [SerializeField] Transform targetSpacePoint;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Camera cam;

    [SerializeField] float wallDistanceFloat = 2;

    RaycastHit hit;
    float nextTimeToFire = 0;
    bool floating;

    void Start()
    {
        ActivateAimRig(false);
    }

    public void ActivateAimRig(bool activate)
    {
        aimRig.weight = activate ? 1 : 0;
    }

    void Update()
    {
        if (aimRig.weight == 0) return;

        Ray screenRay = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 gunEndPos = gunEnd.position;
        Vector3 direction = (screenRay.origin + screenRay.direction * 1000) - gunEndPos;

        if (Physics.Raycast(gunEndPos, direction, out hit, 1000, layerMask) && Vector3.Distance(gunEndPos, hit.point) > wallDistanceFloat)
        {
            targetPoint.position = hit.point;
            floating = false;
        }
        else
        {
            targetPoint.position = targetSpacePoint.position;
            floating = true;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            gunShotFX.Emit(1);
            bulletTrail.SetActive(true);
            Shoot(targetPoint.position);
        }
        else
        {
            bulletTrail.SetActive(false);
        }
    }

    void Shoot(Vector3 point)
    {
        if (floating == false)
        {
            GameObject decal = Instantiate(decalPrefab, point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
        }
    }
}