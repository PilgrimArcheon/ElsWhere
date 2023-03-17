using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class ThirdPersonShooterController : MonoBehaviour 
{
    [SerializeField] private CinemachineVirtualCamera aimVirtualCamera;
    [SerializeField] private float normalSensitivity;
    [SerializeField] private float aimSensitivity;
    [SerializeField] private LayerMask aimColliderLayerMask = new LayerMask();
    [SerializeField] private Transform debugTransform;
    [SerializeField] private Transform pfBulletProjectile;
    [SerializeField] private Transform spawnBulletPosition;
    [SerializeField] private Rig aimRig; 
    [SerializeField] private Rig holderRig;
    [SerializeField] bool isAiming;

    [SerializeField] GameObject[] itemGameObjects;
    [SerializeField] GameObject activeItemGameObject;
    [SerializeField] GameObject ShotGun;
    public GameObject orbGameObject;
    GameObject crossHair;
    GameObject hasOrbIndicator;

    public LayerMask whatIsEnemy;
    public float enemyInRange;
    
    private ThirdPersonController thirdPersonController;
    private PlayerStats playerStats;
    private PlayerInput playerInput;
    private Animator animator;

    Transform hitTransform;
    Vector3 mouseWorldPosition;
    public float aimRigWeight;
    public float holdingRigWeight;
    public bool weaponEquiped;

    public Item selectedItem;

    private void Awake() 
    {
        thirdPersonController = GetComponent<ThirdPersonController>();
        playerStats = GetComponent<PlayerStats>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        crossHair = GameObject.Find("Crosshair");
        hasOrbIndicator = GameObject.Find("OrbIndicator");
    }

    private void FixedUpdate()
    {
        isAiming = Physics.CheckSphere(transform.position, enemyInRange, whatIsEnemy);
    }

    private void Update() 
    {
        float _olderAimRigWeight = aimRig.weight;
        aimRig.weight = Mathf.Lerp(aimRig.weight, aimRigWeight, Time.deltaTime * 30f);
        if(aimRig.weight < 0.1f && _olderAimRigWeight > aimRig.weight) aimRig.weight = 0; 
        animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), aimRigWeight, Time.deltaTime * 30f));

        float _olderRigWeight = holderRig.weight;
        holderRig.weight = Mathf.Lerp(holderRig.weight, holdingRigWeight, Time.deltaTime * 15f);

        if(holderRig.weight < 0.1f && _olderRigWeight > holderRig.weight) holderRig.weight = 0;  

        if(thirdPersonController._isInteracting)
            return;
        
        Vector3 mouseWorldPosition = Vector3.zero;

        if(weaponEquiped && activeItemGameObject == null) aimRigWeight = 1;
        else aimRigWeight = 0;

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        Ray ray = new Ray(Camera.main.transform.position + Camera.main.transform.forward * 4, Camera.main.transform.forward);
        //Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        Transform hitTransform = null;
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 999f, aimColliderLayerMask)) 
        {
            debugTransform.position = raycastHit.point;
            mouseWorldPosition = raycastHit.point;
            hitTransform = raycastHit.transform;
        }

        if (playerInput.aim) 
        {
            aimVirtualCamera.gameObject.SetActive(true);
            thirdPersonController.SetSensitivity(aimSensitivity);
            thirdPersonController.SetRotateOnMove(false);
            SetDirection(mouseWorldPosition);
        } 
        else 
        {
            aimVirtualCamera.gameObject.SetActive(false);
            thirdPersonController.SetSensitivity(normalSensitivity);
            thirdPersonController.SetRotateOnMove(true);
        }

        CheckForInventoryItem();

        if(Inventory.Instance.canSelectItem)
        {
            if(playerInput.useItemInputHealth) SelectItem(Item.Health);
            else if(playerInput.useItemInputAmmo) SelectItem(Item.Ammo);
            else if(playerInput.useItemInputFood) SelectItem(Item.Food);
            else if(playerInput.useItemInputWater) SelectItem(Item.Water);
        }
        
        if(playerInput.pickUp && activeItemGameObject != null)
        {
            UseEquipedItem();
        }

        crossHair.SetActive(weaponEquiped);

        if(playerInput.toggleGun) 
        {
            if(!weaponEquiped)
            {
                weaponEquiped = true;
                thirdPersonController.WeaponEquipedSound();
            }
            else
            {
                weaponEquiped = false;
                thirdPersonController.WeaponEquipedSound();
            }
            
            orbGameObject.SetActive(false);
            activeItemGameObject = null;
            playerInput.toggleGun = false;
            NotHolding();
        }

        if(playerInput.useItemInputOrb) 
        {
            if(orbGameObject != null && Inventory.Instance.hasOrb)
            {
                if(!orbGameObject.activeSelf)
                {
                    if(activeItemGameObject != null)
                        activeItemGameObject.SetActive(false);
                    orbGameObject.SetActive(true);
                    activeItemGameObject = orbGameObject;
                }
                else
                {
                    orbGameObject.SetActive(false);
                    Inventory.Instance.indicator.SetActive(false);
                    activeItemGameObject = null;
                }
            }
            HoldRig();
            playerInput.useItemInputOrb = false;
        }

        if(playerInput.toggleInventory)
        {
            Inventory.Instance.ShowInventoryItem();
            Time.timeScale = 0.5f;
        }
        else
        {
            Inventory.Instance.HideInventoryItem();
            Time.timeScale = 1f;
        }

        ShotGun.SetActive(weaponEquiped);
        hasOrbIndicator.SetActive(Inventory.Instance.hasOrb);

        if (playerInput.shoot && weaponEquiped && !thirdPersonController._isInteracting && playerStats.ammo > 0)
        {
            // Projectile Shoot
            SetDirection(mouseWorldPosition);
            playerStats.ammo--;
            Vector3 aimDir = (mouseWorldPosition - spawnBulletPosition.position).normalized;
            thirdPersonController.StopInteraction();
            aimRigWeight = 0;
            thirdPersonController.AnimShoot();
            Instantiate(pfBulletProjectile, spawnBulletPosition.position, Quaternion.LookRotation(aimDir, Vector3.up));
            //*/
            playerInput.shoot = false;
        }
    }

    void CheckForInventoryItem()
    {
        if(Inventory.Instance.canReceiveInput && playerInput.pickUp)
        {
            thirdPersonController.AnimPickUp();
            Inventory.Instance.AddItem();
        }
    }

    void SelectItem(Item itemType)
    {
        if(activeItemGameObject != null && itemType == Inventory.Instance.activeUsableItem)
        {
            activeItemGameObject.SetActive(false);
            Inventory.Instance.activeUsableItem = Item.Null;
            NotHolding();
            ResetInput();
        }
        else
        {
            Inventory.Instance.SelectActiveItem(itemType);
            SelectActiveGameObject(itemType); 
            ResetInput();     
            weaponEquiped = false;
        }
    }

    void SelectActiveGameObject(Item itemType)
    {
        int itemValue = ((int)itemType) - 1;

        for (int i = 0; i < itemGameObjects.Length; i++)
        {
            if(i == itemValue && itemType != Item.Null && Inventory.Instance.numberOfItems[itemType] > 0)
            {
                itemGameObjects[i].SetActive(true);
                HoldRig();
                activeItemGameObject = itemGameObjects[i];
            }
            else
            {
                itemGameObjects[i].SetActive(false);
            }
        }
    }

    void HoldRig()
    {
        holdingRigWeight = 1;
        aimRigWeight = 0;   
        weaponEquiped = false;
    }

    void NotHolding()
    {
        holdingRigWeight = 0;
        aimRigWeight = 1;
        activeItemGameObject = null;
        if(activeItemGameObject != null) 
            activeItemGameObject.SetActive(false);
    }

    void UseEquipedItem()
    {
        if(Inventory.Instance.activeUsableItem != Item.Null && Inventory.Instance.activeUsableItem != Item.Orb)
        {
            if(Inventory.Instance.numberOfItems[Inventory.Instance.activeUsableItem] > 0)
            {
                Inventory.Instance.sfx.PlayOneShot(Inventory.Instance.useSfx);
                activeItemGameObject.GetComponent<I_InventoryItem>().Use();
            }
        }
        if (activeItemGameObject != null && activeItemGameObject.GetComponent<Orb>() != null)
            activeItemGameObject.GetComponent<I_InventoryItem>().Use();
    }

    public void OnUsedEquiped()
    {
        SelectActiveGameObject(Item.Null);
        NotHolding();
    }

    void ResetInput()
    {
        playerInput.useItemInputHealth = false;
        playerInput.useItemInputAmmo = false;
        playerInput.useItemInputFood = false;
        playerInput.useItemInputWater = false;
    }

    void SetDirection(Vector3 worldDir)
    {
        Vector3 worldAimTarget = worldDir;
        worldAimTarget.y = transform.position.y;
        Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * 20f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, enemyInRange);
    }
}