using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public Transform player;
    private Driving currentDrivingScript;

    void Update()
    {
        // Find the current active car
        Driving[] allDrivingScripts = GameObject.FindObjectsOfType<Driving>();

        currentDrivingScript = null;

        foreach (Driving d in allDrivingScripts)
        {
            if (d.isDriving)
            {
                currentDrivingScript = d;
                break;
            }
        }

        // Follow player if not driving, else follow car
        if (currentDrivingScript == null)
        {
            transform.position = new Vector3(player.position.x, transform.position.y, player.position.z);
        }
        else
        {
            Transform carTransform = currentDrivingScript.transform;
            transform.position = new Vector3(carTransform.position.x, transform.position.y, carTransform.position.z);
        }
    }
}