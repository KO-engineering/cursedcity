using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using System.Linq;
using TMPro;

public class NPCSheriff : MonoBehaviour
{
    [HorizontalLine]
    public Transform npcPatrolPathParent;
    [ReadOnly] public List<Transform> npcPatrolPaths = new List<Transform>();
    public float stoppingDistance = 0.1f;
    [ReadOnly] public int index;
    [ReadOnly] public bool isThere = false;
    [ReadOnly] public bool killing;
    [ReadOnly] public bool isShooting;
    public int damage = 10;
    [HorizontalLine]

    public GameObject playerGameObject;
    public Transform player;
    public GameObject gunTip;
    public ParticleSystem gunSmoke;
    UnityEngine.AI.NavMeshAgent navMeshAgent;
    Animator animator;
    Health health;
    public float maxDist = 30f;

    void Start()
    {
        health = playerGameObject.GetComponent<Health>();
        killing = false;
        isShooting = false;
        navMeshAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        if (npcPatrolPathParent != null) StartCoroutine(MoveToNextTarget());
    }

    void OnDisable()
    {
        StopAllCoroutines();
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
        if(GetComponent<Health>().hp > 0){
            animator.SetFloat("Speed", navMeshAgent.isStopped? 0 : 0.5f);

            // Check if the NPC is not stopped
            if (!navMeshAgent.isStopped)
            {
                // If not stopped, set the animator speed
                animator.SetFloat("Speed", 0.5f);
                isThere = false;
            }

            float dist = Vector3.Distance(player.position, transform.position);

            if (KillCount.Instance.killCount > 0 && dist <= maxDist)
            {
                if (!isShooting && !Input.GetKeyDown(KeyCode.Space))
                {
                    isShooting = true;
                    StartCoroutine(SheriffShoot());
                }
                killing = true;
                navMeshAgent.SetDestination(player.position);
                animator.SetBool("Aiming", true);

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    isShooting = true;
                    navMeshAgent.speed = 8;
                    animator.SetFloat("Speed", 8f);
                    animator.SetBool("Aiming", false);
                }
            }
        }
    }


    IEnumerator MoveToNextTarget()
    {
        while (true)
        {
            int randomIndex = Random.Range(0, npcPatrolPaths.Count);
            Transform targetTransform = npcPatrolPaths[randomIndex];

            navMeshAgent.SetDestination(targetTransform.position);

            // Wait until the NPC has reached the target
            while (navMeshAgent.remainingDistance > stoppingDistance)
            {
                if (!navMeshAgent.enabled) yield break;
                yield return null;
            }

            yield return null;
        }
    }
    IEnumerator SheriffShoot()
    {
        ParticleSystem gunParticle = Instantiate(gunSmoke, gunTip.transform.position, Quaternion.identity);
        gunParticle.Play();
        health.ChangeHealth(-damage);
        yield return new WaitForSeconds(5);
        isShooting = false; // Set isShooting to false after the delay
    }
    [Button("Update NPC Patrol Paths")]
    public void UpdateNPCPatrolPaths()
    {
        npcPatrolPaths = npcPatrolPathParent.Cast<Transform>().ToList();
    }
}
