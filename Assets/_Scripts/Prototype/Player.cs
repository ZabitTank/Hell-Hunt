using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Character's Attributes
    [SerializeField]
    [Header("Movement Speed of character")]
    private float movementSpeed;

    [SerializeField]
    [Header("Max HP of charracter")]
    private int MaxHP;

    [SerializeField]
    [Header("Current HP of character")]
    private int currentHP;

    public Inventory inventory;

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
    bool isShowInventory;
    bool isPickupItem;

    // Component
    Rigidbody2D rb;
    AudioSource audioSource;
    
    //References
    public Animator bodyAnimator;
    public Animator feetAnimator;
    public AnimatorOverrideController[] animatorOverride;

    public GameObject inventoryUI;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        isShowInventory = true;
    }
    public void Start()
    {

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
        }
        if (Input.GetKeyDown(KeyCode.End))
        {
            inventory.Load();
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isShowInventory = !isShowInventory;
            inventoryUI.SetActive(isShowInventory);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            // try raycast
            PickupItem();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            DropSelectItem();
        }

    }

    private void FixedUpdate()
    {
        transform.Translate(movementSpeed * Time.fixedDeltaTime * movingDirection,Space.World);
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
            ItemRef itemRef = new(baseItem.item);
            inventory.AddItem(itemRef, 1);
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
                    inventory.AddItem(new(baseItem.item), 1);
                    Destroy(baseItem.gameObject);
                }
            }
        }
    }

    private void DropSelectItem()
    {
        ItemRef itemRef = inventory.currentSelectSlot.itemRef;
        Item item = inventory.database.getItem[itemRef.id];
        inventory.RemoveItem(inventory.currentSelectSlot.itemRef);
        Instantiate(item.prefabs, transform.position, Quaternion.identity);
    }

    private void OnApplicationQuit()
    {
        inventory.container = new InventorySlot[16];
    }


}
