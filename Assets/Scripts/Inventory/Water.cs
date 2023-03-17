using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour, I_InventoryItem
{
    public float waterEnergyValue;

    public void Use()
    {
        if(FindObjectOfType<PlayerStats>().energy < 100)
        {
            FindObjectOfType<PlayerStats>().energy += waterEnergyValue;
            Inventory.Instance.RemoveItem(Inventory.Instance.activeUsableItem);
            FindObjectOfType<ThirdPersonShooterController>().OnUsedEquiped();
        }
    }
}