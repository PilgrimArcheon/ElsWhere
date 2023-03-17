using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orb : MonoBehaviour, I_InventoryItem
{
    public bool closeToRoots;
    public AudioSource itemSfx;
    public GameObject roots;

    public void Use()
    {
        Debug.Log("USE ORB!!!!!!!!!");
        if(closeToRoots && roots != null)
        {
            FindObjectOfType<ThirdPersonShooterController>().OnUsedEquiped();
            foreach (Transform child in roots.transform)
            {
                child.gameObject.GetComponent<GrowRoot>().UpdateRoots();
            }
            roots.GetComponent<RootTrigger>().UncoveredRoots();
            roots.GetComponent<Collider>().enabled = false;
            Inventory.Instance.indicator.SetActive(false);
            Inventory.Instance.hasOrb = false;
            Inventory.Instance.sfx.PlayOneShot(Inventory.Instance.orbSfx);
            roots = null;
            gameObject.SetActive(false);
        } 
    }

    void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag == "RootsTrigger")
        {
            closeToRoots = true;
            roots = other.gameObject;
            Inventory.Instance.indicator.SetActive(true);
            Inventory.Instance.needOrbIndicator.SetActive(false);
        }

        if(other.gameObject.tag == "PopUp")
        {
            if(other.gameObject.GetComponent<PopUpItem>() != null)
            {
                Inventory.Instance.popUpItem = other.gameObject.GetComponent<PopUpItem>();
            }
        }
    }

    void OnTriggerExit(Collider other) 
    {
        if(other.gameObject.tag == "RootsTrigger")
        {
            closeToRoots = false;
            Inventory.Instance.indicator.SetActive(false);
            roots = null;
        }

        if(other.gameObject.tag == "PopUp")
            Inventory.Instance.popUpItem = null;
    }
}