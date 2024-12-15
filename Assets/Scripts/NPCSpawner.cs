using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public Transform spawnLocation;
    public Transform exit;
    public Transform shopEntrance;

    public PrefabDatabaseSO itemDatabase;

    public GameObject NPCPrefab;

    public bool spawnNpcs = true;

    private float waitTime;

    private void Start()
    {
        PrepareNextSpawn();
    }

    public void BeginSpawning()
    {
        PrepareNextSpawn();
    }

    private void PrepareNextSpawn()
    {
        waitTime = UnityEngine.Random.Range(5, 10);
        StartCoroutine(WaitUntilNextSpawn(waitTime, spawnNpcs));
    }

    private IEnumerator WaitUntilNextSpawn(float delay, bool spawnNPCs)
    {
        yield return new WaitForSeconds(delay);
        SpawnNPC();

        if (spawnNPCs)
        {
            PrepareNextSpawn();
        }
    }

    private void SpawnNPC()
    {
        int itemIndex = UnityEngine.Random.Range(0, itemDatabase.objectsData.Count);
        string itemName = itemDatabase.objectsData[itemIndex].Prefab.name;

        GameObject spawnedNPC = Instantiate(NPCPrefab, spawnLocation.position, Quaternion.identity);
        NPCAI AI = spawnedNPC.GetComponent<NPCAI>();
        AI.exit = exit;
        AI.shopEntrance = shopEntrance;
        AI.itemDatabase = itemDatabase;
        AI.destination = SearchForItem(itemName);
        AI.Activate();
    }

    private Transform SearchForItem(string itemName)
    {
        GameObject itemObj = GameObject.Find(itemName + "(Clone)");
        if (itemObj != null)
        {
            return itemObj.transform;
        }
        return null;
    }
}
