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

    [SerializeField]
    private ItemSellPriceDataBase itemSellPriceData;

    [ReadOnly]
    public int itemIndex;
    [ReadOnly]
    public string itemName;

    [ReadOnly, SerializeField]
    private float minPrice;
    [ReadOnly, SerializeField]
    private float maxPrice;

    // Start is called before the first frame update
    void Start()
    {
        //itemIndex = Random.Range(0, itemDatabase.objectsData.Count);
        //itemName = itemDatabase.objectsData[itemIndex].Prefab.name;
    }

    public void Activate()
    {
        float itemPrice = itemDatabase.objectsData[itemIndex].ItemData.BuyPrice;

        minPrice = Random.Range(0, (itemPrice * 0.5f));
        maxPrice = minPrice + Random.Range((minPrice + 5), (minPrice + 10) + (itemPrice * 2));


        if (destination != null)
        {
            Debug.Log("Found Item");
            StartCoroutine(MoveBuyAndLeave());
        }
        else
        {
            Debug.Log("No Item Found");
            StartCoroutine(MoveAndLeave());
        }
    }

    private void AttemptToBuy()
    {
        int itemID = itemDatabase.objectsData[itemIndex].ID;
        float itemPrice = 0;
        bool sellPriceNotSet = true;
        foreach (ItemSellPriceData priceData in itemSellPriceData.itemSellPrices)
        {
            if (priceData.ID == itemID)
            {
                itemPrice = priceData.SellPrice;
                sellPriceNotSet = false;
            }
        }
        if (sellPriceNotSet)
        {
            itemPrice = itemDatabase.objectsData[itemIndex].ItemData.BuyPrice;
        }


        if (destination != null)
        {
            if (itemPrice >= minPrice && maxPrice >= itemPrice)
            {
                SFXManager.instance.PlaySFX(SFXManager.SFX.BuyItem);

                // Add itemPrice to wallet
                WalletManager.instance.AddToWallet(itemPrice);

                Transform slot = destination.parent;
                ItemHolder holder = slot.GetComponentInParent<ItemHolder>();

                // Take item
                holder.RemoveItemIn(slot, false);
                Debug.Log($"Item Bought: {itemName}");
            }
            else
            {
                SFXManager.instance.PlaySFX(SFXManager.SFX.Invalid);
                Debug.Log($"Item not within price range ({minPrice}, {maxPrice}): {itemPrice}");
            }
        }
        else
        {
            Debug.Log("Item no longer exists");
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
