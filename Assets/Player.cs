using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region components
    private Rigidbody2D rb; // rb - rigid body
    private Animator anim;
    #endregion

    [Header("Movement Info")]
    public float moveSpeed;
    public float maxMoveSpeed;
    private float defaultMoveSpeed;
    private bool canRun = false;

    private float speedMilestone;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private float speedIncreaseMilestone;
                     private float defaultSpeedIncreaseMilestone;

    [Header("Jump info")]
    public float jumpForce;
    public float doubleJumpForce;

    private float defaultJumpForce;
    private bool canDoubleJump;

    [Header("Slide info")]
    public float slideSpeedMultiplier;
    private bool isSliding;
    private bool canSlide;

    [SerializeField] private float slidingCooldown;
    [SerializeField] private float slidingTime;
                     private float slidingBegun;



    [Header("Collision Detection")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask whatIsGround;
    private bool isGrounded;
    private bool isRunning;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        defaultJumpForce = jumpForce;
        defaultMoveSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            canRun = true;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            speedReset();
        }
        
        checkForRun();
        checkForJump();
        checkForSlide();

        checkForCollision();
        AnimationControllers();
    }

    private void AnimationControllers()
    {
        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isRunning", isRunning);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isSliding", isSliding);
    }

    private void checkForRun()
    {
        if (transform.position.x > speedMilestone)
        {
            speedMilestone += speedIncreaseMilestone;

            moveSpeed = moveSpeed * speedMultiplier;
            speedIncreaseMilestone = speedIncreaseMilestone * speedMultiplier;

            if (moveSpeed > maxMoveSpeed)
            {
                moveSpeed = maxMoveSpeed;
            }
        }
        
        
        
        if (isSliding)
        {
            rb.velocity = new Vector2(moveSpeed * slideSpeedMultiplier, rb.velocity.y);
        }
        else if (canRun == true)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }

        if (rb.velocity.x > 0)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }
    }

    private void checkForJump()
    {
        if (Input.GetKeyDown("space") || Input.GetKeyDown(KeyCode.Mouse0))
        {

            if (isGrounded)
            {
                jump();
            }
            else if (canDoubleJump)
            {
                jumpForce = doubleJumpForce;
                jump();
                canDoubleJump = false;
            }
        }

        if (isGrounded)
        {
            jumpForce = defaultJumpForce;
            canDoubleJump = true;
        }

    }

    private void checkForSlide()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canSlide && isGrounded)
        {
            Debug.Log("isSliding");
            isSliding = true;
            canSlide = false;
            slidingBegun = Time.time;

        }

        if (Time.time > slidingBegun + slidingTime)
        {
            isSliding = false;
        }

        if (Time.time > slidingBegun + slidingCooldown)
        {
            canSlide = true;
        }
    }
    private void jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }

    private void speedReset()
    {
        moveSpeed = defaultMoveSpeed;
        speedIncreaseMilestone = 50;
    }
    private void checkForCollision()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
