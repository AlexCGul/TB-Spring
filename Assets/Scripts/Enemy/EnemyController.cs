using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    // Adjustable parameters
    [SerializeField] private GameObject[] patrolPoints;
    [SerializeField] private float detectionRadius = 10f;
    [SerializeField] private float lostVisualTimer = 2f;
    
    int currentPatrolPoint = 0;
    
    NavMeshAgent agent;

    private bool playerDetected = false;
    private bool lostVisual = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        Vector3 dest = GetNextPatrolPoint();
        agent.SetDestination(dest);
        agent.isStopped = false;

        StartCoroutine(DetectPlayer());
        StartCoroutine(Patrol());
    }


    IEnumerator Patrol()
    {
        Vector3 dest = agent.destination;
        while (!playerDetected)
        {
            yield return new WaitForSeconds(0.5f);
            if (Vector3.Distance(transform.position, dest) < 1f)
            {
                dest = GetNextPatrolPoint();
                agent.SetDestination(dest);
            }
        }

    }
    
    
    IEnumerator ChasePlayer(Transform player)
    {
        while (playerDetected)
        {
        #if UNITY_EDITOR
            Debug.Log("CHASING");
        #endif
            agent.SetDestination(player.position);
            yield return new WaitForSeconds(0.5f);
        }

        StartCoroutine(Patrol());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 255, 0, 0.1f);
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }

    IEnumerator DetectPlayer()
    {
        PlayerController player = PlayerController.Instance;
        BoxCollider bc = player.GetComponent<BoxCollider>();
        
        while (gameObject)
        {
            yield return new WaitForSeconds(0.25f);
            
            // Declarations
            RaycastHit hit;
            bool rayTop = false;
            bool rayBottom = false;
            
            bool rayStraight = Physics.Raycast(transform.position, player.transform.position - transform.position
                , out hit, detectionRadius*0.5f);
            if (hit.collider && !hit.collider.CompareTag("Player"))
            {
                rayTop = Physics.Raycast(transform.position,
                    (player.transform.position + new Vector3(0, bc.size.y * 0.5f, 0)) - transform.position
                    , out hit, detectionRadius * 0.5f);
            }

            if (hit.collider && !hit.collider.CompareTag("Player"))
            {
                rayBottom = Physics.Raycast(transform.position,
                    (player.transform.position - new Vector3(0, bc.size.y * 0.4f, 0)) - transform.position
                    , out hit, detectionRadius * 0.5f);
            }


            // Draw the detection lines for debugging
            #if UNITY_EDITOR
            Debug.DrawRay(transform.position, player.transform.position - transform.position, Color.red, 2f);
            Debug.DrawRay(transform.position, (player.transform.position + new Vector3(0,bc.size.y * 0.5f,0)) - transform.position, Color.red, 2f);
            Debug.DrawRay(transform.position, (player.transform.position - new Vector3(0,bc.size.y * 0.4f,0)) - transform.position, Color.red, 2f);
            #endif
            
            if (rayStraight || rayBottom || rayTop)
            {
                if (lostVisual)
                    lostVisual = false;
                
                if (hit.collider && hit.collider.CompareTag("Player"))
                {
                    playerDetected = true;
                    StartCoroutine(ChasePlayer(hit.transform));

                    #if UNITY_EDITOR
                    Debug.Log("START CHASE");
                    #endif
                }
                
                else if (playerDetected == true && !lostVisual)
                {
                    StartCoroutine(LostVisualCoolDown());
                }
            }

        }
    }


    IEnumerator LostVisualCoolDown()
    {
        lostVisual = true;
        yield return new WaitForSeconds(lostVisualTimer);

        if (!lostVisual) yield break;
                
        playerDetected = false;
        lostVisual = false;

        #if UNITY_EDITOR
        Debug.Log(("LOST VISUAL"));
        #endif
    }


    Vector3 GetNextPatrolPoint()
    {
        currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
        Debug.Log(patrolPoints[currentPatrolPoint].transform.position + " Current patrol point set to " + currentPatrolPoint);
        return patrolPoints[currentPatrolPoint].transform.position;
    }    
}
