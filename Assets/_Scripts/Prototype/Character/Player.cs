using System;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviour
{
    // Character's Attributes
    public DynamicCharacterStat playerStats;

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

    public BaseWeapon weapon;

    private GameObject detectObject;
    public LayerMask detectableLayer;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        characterController = GetComponentInChildren<CharacterController>();
        weapon = GetComponentInChildren<BaseWeapon>();
    }

    public void Start()
    {
        playerStats.HP.RegisterBaseModEvent(() =>
        {
            if (playerStats.HP.BaseValue <= 0)
            {
                SceneSetting.Instance.isPlayerDead = true;
                var components = GetComponentsInChildren<Component>();
                foreach(var component in components)
                {
                    if(component.GetType() != typeof(Transform))
                    {
                        Destroy(component);
                    }
                        SceneSetting.Instance.UILose.SetActive(true);
                }
            }
        });
        InitState();
    }
    void Update()
    {
        GetPlayerInput();
        HandleMovingAnimation();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            weapon.DoPrimaryAttack();
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            weapon.DoSeconddaryAttack();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            weapon.PreparePrimaryAttack();
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
            GlobalVariable.Instance.playerReferences.inventoryUI.SwapActiveUnActive();
            MouseData.highLightSlot = null;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            GlobalVariable.Instance.playerReferences.equipmentUI.SwapActiveUnActive();
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
        if (Input.GetKeyUp(KeyCode.P))
        {
            GameManager.Instance.SwapPauseResumeGame();
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(playerStats.GetStatValue(EquipmentAttribute.Movement) * Time.fixedDeltaTime * characterController.movingDirection, Space.World);
        //rb.MovePosition(transform.position);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((detectableLayer & (1 << collision.gameObject.layer)) != 0)
        {
            detectObject = collision.gameObject;
            detectObject.GetComponent<Renderer>().material.color = Color.yellow;
        }

        // if (collision.tag = "NextLevel")
        // {
        //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        // }
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
        playerStats.HP.UpdateBaseValue(damage);
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
            var slots = MouseData.highLightSlot;
            var item = slots.Item;
            if (!item) return;
            switch (item.type)
            {
                case ItemType.Ammunition:
                    var ammo = (Ammunition)item;
                    var magazine = playerStats.GetMagazine[ammo.gunType];
                    magazine.UpdateBaseValue(slots.amount);
                    slots.UpdateSlot(new(), 0);
                    MouseData.highLightSlot = null;
                    break;

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
        inventory = GlobalVariable.Instance.playerReferences.playerInventory;
        inventory.setParent(gameObject);

        equipment = GlobalVariable.Instance.playerReferences.playerEquipment;
        equipment.setParent(gameObject);

        playerStats.SetParent(this);
        if (!playerStats.playerCurrentWeapon)
            playerStats.playerCurrentWeapon = playerStats.playerDefaultWeapon;

        characterController.setParent(gameObject);


        weapon.characterController = characterController;
        weapon.playerStats = playerStats;
        weapon.ChangeWeapon(playerStats.playerCurrentWeapon);
    }
}