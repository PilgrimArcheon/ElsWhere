using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public bool canReceiveInput;
    public bool hasOrb;
    public InventoryItem currentPickableItem;
    public PopUpItem popUpItem;
    public Item activeUsableItem;
    public GameObject inventoryHolderUI;
    public GameObject indicator, needOrbIndicator;
    public bool canSelectItem;
    bool shownInventory;

    public Dictionary<Item, int> numberOfItems;
    public Text healthText, ammoText, foodText, waterText;
    public AudioSource sfx;
    public AudioClip pickUpSfx, openSfx, useSfx, orbSfx;

    void Awake()
    {
        if(Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    void Start()
    {
        numberOfItems = new Dictionary<Item, int>()
        {
            {Item.Health, 0},
            {Item.Ammo, 1},
            {Item.Food, 1},
            {Item.Water, 0}
        };
        indicator = GameObject.Find("Indicator").transform.GetChild(0).gameObject;
        needOrbIndicator = GameObject.Find("NeedOrbIndicator").transform.GetChild(0).gameObject;
        UpdateTextUI();
    }

    public void SelectActiveItem(Item itemType)
    {
        if(numberOfItems[itemType] > 0)
        {
            activeUsableItem = itemType;
            sfx.PlayOneShot(pickUpSfx);
        }
    }

    public void UseEquipedItem()
    {
        RemoveItem(activeUsableItem);
        sfx.PlayOneShot(useSfx);
    }

    public void ShowInventoryItem()
    {
        if(!shownInventory)
        {
            inventoryHolderUI.SetActive(true);
            sfx.PlayOneShot(openSfx);
            canSelectItem = true;
            shownInventory = true;
        }
    }

    public void HideInventoryItem()
    {
        inventoryHolderUI.SetActive(false);
        canSelectItem = false;
        shownInventory = false;
    }
    
    public void AddItem()
    {
        canReceiveInput = false;
        if(currentPickableItem != null)
        {
            if(currentPickableItem.itemType != Item.Orb)
            {
                PickUp();
                numberOfItems[currentPickableItem.itemType]++;
                UpdateTextUI(); 
            }
            else 
            {
                if(!hasOrb)
                {
                    PickUp();
                    hasOrb = true;
                }
            }
            currentPickableItem = null;
            indicator.SetActive(false);
                  
        }
    }

    void PickUp()
    {
        sfx.PlayOneShot(pickUpSfx);
        DestroyImmediate(currentPickableItem.gameObject, true);
        FindObjectOfType<CompassBar>().UpdateItemMarkers();
    }

    public void RemoveItem(Item itemType)
    {
        if(numberOfItems[itemType] > 0)
            numberOfItems[itemType]--;
        UpdateTextUI();
    }

    public void UpdateTextUI()
    {
        healthText.text = numberOfItems[Item.Health].ToString("0");
        ammoText.text = numberOfItems[Item.Ammo].ToString("0");
        foodText.text = numberOfItems[Item.Food].ToString("0");
        waterText.text = numberOfItems[Item.Water].ToString("0");
    }
}