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
        StartCoroutine(DetectPlayer());
        StartCoroutine(Patrol());
    }


    IEnumerator Patrol()
    {
        Vector3 dest = GetNextPatrolPoint();
        while (!playerDetected)
        {
            yield return new WaitForSeconds(0.5f);
            if (Vector3.Distance(transform.position, dest) < 0.001f)
            {
                Debug.Log("DESTINATION REACHED " + Vector3.Distance(transform.position, dest ));
                dest = GetNextPatrolPoint();
                agent.SetDestination(dest);
                Debug.Log("DESINATION SET TO " + dest);
            }
        }

    }
    
    
    IEnumerator ChasePlayer(Transform player)
    {
        while (playerDetected)
        {
            agent.SetDestination(player.position);
            yield return new WaitForSeconds(0.5f);
        }

        StartCoroutine(Patrol());
    }


    IEnumerator DetectPlayer()
    {
        PlayerController player = PlayerController.Instance;
        BoxCollider bc = player.GetComponent<BoxCollider>();
        
        while (gameObject)
        {
            yield return new WaitForSeconds(0.25f);
            
            RaycastHit hit;
            bool rayStraight = Physics.Raycast(transform.position, transform.position - player.transform.position, out hit, 10f);
            bool rayTop = Physics.Raycast(transform.position + Vector3.up, transform.position - (player.transform.position + new Vector3(0,bc.size.y * 0.5f,0))
                , out hit, 10f);
            bool rayBottom = Physics.Raycast(transform.position + Vector3.up, transform.position - (player.transform.position - new Vector3(0,bc.size.y * 0.4f,0))
                , out hit, 10f);
            
            
            // Draw the detection lines for debugging
            #if UNITY_EDITOR
            Debug.DrawRay(transform.position, transform.position - player.transform.position, Color.red);
            Debug.DrawRay(transform.position + Vector3.up, transform.position - (player.transform.position + new Vector3(0,bc.size.y * 0.5f,0)), Color.red);
            Debug.DrawRay(transform.position + Vector3.up, transform.position - (player.transform.position - new Vector3(0,bc.size.y * 0.4f,0)), Color.red);
            #endif
            
            if (rayStraight || rayBottom || rayTop)
            {
                if (lostVisual)
                    lostVisual = false;
                
                if (hit.collider.CompareTag("Player"))
                {
                    playerDetected = true;
                    StartCoroutine(ChasePlayer(hit.transform));
                }

                continue;
            }

            //StartCoroutine(LostVisualCoolDown());
        }
    }


    IEnumerator LostVisualCoolDown()
    {
        lostVisual = true;
        yield return new WaitForSeconds(lostVisualTimer);

        if (!lostVisual) yield break;
        
        playerDetected = false;
        lostVisual = false;
        agent.SetDestination(GetNextPatrolPoint());
    }


    Vector3 GetNextPatrolPoint()
    {
        currentPatrolPoint = (currentPatrolPoint + 1) % patrolPoints.Length;
        Debug.Log(patrolPoints[currentPatrolPoint].transform.position + " Current patrol point set to " + currentPatrolPoint);
        return patrolPoints[currentPatrolPoint].transform.position;
    }    
}
