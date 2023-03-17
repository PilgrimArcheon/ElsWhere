using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour, I_InventoryItem
{
    public int ammoValue;

    public void Use()
    {
        if(FindObjectOfType<PlayerStats>().ammo < 100)
        {
            FindObjectOfType<PlayerStats>().ammo += ammoValue;
            Inventory.Instance.RemoveItem(Inventory.Instance.activeUsableItem);
            FindObjectOfType<ThirdPersonShooterController>().OnUsedEquiped();
            FindObjectOfType<ThirdPersonController>().AnimReload();
        }
    }
}