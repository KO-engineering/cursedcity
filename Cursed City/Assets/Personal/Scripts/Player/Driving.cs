using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Driving : Interactable
{
    public GameObject player;
    public GameObject playerCamera;
    public UnityEvent OnExitState;
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
    }
    public void exitDrivingState(){
        print("Exit");
        player.SetActive(true);
        playerCamera.SetActive(true);
        OnExitState?.Invoke();
    }
}
