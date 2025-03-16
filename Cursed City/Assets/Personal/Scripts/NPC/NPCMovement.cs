using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCMovement : MonoBehaviour
{
    public float stoppingDistance = 0.1f;
    public int randomNum;
    NPCManager manager;
    NavMeshAgent navMeshAgent;
    Animator animator;
    [ReadOnly] public int randomIndex;
    [ReadOnly] public Transform targetTransform;

    private bool waitingForTrafficLight = false;

    void Start()
    {
        randomNum = Random.Range(0, 100);
        manager = NPCManager.Instance;

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        navMeshAgent.speed = Random.Range(manager.npcSpeedRange.x, manager.npcSpeedRange.y);

        StartCoroutine(MoveToNextTarget());
    }

    [Button("Wander")]
    public void WanderAgent()
    {
        if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
            navMeshAgent.isStopped = false;
    }

    [Button("Stop")]
    public void StopAgent()
    {
        if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
            navMeshAgent.isStopped = true;
    }

    void Update()
    {
        if (animator != null && navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
            animator.SetFloat("Speed", navMeshAgent.isStopped || navMeshAgent.hasPath == false ? 0 : 0.5f);
    }

    IEnumerator MoveToNextTarget()
    {
        while (true)
        {
            /**
            if(DayNightCycle.Instance.isNight == true && randomNum < 50)
            {
                if(manager.npcDrunkPaths != null)
                {
                    randomIndex = Random.Range(0, manager.npcDrunkPaths.Count);
                    targetTransform = manager.npcDrunkPaths[randomIndex];
                }
            }
            else{
            **/
            //randomIndex = Random.Range(0, manager.npcPaths.Count);
            //targetTransform = manager.npcPaths[randomIndex];
            //}
            
            if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
                navMeshAgent.SetDestination(NPCManager.Instance.GetRandomPositionOnNavMesh());
            
            // Wait until the NPC has reached the target or is stopped by a traffic light
            while (navMeshAgent != null && navMeshAgent.isActiveAndEnabled && navMeshAgent.remainingDistance > stoppingDistance)
            {
                if (navMeshAgent == null || !navMeshAgent.isActiveAndEnabled) 
                    yield break;
                
                // If we're waiting for a traffic light
                if (waitingForTrafficLight)
                {
                    // Simply wait a bit and then resume - the traffic light system will handle the rest
                    yield return new WaitForSeconds(5f);
                    waitingForTrafficLight = false;
                    
                    if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
                        navMeshAgent.isStopped = false;
                }
                
                yield return null;
            }
            
            yield return new WaitForSeconds(1f);
        }
    }
    
    // Called when the NPC enters a traffic light's side collider
    void OnTriggerEnter(Collider other)
    {
        // Simple tag check instead of complex component lookups
        if (other.CompareTag("TrafficLightTrigger"))
        {
            // Check if this is a red light
            TrafficSystem trafficSystem = other.GetComponentInParent<TrafficSystem>();
            if (trafficSystem != null)
            {
                // Simplified check - just assume we should stop
                waitingForTrafficLight = true;
                
                if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
                {
                    navMeshAgent.isStopped = true;
                    
                    if (animator != null)
                        animator.SetFloat("Speed", 0);
                }
            }
        }
    }
    
    // Called when the NPC exits a traffic light's side collider
    void OnTriggerExit(Collider other)
    {
        // Simple tag check
        if (other.CompareTag("TrafficLightTrigger"))
        {
            waitingForTrafficLight = false;
            
            if (navMeshAgent != null && navMeshAgent.isActiveAndEnabled)
                navMeshAgent.isStopped = false;
        }
    }
}