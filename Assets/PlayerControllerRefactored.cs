using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRefactored : MonoBehaviour
{

    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] Animator playerAnimator;
    [SerializeField] CapsuleCollider2D playerCollider;

    [Header("Settings")]
    [SerializeField] float speed = 80f;
    [SerializeField] float jumpForce = 60f;
    [SerializeField] float speedMultiplier = 2f;
    [SerializeField] float gravityMod = 3f;
    [SerializeField] float mediumFall = 25f;
    [SerializeField] float highFall = 40f;


    float baseGravityValue;
    bool highGravity = false;
    bool baseGravity = true;

    Vector3 triggerColliderOffset;
    float horizontalInput;
    float playerSpeed;
    bool isOnGround = true;
    bool performJump = false;
    bool performDodge = false;
    bool isJumping = false;
    public bool isDodging = false;
    bool isFacingRight = true;
    bool colliderRestored = true;


    void Start()
    {
        baseGravityValue = playerRb.gravityScale;
    }

    void Update()
    {
        triggerColliderOffset = isFacingRight ? transform.GetComponent<CircleCollider2D>().offset : transform.GetComponent<CircleCollider2D>().offset * new Vector3(-1f, 1f, 1f);

        if (!isDodging)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            if (!colliderRestored) RestoreCollider();
        }

        if (Input.GetButtonDown("Jump") && isOnGround) performJump = true;


        if (!isJumping)
        {

            if (Input.GetButtonDown("Dodge") && horizontalInput != 0f && !isDodging) performDodge = true;

            if (horizontalInput < 0) isFacingRight = false;
            else if (horizontalInput > 0) isFacingRight = true;
            else playerAnimator.SetBool("isWalking", false);
            FlipSprite();


            if (Input.GetButton("Sprint") && isOnGround && horizontalInput != 0f)
            {
                Sprint();
                playerSpeed = speed * speedMultiplier;
            }
            else if (isOnGround)
            {
                playerAnimator.SetBool("isRunning", false);
                playerSpeed = speed;
            }
        }

        if (isJumping && playerRb.velocity.y < 0 && !highGravity) HighGravity();
        else if (!isJumping && !isOnGround && playerRb.velocity.y < 0 && !highGravity) HighGravity();
        else if (isOnGround && !baseGravity) BaseGravity();

        if (playerRb.velocity.y == 0 && !isOnGround && highGravity) isOnGround = CheckFloor(2f);

    }


    private void FixedUpdate()
    {
        MoveLR();
        if (performJump) Jump();
        if (performDodge) Dodge();
    }

    void FlipSprite()
    {
        GetComponentInChildren<Transform>().localScale = isFacingRight ? Vector3.one : new Vector3(-1f, 1f, 1f);
    }

    private void MoveLR()
    {
        playerRb.AddForce(new Vector2(horizontalInput * playerSpeed, 0f));
        playerAnimator.SetBool("isWalking", true);
    }

    private void Sprint()
    {
        playerAnimator.SetBool("isRunning", true);
    }

    private void Jump()
    {
        isOnGround = false;
        playerAnimator.SetBool("isOnGround", isOnGround);
        isJumping = true;
        performJump = false;
        playerAnimator.SetTrigger("jump");
        playerRb.velocity = new Vector2(playerRb.velocity.x, 0f);
        playerRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
    }

    void Dodge()
    {
        performDodge = false;
        isDodging = true;
        playerAnimator.SetTrigger("dodge");
        playerAnimator.SetBool("isDodging", true);
        ReduceCollider();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            Debug.Log("Collision With Floor + isOnGround = " + isOnGround);

            isOnGround = CheckFloor(2f);
            playerAnimator.SetBool("isOnGround", isOnGround);
            isJumping = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor") && !isJumping)
        {
            isOnGround = CheckFloor(2f);
            playerAnimator.SetBool("isOnGround", isOnGround);
            if (!isOnGround) Falling();
        }
    }

    public void Falling()
    {
        if (!CheckFloor(highFall))
        {
            ZeroGravity();
            playerAnimator.SetTrigger("highFall");
        }
        else if (!CheckFloor(mediumFall)) playerAnimator.SetTrigger("mediumFall");
    }

    void ReduceCollider()
    {
        playerCollider.size = new Vector2(1.5f, 2.74f / 2f);
        playerCollider.offset = new Vector2(0.18f, -.42f);
        colliderRestored = false;
        Debug.Log("ReduceColl");
    }

    public void RestoreCollider()
    {
        playerCollider.size = new Vector2(1.5f, 2.74f);
        playerCollider.offset = new Vector2(0.18f, 0.25f);
        colliderRestored = true;
        Debug.Log("RestoreColl");
    }

    public void HighGravity()
    {
        playerRb.gravityScale = gravityMod;
        highGravity = true;
        baseGravity = false;
    }

    public void ZeroGravity()
    {
        playerRb.gravityScale = 0f;
        highGravity = false;
        baseGravity = false;
        playerRb.velocity = Vector2.zero;
        playerSpeed = speed * 0.1f;
    }

    void BaseGravity()
    {
        highGravity = false;
        baseGravity = true;
        playerRb.gravityScale = baseGravityValue;
    }

    bool CheckFloor(float distance)
    {
        RaycastHit2D raycastHit2D;
        raycastHit2D = Physics2D.Raycast(transform.position + triggerColliderOffset, Vector2.down, 10000f, 8);
        if (raycastHit2D.collider != null)
        {
            if (Mathf.Abs(raycastHit2D.point.y - transform.position.y) < distance)
            {
                Debug.DrawLine(transform.position + triggerColliderOffset, transform.position + triggerColliderOffset + (Vector3.down * distance), Color.green, 1f, false);
                return true;
            }
            else
            {
                Debug.DrawLine(transform.position + triggerColliderOffset, transform.position + triggerColliderOffset + (Vector3.down * distance), Color.red, 1f, false);
                return false;
            }
        }
        else
        {
            Debug.DrawLine(transform.position + triggerColliderOffset, transform.position + triggerColliderOffset + (Vector3.down * distance), Color.red, 1f, false);
            return false;
        }
    }

}
