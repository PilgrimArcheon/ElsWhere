using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="New Item", menuName ="InventoryItemData")]
public class InventoryItemData : ScriptableObject
{
    public string displayName;
    //public int id;
    public Sprite icon;
    public GameObject prefab;

}

