using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Driving : Interactable
{
    public GameObject player;
    public GameObject playerCamera;
    public bool isDriving = false;
    public UnityEvent OnExitState;
    public static Transform currentCar;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        playerCamera = GameObject.FindWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Z)){
            exitDrivingState();
        }
    }
    public void drivingState(){
        player.SetActive(false);
        playerCamera.SetActive(false);
        ShowOutline(false);
        isDriving = true;
        currentCar = this.transform;
    }
    public void exitDrivingState(){
        print("Exit");
        player.SetActive(true);
        playerCamera.SetActive(true);
        OnExitState?.Invoke();
        isDriving = false;
    }
}
