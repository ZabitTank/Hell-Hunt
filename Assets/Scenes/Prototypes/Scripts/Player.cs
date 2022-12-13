using System.Collections;
using System.Collections.Generic;
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
    Vector2 movementDirection;

    // Component
    Rigidbody2D rb;
    Animator animator;
    AudioSource audioSource;
    // References

    // Child References

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Start()
    {
        
    }
    void Update()
    {
        GetKeyBoardInputAndSetState();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        LogState();
    }

    private void GetKeyBoardInputAndSetState()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        
        movementDirection = new(horizontalInput, verticalInput);
        isMoving = movementDirection.magnitude > 0.1;

    }

    private void HandleMovement()
    {
        transform.Translate(movementSpeed * Time.fixedDeltaTime * movementDirection, Space.World);
        rb.MovePosition(transform.position);
    }

    private void LogState()
    {
        Debug.Log("Movement " + isMoving.ToString());

    }

}
