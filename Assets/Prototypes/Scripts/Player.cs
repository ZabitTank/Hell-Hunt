using System.Collections;
using System.Collections.Generic;
using TreeEditor;
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

    // Input
    float horizontalInput;
    float verticalInput;
    Vector2 mousePosition;
    Vector2 lastMousePosition;
    Vector2 playerToMouse;
    // State
    bool isMoving;
    bool isMouseChange;
    Vector2 movingDirection;

    // Component
    Rigidbody2D rb;
    AudioSource audioSource;
    
    //References
    public Animator body;
    public Animator feet;
    public AnimatorOverrideController[] animatorOverride;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        //rb = GetComponentInChildren<Rigidbody2D>();
    }
    void Update()
    {
        GetPlayerInput();
        SetMovingState();
        SetCharacterRotation();
        SetMovingAnimation();
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            body.SetTrigger("Melee");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            body.runtimeAnimatorController = animatorOverride[0];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            body.runtimeAnimatorController = animatorOverride[1];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            body.runtimeAnimatorController = animatorOverride[2];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            body.runtimeAnimatorController = animatorOverride[3];
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            body.runtimeAnimatorController = animatorOverride[4];
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
            feet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            if (feet.transform.localEulerAngles.z > 90 && feet.transform.localEulerAngles.z < 270)
            {
                feet.transform.localScale = new(-1, -1, -1);
            }
            else
            {
                feet.transform.localScale = new(1, 1, 1);
            }
        };

    }

    private void SetMovingAnimation()
    {
        feet.SetBool("Move", isMoving);
        body.SetBool("Move", isMoving);
    }

}
