using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour, I_InventoryItem
{
    public float foodEnergyValue;

    public void Use()
    {
        if(FindObjectOfType<PlayerStats>().energy < 100)
        {
            FindObjectOfType<PlayerStats>().energy += foodEnergyValue;
            Inventory.Instance.RemoveItem(Inventory.Instance.activeUsableItem);
            FindObjectOfType<ThirdPersonShooterController>().OnUsedEquiped();
        }
    }
}