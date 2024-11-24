using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Learned from Tutorial: https://www.youtube.com/watch?v=l0emsAHIBjU&list=PLcRSafycjWFepsLiAHxxi8D_5GGvu6arf

[CreateAssetMenu]
public class PrefabDatabaseSO : ScriptableObject
{
    public List<PrefabData> objectsData;

}

[Serializable]
public class PrefabData
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField]
    public int ID { get; private set; }
    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;
    [field: SerializeField]
    public GameObject Prefab { get; private set; }
    [field: SerializeField]
    public ItemData ItemData { get; private set; }
}

[Serializable]
public class ItemData
{
    [field: SerializeField]
    public string Name { get; private set; }
    [field: SerializeField, Multiline]
    public string Description { get; private set; }
    [field: SerializeField]
    public Sprite Icon { get; private set; }
    [field: SerializeField]
    public float BuyPrice { get; private set; }
}
