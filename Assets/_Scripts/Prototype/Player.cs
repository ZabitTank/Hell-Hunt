using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    // Character's Attributes
    public CharacterStat stats;

    // Input
    float horizontalInput;
    float verticalInput;
    Vector2 mousePosition;
    Vector2 lastMousePosition;
    Vector2 playerToMouse;

    //State
    bool isMoving;
    bool isMouseChange;
    Vector2 movingDirection;

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

    public PlayerWeapon playerWeapon;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

    }
    public void Start()
    {
        InitState();
    }
    void Update()
    {
        GetPlayerInput();
        SetMovingState();
        characterController.SetCharacterRotation(movingDirection,isMoving,playerToMouse,isMouseChange);
        characterController.SetMovingAnimation(isMoving);

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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryUI.SwapActiveUnActive();
            MouseData.highLightSlot = null;
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
        if (Input.GetKeyUp(KeyCode.G))
        {
            inventory.DropSlotInScene(MouseData.highLightSlot);
        }
        if (Input.GetKeyUp(KeyCode.E))
        {
            UseHightlightItem();
        }
        transform.Translate(stats.GetStatValue(EquipmentAttribute.Movement) * Time.fixedDeltaTime * movingDirection,Space.World);
        //rb.MovePosition(transform.position);
    }

    private void GetPlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //float distance = Vector3.Distance(gameObject.transform.position, Camera.main.ScreenToWorldPoint(mousePosition));
        mousePosition = Input.mousePosition;
    }

    private void SetMovingState()
    {
        movingDirection = new(horizontalInput, verticalInput);
        isMoving = movingDirection.magnitude > 0.00f;
        
        if(mousePosition != lastMousePosition)
        {
            lastMousePosition = mousePosition;
            isMouseChange = true;
            playerToMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            playerToMouse.Normalize();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        BaseItem baseItem = collision.GetComponent<BaseItem>();
        if (baseItem)
        {
            inventory.AddItem(baseItem.item.itemRef, 1);
            Destroy(baseItem.gameObject);
        }
    }
    private void PickupItem()
    {
        if(Physics.Raycast(transform.position, -Vector3.up, out RaycastHit hit, 10))
        {
            Debug.Log(hit.collider.gameObject);
            if(hit.collider.gameObject.CompareTag("Item"))
            {
                BaseItem baseItem = hit.collider.gameObject.GetComponent<BaseItem>();
                if (baseItem)
                {
                    inventory.AddItem(baseItem.item.itemRef, 1);
                    Destroy(baseItem.gameObject);
                }
            }
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
        playerWeapon.parent = this;
        inventory.setParent(gameObject);
        characterController.setParent(gameObject);

        if (!stats.playerCurrentWeapon)
            stats.playerCurrentWeapon = stats.playerDefaultWeapon;

        playerWeapon.ChangeWeapon(stats.playerCurrentWeapon);


    }
}