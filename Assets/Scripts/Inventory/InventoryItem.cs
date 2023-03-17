using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public InventoryItemData inventoryData;
    public Item itemType;
}

public enum Item
{
    Orb,
    Health,
    Ammo,
    Food,
    Water,
    Null
}