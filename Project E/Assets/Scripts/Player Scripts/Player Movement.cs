using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{

    public Rigidbody2D rb;
    public Playerinput playerMovement;
    public Playerinput jumpScript;
    //public Animator animator;

    private InputAction move;
    private InputAction jump;


    public float movementSpeed;
    public float jumpForce;
    public int extraJumps;


    private int jumpsLeft;
    private bool isFacingRight = true;


    [SerializeField] private float normalCharGravity;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private void Start()
    {
        jumpsLeft = extraJumps;
        rb.gravityScale *= normalCharGravity ;

    }

    #region necessary input system calls
    private void Awake()
    {
        playerMovement = new();
        jumpScript = new();
    }
    private void OnEnable()
    {
        move = playerMovement.Player.Move;
        move.Enable();

        jump = jumpScript.Player.Jump;
        jump.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
    }

#endregion
    void Update()
    {
        Flip();

        Jump();
        //animator.SetBool("isJumping", !IsGrounded());

        //if (IsGrounded() && jumpsLeft != extraJumps)
        //{
        //    jumpsLeft = extraJumps; 
        //    animator.SetBool("isJumping",false);
        //}
    }


    private void LateUpdate()
    {
        JumpReset();
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        Vector2 direction = move.ReadValue<Vector2>();

        rb.velocity = new Vector2(movementSpeed * direction.x, rb.velocity.y);
        //animator.SetFloat("Speed", math.abs(rb.velocity.x));
    }
    private void Jump()
    {
        if (jumpsLeft > 0 && jump.WasPressedThisFrame())
        {
            //animator.SetBool("isJumping", true);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            rb.gravityScale /= 2;
            --jumpsLeft;
        }

        if (jump.WasReleasedThisFrame())
            rb.gravityScale = normalCharGravity;
    }
    private void JumpReset()
    {
        if (IsGrounded() && jumpsLeft != extraJumps)
        {
            jumpsLeft = extraJumps * Convert.ToInt32(IsGrounded() && jumpsLeft != extraJumps);
        }
    }
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.25f, groundLayer);
    }
    private void Flip()
    {
        float movementSpeed = move.ReadValue<Vector2>().x;
        if (isFacingRight && movementSpeed < 0f || !isFacingRight && movementSpeed > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
