using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSystem : MonoBehaviour
{
    [System.Serializable]
    public class TrafficLightElement
    {
        // Front side lights
        public GameObject redLightFront;
        public GameObject yellowLightFront;
        public GameObject greenLightFront;
        
        // Back side lights
        public GameObject redLightBack;
        public GameObject yellowLightBack;
        public GameObject greenLightBack;
        
        // Colliders
        public BoxCollider leftSideCollider;
        public BoxCollider rightSideCollider;
        public BoxCollider frontCollider;
        public BoxCollider backCollider;
        
        // Status variables
        public bool isGreen = false;
        public bool npcWaiting = false;
        public bool carInFront = false;
        public bool carInBack = false;
        
        public void SetRed()
        {
            // Activate red lights
            redLightFront.SetActive(true);
            redLightBack.SetActive(true);
            
            // Deactivate other lights
            yellowLightFront.SetActive(false);
            yellowLightBack.SetActive(false);
            greenLightFront.SetActive(false);
            greenLightBack.SetActive(false);
            
            isGreen = false;
        }
        
        public void SetYellow()
        {
            // Deactivate red lights
            redLightFront.SetActive(false);
            redLightBack.SetActive(false);
            
            // Activate yellow lights
            yellowLightFront.SetActive(true);
            yellowLightBack.SetActive(true);
            
            // Deactivate green lights
            greenLightFront.SetActive(false);
            greenLightBack.SetActive(false);
        }
        
        public void SetGreen()
        {
            // Deactivate red lights
            redLightFront.SetActive(false);
            redLightBack.SetActive(false);
            
            // Deactivate yellow lights
            yellowLightFront.SetActive(false);
            yellowLightBack.SetActive(false);
            
            // Activate green lights
            greenLightFront.SetActive(true);
            greenLightBack.SetActive(true);
            
            isGreen = true;
        }
    }
    
    public List<TrafficLightElement> trafficLights = new List<TrafficLightElement>();
    private Dictionary<BoxCollider, int> colliderToLightIndex = new Dictionary<BoxCollider, int>();
    
    // Start is called before the first frame update
    void Start()
    {
        // Initialize all traffic lights to red
        for (int i = 0; i < trafficLights.Count; i++)
        {
            TrafficLightElement light = trafficLights[i];
            light.SetRed();
            
            // Map colliders to their corresponding traffic light indices
            colliderToLightIndex[light.leftSideCollider] = i;
            colliderToLightIndex[light.rightSideCollider] = i;
            colliderToLightIndex[light.frontCollider] = i;
            colliderToLightIndex[light.backCollider] = i;
            
            // Start monitoring this traffic light
            StartCoroutine(ManageTrafficLight(i));
        }
    }
    
    // Update is called once per frame
    void Update()
    {
        // Main update loop - could add additional global control logic here
    }
    
    IEnumerator ManageTrafficLight(int index)
    {
        TrafficLightElement light = trafficLights[index];
        
        while (true)
        {
            // If no NPC is waiting, wait for 3 seconds and then change to green
            if (!light.npcWaiting)
            {
                yield return new WaitForSeconds(3.0f);
                if (!light.npcWaiting)
                {
                    light.SetGreen();
                    yield return new WaitForSeconds(10.0f); // Green for 10 seconds
                    light.SetYellow();
                    yield return new WaitForSeconds(1.0f);
                    light.SetRed();
                }
            }
            // If an NPC is waiting
            else
            {
                // Check if there are any cars
                if (!light.carInFront && !light.carInBack)
                {
                    // No cars, change to green immediately
                    light.SetGreen();
                    yield return new WaitForSeconds(10.0f); // Green for 10 seconds
                    light.SetYellow();
                    yield return new WaitForSeconds(1.0f);
                    light.SetRed();
                }
                else
                {
                    // Cars present, wait for them to pass
                    yield return new WaitForSeconds(3.0f);
                    light.SetYellow();
                    yield return new WaitForSeconds(1.0f);
                    light.SetRed();
                    
                    // Wait until no more cars
                    yield return new WaitUntil(() => !light.carInFront && !light.carInBack);
                    yield return new WaitForSeconds(1.0f); // Buffer time
                    
                    light.SetGreen();
                    yield return new WaitForSeconds(10.0f); // Green for 10 seconds
                    light.SetYellow();
                    yield return new WaitForSeconds(1.0f);
                    light.SetRed();
                }
            }
            
            yield return null; // Wait a frame before checking again
        }
    }
    
    // Methods to be called by triggers/colliders
    
    public void OnNPCEnterSideArea(Collider sideCollider)
    {
        if (colliderToLightIndex.TryGetValue(sideCollider as BoxCollider, out int index))
        {
            trafficLights[index].npcWaiting = true;
        }
    }
    
    public void OnNPCExitSideArea(Collider sideCollider)
    {
        if (colliderToLightIndex.TryGetValue(sideCollider as BoxCollider, out int index))
        {
            trafficLights[index].npcWaiting = false;
        }
    }
    
    public void OnCarEnterFrontArea(Collider frontCollider)
    {
        if (colliderToLightIndex.TryGetValue(frontCollider as BoxCollider, out int index))
        {
            trafficLights[index].carInFront = true;
        }
    }
    
    public void OnCarExitFrontArea(Collider frontCollider)
    {
        if (colliderToLightIndex.TryGetValue(frontCollider as BoxCollider, out int index))
        {
            trafficLights[index].carInFront = false;
        }
    }
    
    public void OnCarEnterBackArea(Collider backCollider)
    {
        if (colliderToLightIndex.TryGetValue(backCollider as BoxCollider, out int index))
        {
            trafficLights[index].carInBack = true;
        }
    }
    
    public void OnCarExitBackArea(Collider backCollider)
    {
        if (colliderToLightIndex.TryGetValue(backCollider as BoxCollider, out int index))
        {
            trafficLights[index].carInBack = false;
        }
    }
    
    // You can call this from NPC scripts to check if they should wait
    public bool ShouldNPCWait(Collider sideCollider)
    {
        if (colliderToLightIndex.TryGetValue(sideCollider as BoxCollider, out int index))
        {
            return !trafficLights[index].isGreen;
        }
        return true; // Default to waiting if collider not found
    }
}