using GD;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private GameEvent dayEvent;
    [SerializeField]
    public Timer timer;
    [SerializeField]
    public GameObject player;

    [Required, BoxGroup("Fields of View")]
    public FieldOfView fieldOfView1;
    [Required, BoxGroup("Fields of View")]
    public FieldOfView fieldOfView2;
    [Required, BoxGroup("Other Components")]
    public GunController gunController;
    [BoxGroup("Other Components")]
    public HealthManager healthManager;

    [Required, BoxGroup("Navigation")]
    public NavMeshAgent agent;
    [BoxGroup("Navigation")]
    public Transform objective;
    [BoxGroup("Navigation")]
    public Transform exit;

    public float fleeAtHealthTreshhold = 10;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // For testing purposes
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    Activate();
        //}
    }

    public void Activate()
    {
        StartCoroutine(MoveToObjective());
        //transform.position = exit.position;
    }

    IEnumerator MoveToObjective()
    {
        agent.speed = 2;
        agent.SetDestination(objective.position);

        // Wait until the path is finished computing and then has reached destination
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            if (healthManager.GetHealth() <= 10)
            {
                StartCoroutine(Flee());
                // Flee sound effect here

                Debug.Log("Fleeing...");
                yield break;
            }
            // Engage the player if spotted or damaged
            if ((fieldOfView1.canSeePlayer || fieldOfView2.canSeePlayer) || healthManager.damaged)
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
        agent.speed = 3;
        // Only change destination if it has chenged
        if (agent.destination != playerTransform.position)
        {
            agent.SetDestination(playerTransform.position);
        }

        // Wait until the path is finished computing and then has reached destination
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            if (healthManager.GetHealth() <= fleeAtHealthTreshhold)
            {
                StartCoroutine(Flee());
                // Flee sound effect here

                Debug.Log("Fleeing...");
                yield break;
            }
            // Shoot at player when in line of sight
            if (fieldOfView1.canSeePlayer || fieldOfView2.canSeePlayer)
            {
                agent.SetDestination(playerTransform.position);
                gunController.FireFromObject();
                // Retarget player
                if (agent.destination != playerTransform.position)
                {
                    agent.SetDestination(playerTransform.position);
                }
            }
            // Retarget player if hit
            else if (healthManager.damaged)
            {
                if (agent.destination != playerTransform.position)
                {
                    agent.SetDestination(playerTransform.position);
                }
            }
            yield return null;
        }

        yield return new WaitForSeconds(1);
        Debug.Log("Enemy has lost the player. Returning to objective.");
        StartCoroutine(MoveToObjective());
    }

    IEnumerator Flee()
    {
        agent.speed = 5;
        agent.SetDestination(exit.position);

        // Wait until the path is finished computing and then has reached destination
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            // Kill enemy if health reaches 0
            if (healthManager.GetHealth() <= 0)
            {
                // Death sound effect here

                Debug.Log("Enemy has been killed!");
                agent.speed = 0;
                StartCoroutine(CallDayModeAfterSeconds(5f));
                yield break;
            }
            yield return null;
        }

        StartCoroutine(CallDayModeAfterSeconds(5f));
    }

    IEnumerator CallDayModeAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        dayEvent?.Raise();
        timer.SetTime(8, 0);
        gameObject.SetActive(false);
    }
}
