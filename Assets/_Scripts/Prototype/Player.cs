using Newtonsoft.Json.Bson;
using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Character's Attributes
    public CharacterStat characterStat;

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
    Rigidbody2D rb;
    AudioSource audioSource;
    
    //References
    public Animator bodyAnimator;
    public Animator feetAnimator;


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
        SetCharacterRotation();
        SetMovingAnimation();

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
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            // try raycast
            PickupItem();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            inventory.DropSlotInScene(MouseData.highLightSlot);
        }
        if (Input.GetKey(KeyCode.E))
        {
            Item weapon = characterStat.playerCurrentWeapon;
            playerWeapon.ChangeWeapon(weapon);
        }

    }

    private void FixedUpdate()
    {
        transform.Translate(characterStat.GetStatValue(EquipmentAttribute.Movement) * Time.fixedDeltaTime * movingDirection,Space.World);
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

    private void SetCharacterRotation()
    {
        if (isMouseChange)
        {
            float rotation = Mathf.Atan2(playerToMouse.y, playerToMouse.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotation);
        };
        if (isMoving)
        {
            float angle = Mathf.Atan2(movingDirection.y, movingDirection.x) * Mathf.Rad2Deg;
            feetAnimator.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            if (feetAnimator.transform.localEulerAngles.z > 90 && feetAnimator.transform.localEulerAngles.z < 270)
            {
                feetAnimator.transform.localScale = new(-1, -1, -1);
            }
            else
            {
                feetAnimator.transform.localScale = new(1, 1, 1);
            }
        };

    }

    private void SetMovingAnimation()
    {
        feetAnimator.SetBool("Move", isMoving);
        bodyAnimator.SetBool("Move", isMoving);
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

    private void DropSelectItem()
    {

    }

    public void AttributeModified(Attribute attribute)
    {
        
    }

    private void InitState()
    {
        inventoryUI = GlobalVariable.Instance.playerReferences.inventoryUI;
        equipmentUI = GlobalVariable.Instance.playerReferences.equipmentUI;

        inventory = GlobalVariable.Instance.playerReferences.playerInventory;
        equipment = GlobalVariable.Instance.playerReferences.playerEquipment;

        inventory.setParent(gameObject);
        characterStat.SetParent(this);

    }
}