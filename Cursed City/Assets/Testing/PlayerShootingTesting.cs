using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class PlayerShootingTesting : Singleton<PlayerShootingTesting>
{
    [SerializeField] float fireRate = 0.1f;
    [SerializeField] GameObject decalPrefab;
    [SerializeField] GameObject bulletTrail;
    [SerializeField] ParticleSystem gunShotFX;

    [HorizontalLine]

    [SerializeField] Rig aimRig;
    [SerializeField] Rig armRig;
    [SerializeField] RigBuilder builder;
    [SerializeField] GameObject gun;
    [SerializeField] Transform gunEnd;
    [SerializeField] Transform targetPoint;
    [SerializeField] LayerMask layerMask;
    [SerializeField] Camera cam;

    [Header("Smoothing Settings")]
    [SerializeField] float smoothingSpeed = 5f;

    [HorizontalLine]

    RaycastHit hit;
    float nextTimeToFire = 0;

    private Coroutine aimRigCoroutine;

    void Start()
    {
        aimRig.weight = 0;
        armRig.weight = 0;
        builder.enabled = false;
    }

    public void ActivateAimRig(bool activate)
    {
        if (aimRigCoroutine != null)
        {
            StopCoroutine(aimRigCoroutine);  // Ensure multiple transitions don't overlap
        }

        aimRigCoroutine = StartCoroutine(SmoothAimRigWeight(activate));
    }

    private IEnumerator SmoothAimRigWeight(bool activate)
    {
        gun.SetActive(activate);
        builder.enabled = activate;

        float targetWeight = activate ? 1f : 0f;
        float currentWeight = aimRig.weight;

        while (!Mathf.Approximately(aimRig.weight, targetWeight))
        {
            currentWeight = Mathf.MoveTowards(currentWeight, targetWeight, smoothingSpeed * Time.deltaTime);
            aimRig.weight = currentWeight;
            armRig.weight = currentWeight;
            yield return null;
        }

        aimRig.weight = targetWeight;
        armRig.weight = targetWeight;
    }

    void Update()
    {
        if (aimRig.weight == 0)
        {
            gun.SetActive(false);
            bulletTrail.SetActive(false);
            return;
        }

        Ray screenRay = cam.ScreenPointToRay(Input.mousePosition);
        Vector3 gunEndPos = gunEnd.position;
        Vector3 direction = (screenRay.origin + screenRay.direction * 1000) - gunEndPos;

        if (Physics.Raycast(gunEndPos, direction, out hit, 1000, layerMask))
        {
            targetPoint.position = hit.point;
        }

        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + fireRate;
            gunShotFX.Emit(1);
            bulletTrail.SetActive(true);
            PlayerControllerTesting.Instance.animator.SetBool("Shooting", true);
            Shoot(targetPoint.position);
        }
        else
        {
            PlayerControllerTesting.Instance.animator.SetBool("Shooting", false);
            bulletTrail.SetActive(false);
        }
    }

    void Shoot(Vector3 point)
    {
        Instantiate(decalPrefab, point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
    }
}
