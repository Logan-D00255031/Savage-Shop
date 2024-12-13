using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [Required]
    public FieldOfView fieldOfView;
    [Required]
    public GunController gunController;
    public HealthManager healthManager;

    [Required]
    public NavMeshAgent agent;
    public Transform objective;

    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // For testing purposes
        if (Input.GetKeyDown(KeyCode.N))
        {
            Activate();
        }
    }

    public void Activate()
    {
        StartCoroutine(MoveToObjective());
    }

    IEnumerator MoveToObjective()
    {
        agent.SetDestination(objective.position);

        // Wait until the path is finished computing and then has reached destination
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            // Engage the player if spotted or damaged
            if (fieldOfView.canSeePlayer || healthManager.damaged)
            {
                Debug.Log("Spotted player! Beginning to target.");
                StartCoroutine(TargetPlayer());
                yield break;
            }
            yield return null;
        }

        Debug.Log("Enemy has reached their objective!");
        //Destroy(gameObject);
    }

    IEnumerator TargetPlayer()
    {
        Transform playerTransform = player.transform;
        agent.SetDestination(playerTransform.position);

        // Wait until the path is finished computing and then has reached destination
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            // Shoot at player when in line of sight
            if (fieldOfView.canSeePlayer)
            {
                agent.SetDestination(playerTransform.position);
                gunController.FireFromObject();
            }
            yield return null;
        }

        yield return new WaitForSeconds(1);
        Debug.Log("Enemy has lost the player. Returning to objective.");
        StartCoroutine(MoveToObjective());
    }
}