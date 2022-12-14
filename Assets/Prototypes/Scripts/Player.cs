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
    Vector2 mouseDirection;

    // State
    bool isMoving;
    Vector2 movingDirection;

    // Component
    Rigidbody2D rb;
    AudioSource audioSource;
    // References

    // Child References
    public Animator body;
    public Animator feet;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Start()
    {
        
    }
    void Update()
    {
        GetKeyBoardInput();
        SetMovingState();
        SetBodyMouseRotation();
        SetFeetAnimation();
    }

    private void FixedUpdate()
    {
        transform.Translate(movementSpeed * movingDirection * Time.fixedDeltaTime);
        rb.MovePosition(transform.position);
    }

    private void GetKeyBoardInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void SetMovingState()
    {
        movingDirection = new(horizontalInput, verticalInput);
        isMoving = movingDirection.magnitude > 0.00f;
    }

    private void SetBodyMouseRotation()
    {
        Vector3 mousePosition = Input.mousePosition;

        float distance = Vector3.Distance(gameObject.transform.position, Camera.main.ScreenToWorldPoint(mousePosition));

        Vector3 facingDirection = Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;


        facingDirection.Normalize();

        float rotation = Mathf.Atan2(facingDirection.y, facingDirection.x) * Mathf.Rad2Deg;
        body.transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    private void SetFeetAnimation()
    {
        feet.SetBool("isMoving", isMoving);
        if (isMoving)
        {
            Vector2 velocity = movingDirection;
            float angle = Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg;
            feet.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (feet.transform.localEulerAngles.z > 90 && feet.transform.localEulerAngles.z < 270)
            {
                feet.transform.localScale = new(-1, -1, -1);
            }
            else
            {
                feet.transform.localScale = new(1, 1, 1);
            }
        }     

    }

    private void LogState()
    {
        Debug.Log("Movement " + isMoving.ToString());

    }

}
