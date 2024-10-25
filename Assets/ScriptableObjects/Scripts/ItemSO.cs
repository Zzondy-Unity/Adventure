using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Consumable,
    Equipable,
    Openable,
    Resource
}

public enum ConsumableType
{
    HP,
    MP,
    Stamina
}

[Serializable]
public class ItemDataConsumable
{
    public ConsumableType ConsumableType;
    public float value;
}

[CreateAssetMenu( fileName = "ItemSO", menuName = "DefaultItemSO", order = 0)]
public class ItemSO : ScriptableObject
{
    [Header("Info")]
    public string itemName;
    public string description;
    public ItemType itemType;
    public Sprite icon;
    public GameObject prefab;

    [Header("Stacking")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumables;

    [Header("Equip")]
    public GameObject equipPrefab;
}
