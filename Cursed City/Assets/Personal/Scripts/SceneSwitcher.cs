using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneSwitcher : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C)){
            SwitchToScene("Car Customizer");
        }
        if(Input.GetKeyDown(KeyCode.P)){
            SwitchToScene("Player Customizer");
        }
    }
    public void SwitchToScene(string sceneName){
        SceneManager.LoadScene(sceneName);
    }
}
