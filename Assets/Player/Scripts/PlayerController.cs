using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Header("Movement Attributes")]
    [SerializeField] private float speed = 13;
    [Range(0, 10)]
    [SerializeField] private float fallMultiplier = 6f;
    [Range(0, 10)]
    [SerializeField] private float jumpMultiplier = 5f;
    [Range(0, 50)]
    [SerializeField] private float jumpForce = 25f;

    private LayerMask playerLayer;
    private Rigidbody2D rb;
    private bool isGrounded;
    private float checkRadius;
    private Transform groundDetector;
    private float horizontal;

    void Start()
    {
        checkRadius = 0.03f;
        rb = GetComponent<Rigidbody2D>();
        playerLayer = LayerMask.NameToLayer("Player");
        groundDetector = transform.Find("groundDetector").transform;
    }

    void Update() 
    {
        Time.timeScale = 1.0f;
        horizontal = Input.GetAxisRaw("Horizontal");

        DetectGround();
        JumpController();
        RotatePlayer();
    }

    void RotatePlayer() 
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Transform riggedPlayer = transform.Find("riggedPlayer");
        
        float aimHorizontalDirection = mousePosition.x - transform.position.x;
        
        if(Time.timeScale != 0) 
        {
            if (aimHorizontalDirection > 0)
                riggedPlayer.rotation = new Quaternion(0, 0, 0, 0);
            else if (aimHorizontalDirection < 0)
                riggedPlayer.rotation = new Quaternion(0, 180, 0, 0);
        }
    }
    
    void FixedUpdate()
    {
        Move();
    }

    void Move() 
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
    }

    void DetectGround() 
    {
        isGrounded = Physics2D.OverlapCircle(groundDetector.position, checkRadius, ~(playerLayer));
    }

    void JumpController() 
    {
        if(isGrounded && Input.GetKeyDown(KeyCode.Space)) 
        {
            rb.velocity = Vector2.up * jumpForce;
        }

        if(rb.velocity.y < 0) 
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if(rb.velocity.y > 0) 
        {
             rb.velocity += Vector2.up * Physics2D.gravity.y * (jumpMultiplier - 1) * Time.deltaTime;   
        }
    }
}
