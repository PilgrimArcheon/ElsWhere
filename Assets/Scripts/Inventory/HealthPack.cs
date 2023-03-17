using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPack : MonoBehaviour, I_InventoryItem
{
    public int healthPackValue;

    public void Use()
    {
        if(FindObjectOfType<PlayerStats>().health < 100)
        {
            FindObjectOfType<PlayerStats>().health += healthPackValue;
            Inventory.Instance.RemoveItem(Inventory.Instance.activeUsableItem);
            FindObjectOfType<ThirdPersonShooterController>().OnUsedEquiped();
        }
    }
}