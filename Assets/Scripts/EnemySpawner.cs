using GD;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [Required, SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private Timer timer;
    [SerializeField]
    private GameObject player;

    [BoxGroup("Navigation")]
    public Transform spawnPoint;
    [BoxGroup("Navigation")]
    public Transform objective;
    [BoxGroup("Navigation")]
    public Transform exit;

    public void SpawnEnemy()
    {
        GameObject spawnedEmemy = Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
        EnemyAI AI = spawnedEmemy.GetComponent<EnemyAI>();
        AI.timer = timer;
        AI.player = player;
        AI.objective = objective;
        AI.exit = exit;
        ActivateEnemy(AI);
    }

    private static void ActivateEnemy(EnemyAI AI)
    {
        AI.Activate();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    SpawnEnemy();
        //}
    }
}
