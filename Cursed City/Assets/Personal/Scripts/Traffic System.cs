using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrafficSystem : MonoBehaviour
{
    [System.Serializable]
    public class TrafficLightElement
    {
        public string name;
        [Header("Front Light")]
        public GameObject frontRedLight;
        public GameObject frontYellowLight;
        public GameObject frontGreenLight;
        
        [Header("Back Light")]
        public GameObject backRedLight;
        public GameObject backYellowLight;
        public GameObject backGreenLight;
        
       [Header("Detection")]
        public List<SphereCollider> pedestrianDetectors = new List<SphereCollider>();
        
        [Header("Sidewalk Points")]
        public List<Transform> sidewalkPoints = new List<Transform>();
        
        [HideInInspector]
        public TrafficLightState currentState = TrafficLightState.Red;
        
        private Dictionary<NPCMovement, Vector3> affectedNPCs = new Dictionary<NPCMovement, Vector3>();
        
        public void SetRed()
        {

            frontRedLight.SetActive(true);
            frontYellowLight.SetActive(false);
            frontGreenLight.SetActive(false);
            

            backRedLight.SetActive(true);
            backYellowLight.SetActive(false);
            backGreenLight.SetActive(false);
            
            currentState = TrafficLightState.Red;
            if (currentState != TrafficLightState.Yellow)
            {
                MoveNPCsToSidewalk();
            }
        }
        
        public void SetYellow()
        {
            frontRedLight.SetActive(false);
            frontYellowLight.SetActive(true);
            frontGreenLight.SetActive(false);
            backRedLight.SetActive(false);
            backYellowLight.SetActive(true);
            backGreenLight.SetActive(false);
            
            currentState = TrafficLightState.Yellow;
            MoveNPCsToSidewalk();
        }
        
        public void SetGreen()
        {
            frontRedLight.SetActive(false);
            frontYellowLight.SetActive(false);
            frontGreenLight.SetActive(true);
            backRedLight.SetActive(false);
            backYellowLight.SetActive(false);
            backGreenLight.SetActive(true);
            
            currentState = TrafficLightState.Green;
            ResumeNPCs();
        }
        
        private void MoveNPCsToSidewalk()
        {
            if (sidewalkPoints.Count == 0)
            {
                Debug.LogWarning("No sidewalk points assigned to traffic light: " + name);
                return;
            }
            
            foreach (SphereCollider detector in pedestrianDetectors)
            {
                Collider[] colliders = Physics.OverlapSphere(
                    detector.transform.position, 
                    detector.radius * Mathf.Max(
                        detector.transform.lossyScale.x,
                        detector.transform.lossyScale.y,
                        detector.transform.lossyScale.z
                    )
                );
                
                foreach (Collider col in colliders)
                {
                    NPCMovement npc = col.GetComponent<NPCMovement>();
                    if (npc != null)
                    {
                        if (!affectedNPCs.ContainsKey(npc))
                        {
                            NavMeshAgent originalAgent = npc.GetComponent<NavMeshAgent>();
                            if (originalAgent != null && originalAgent.hasPath)
                            {
                                affectedNPCs[npc] = originalAgent.destination;
                            }
                            else
                            {
                                affectedNPCs[npc] = npc.transform.position;
                            }
                        }
                        Transform nearestSidewalk = GetNearestSidewalkPoint(npc.transform.position);
                        NavMeshAgent sidewalkAgent = npc.GetComponent<NavMeshAgent>();
                        if (sidewalkAgent != null)
                        {
                            sidewalkAgent.isStopped = false; 
                            sidewalkAgent.SetDestination(nearestSidewalk.position);
                        }
                    }
                }
            }
        }
        
        private void ResumeNPCs()
        {
            foreach (KeyValuePair<NPCMovement, Vector3> npcPair in affectedNPCs)
            {
                NPCMovement npc = npcPair.Key;
                if (npc != null)
                {
                    NavMeshAgent resumeAgent = npc.GetComponent<NavMeshAgent>();
                    if (resumeAgent != null)
                    {
                        resumeAgent.SetDestination(npcPair.Value);
                        npc.WanderAgent(); 
                    }
                }
            }
            affectedNPCs.Clear();
        }
        
        private Transform GetNearestSidewalkPoint(Vector3 position)
        {
            Transform nearest = sidewalkPoints[0];
            float minDistance = Vector3.Distance(position, nearest.position);
            
            for (int i = 1; i < sidewalkPoints.Count; i++)
            {
                float distance = Vector3.Distance(position, sidewalkPoints[i].position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearest = sidewalkPoints[i];
                }
            }
            
            return nearest;
        }
    }
    
    public enum TrafficLightState
    {
        Red,
        Yellow,
        Green
    }
    
    [Header("Traffic Light Settings")]
    public List<TrafficLightElement> trafficLights = new List<TrafficLightElement>();
    
    [Header("Timing Settings")]
    public float greenLightDuration = 10.0f;
    public float yellowLightDuration = 3.0f;
    public float redLightDuration = 15.0f;
    
    void Start()
    {
        foreach (TrafficLightElement light in trafficLights)
        {
            light.SetRed();
        }
        
        for (int i = 0; i < trafficLights.Count; i++)
        {
            StartCoroutine(TrafficLightCycle(i));
        }
    }
    
    IEnumerator TrafficLightCycle(int index)
    {
        TrafficLightElement light = trafficLights[index];
        
        while (true)
        {
            light.SetRed();
            yield return new WaitForSeconds(redLightDuration);
            
            light.SetGreen();
            yield return new WaitForSeconds(greenLightDuration);
            
            light.SetYellow();
            yield return new WaitForSeconds(yellowLightDuration);
        }
    }
    
    public bool IsTrafficLightGreen(int index)
    {
        if (index >= 0 && index < trafficLights.Count)
        {
            return trafficLights[index].currentState == TrafficLightState.Green;
        }
        return false;
    }
    
    public TrafficLightState GetTrafficLightState(int index)
    {
        if (index >= 0 && index < trafficLights.Count)
        {
            return trafficLights[index].currentState;
        }
        return TrafficLightState.Red; 
    }
}