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

    Coroutine destinationRoutine;

    void Start()
    {
        randomNum = Random.Range(0, 100);
        manager = NPCManager.Instance;

        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        navMeshAgent.speed = Random.Range(manager.npcSpeedRange.x, manager.npcSpeedRange.y);

        destinationRoutine = StartCoroutine(MoveToNextTarget());
    }

    void OnDisable()
    {
        if (destinationRoutine != null)
        {
            StopCoroutine(destinationRoutine);
        }
    }

    [Button("Wander")]
    public void WanderAgent()
    {
        navMeshAgent.isStopped = false;
    }

    [Button("Stop")]
    public void StopAgent()
    {
        navMeshAgent.isStopped = true;
    }

    void Update()
    {
        animator.SetFloat("Speed", navMeshAgent.isStopped || navMeshAgent.hasPath == false? 0 : 0.5f);
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
        
            navMeshAgent.SetDestination(NPCManager.Instance.GetRandomPositionOnNavMesh());

            // Wait until the NPC has reached the target
            while (navMeshAgent.remainingDistance > stoppingDistance)
            {
                if(!navMeshAgent.enabled) yield break;
                yield return null;
            }

            yield return null;

        }
    }
}
