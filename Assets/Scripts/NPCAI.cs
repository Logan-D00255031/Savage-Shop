using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.AI;

public class NPCAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform exit;
    public Transform shopEntrance;
    public Transform destination;

    public PrefabDatabaseSO itemDatabase;

    [ReadOnly, SerializeField]
    private int itemIndex;
    [ReadOnly, SerializeField]
    private string itemName;

    [ReadOnly, SerializeField]
    private int minPrice;
    [ReadOnly, SerializeField]
    private int maxPrice;

    // Start is called before the first frame update
    void Start()
    {
        itemIndex = Random.Range(0, itemDatabase.objectsData.Count);
        itemName = itemDatabase.objectsData[itemIndex].Prefab.name;
        SearchForItem();

        minPrice = Random.Range(0, 10);
        maxPrice = minPrice + Random.Range(5, 20);
    }

    private bool SearchForItem()
    {
        GameObject itemObj = GameObject.Find(itemName + "(Clone)");
        if (itemObj != null)
        {
            destination = itemObj.transform;
            return true;
        }
        return false;
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetKeyDown(KeyCode.L))
        {
            if (SearchForItem())
            {
                StartCoroutine(MoveBuyAndLeave());
            }
            else
            {
                StartCoroutine(MoveAndLeave());
            }
        }

    }

    private void AttemptToBuy()
    {
        float itemPrice = itemDatabase.objectsData[itemIndex].ItemData.BuyPrice;

        if (itemPrice >= minPrice && maxPrice >= itemPrice)
        {
            // Add itemPrice to wallet


            Transform slot = destination.parent;
            ItemHolder holder = slot.GetComponentInParent<ItemHolder>();

            // Take item
            holder.RemoveItemIn(slot, false);
            Debug.Log($"Item Bought: {itemName}");
        }
        else
        {
            Debug.Log($"Item not within price range ({minPrice}, {maxPrice}): {itemPrice}");
        }

        // Leave the store
        StartCoroutine(LeaveStore());
    }

    IEnumerator MoveBuyAndLeave()
    {
        agent.SetDestination(destination.position);

        // Wait until the path is finished computing and then has reached destination
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        AttemptToBuy();
    }

    IEnumerator LeaveStore()
    {
        agent.SetDestination(exit.position);

        // Wait until the path is finished computing and then has reached destination
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        Destroy(gameObject);
    }

    IEnumerator MoveAndLeave()
    {
        agent.SetDestination(shopEntrance.position);

        // Wait until the path is finished computing and then has reached destination
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }

        StartCoroutine(LeaveStore());
    }
}
