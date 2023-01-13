using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // Character's Attributes
    public DynamicCharacterStat stats;

    // Input
    float horizontalInput;
    float verticalInput;
    Vector2 mousePosition;
    Vector2 lastMousePosition;
    Vector2 playerToMouse;

    // Component
    public CharacterController characterController;
    Rigidbody2D rb;
    AudioSource audioSource;

    //References
    private InventoryUI inventoryUI;
    private InventoryUI equipmentUI;

    private Inventory inventory;
    public Inventory Inventory
    {
        get { return inventory; }
    }

    private Inventory equipment;
    public Inventory Equipment
    {
        get { return equipment; }
        private set { equipment = value; }
    }

    public BaseWeapon playerWeapon;

    private GameObject detectObject;
    public LayerMask detectableLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        characterController = GetComponentInChildren<CharacterController>();
        playerWeapon = GetComponentInChildren<BaseWeapon>();
    }

    public void Start()
    {
        stats.HP.RegisterBaseModEvent(() =>
        {
            if (stats.HP.BaseValue <= 0) Destroy(gameObject);
        });
        InitState();
    }
    void Update()
    {
        GetPlayerInput();
        HandleMovingAnimation();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            playerWeapon.DoPrimaryAttack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            playerWeapon.DoSeconddaryAttack();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            playerWeapon.PreparePrimaryAttack();
        }

        if (Input.GetKeyDown(KeyCode.Home))
        {
            inventory.Save();
            equipment.Save();
        }
        if (Input.GetKeyDown(KeyCode.End))
        {
            inventory.Load();
            equipment.Load();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            inventoryUI.SwapActiveUnActive();
            MouseData.highLightSlot = null;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            equipmentUI.SwapActiveUnActive();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            // try raycast
            PickupItem();
        }
        if (Input.GetKeyUp(KeyCode.G))
        {
            inventory.DropSlotInScene(MouseData.highLightSlot);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            UseHightlightItem();
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(stats.GetStatValue(EquipmentAttribute.Movement) * Time.fixedDeltaTime * characterController.movingDirection, Space.World);
        //rb.MovePosition(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((detectableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            detectObject = collision.gameObject;
            detectObject.GetComponent<Renderer>().material.color = Color.yellow;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (detectObject && (detectableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            detectObject.GetComponent<Renderer>().material.color = Color.white;
            detectObject = null;
        }

    }

    public void TakeDamge(int damage)
    {
        stats.HP.UpdateBaseValue(damage);
        Instantiate(GlobalVariable.Instance.bloodEffectPrefab, transform.position, transform.rotation, null);
    }

    private void GetPlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        mousePosition = Input.mousePosition;
    }

    private void HandleMovingAnimation()
    {
        if (mousePosition != lastMousePosition && !EventSystem.current.IsPointerOverGameObject())
        {
            lastMousePosition = mousePosition;
            playerToMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            playerToMouse.Normalize();
        }
        characterController.HandleStateWithMouse(horizontalInput, verticalInput, playerToMouse);
    }
    private void PickupItem()
    {
        if (!detectObject) return;

        BaseItem baseItem = detectObject.GetComponent<BaseItem>();
        if (baseItem)
        {
            inventory.AddItem(baseItem.item.itemRef, baseItem.amount);
            Destroy(baseItem.gameObject);
            GlobalAudio.Instance.PlaySwapSlot();
        }
    }

    private void UseHightlightItem()
    {
        if (inventory.GetSlots.Contains(MouseData.highLightSlot))
        {
            var item = MouseData.highLightSlot.Item;
            if (!item) return;
            switch (item.type)
            {
                case ItemType.Gun:
                case ItemType.MeleeWeapon:
                    var weaponSlot = equipment.GetSlots[2];
                    inventory.SwapSlot(MouseData.highLightSlot, weaponSlot);
                    break;
                case ItemType.Headgear:
                    var headGearSlot = equipment.GetSlots[0];
                    inventory.SwapSlot(MouseData.highLightSlot, headGearSlot);
                    break;
                case ItemType.Armor:
                    var armorSlot = equipment.GetSlots[1];
                    inventory.SwapSlot(MouseData.highLightSlot, armorSlot);
                    break;
            }
        }
    }

    private void InitState()
    {
        inventoryUI = GlobalVariable.Instance.playerReferences.inventoryUI;
        equipmentUI = GlobalVariable.Instance.playerReferences.equipmentUI;

        inventory = GlobalVariable.Instance.playerReferences.playerInventory;
        equipment = GlobalVariable.Instance.playerReferences.playerEquipment;

        stats.SetParent(this);
        characterController.setParent(gameObject);
        playerWeapon.characterController = characterController;
        inventory.setParent(gameObject);

        if (!stats.playerCurrentWeapon)
            stats.playerCurrentWeapon = stats.playerDefaultWeapon;

        playerWeapon.ChangeWeapon(stats.playerCurrentWeapon);
        inventoryUI.SwapActiveUnActive();
        equipmentUI.SwapActiveUnActive();
    }
}