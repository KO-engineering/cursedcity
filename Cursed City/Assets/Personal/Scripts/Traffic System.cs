using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficSystem : MonoBehaviour
{
    [System.Serializable]
    public class TrafficLightElement
    {
        public GameObject redLight;
        public GameObject yellowLight;
        public GameObject greenLight;
        public BoxCollider leftSideCollider;
        public BoxCollider rightSideCollider;
        public BoxCollider frontCollider;
        public BoxCollider backCollider;
        public bool isGreen = false;
        public bool npcWaiting = false;
        public bool carInFront = false;
        public bool carInBack = false;
        
        
        public void SetRed()
        {
            redLight.SetActive(true);
            yellowLight.SetActive(false);
            greenLight.SetActive(false);
            isGreen = false;
        }
        
        public void SetYellow()
        {
            redLight.SetActive(false);
            yellowLight.SetActive(true);
            greenLight.SetActive(false);
        }
        
        public void SetGreen()
        {
            redLight.SetActive(false);
            yellowLight.SetActive(false);
            greenLight.SetActive(true);
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