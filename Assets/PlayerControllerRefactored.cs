using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerRefactored : MonoBehaviour
{

    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] float speed = 80f;
    [SerializeField] float jumpForce = 60f;
    [SerializeField] float speedMultiplier = 2f;

    Vector3 triggerColliderOffset;
    float horizontalInput;
    float playerSpeed;
    bool isOnGround = true;
    bool performJump = false;
    bool isJumping = false;
    bool isFacingRight = true;


    void Start()
    {

    }

    void Update()
    {
        if (isFacingRight) triggerColliderOffset = transform.GetComponent<CircleCollider2D>().offset;
        else triggerColliderOffset = transform.GetComponent<CircleCollider2D>().offset * new Vector3(-1f, 1f, 1f);

        horizontalInput = Input.GetAxisRaw("Horizontal");
        
        if (Input.GetButtonDown("Jump") && isOnGround) performJump = true;

        if (!isJumping)
        {
            //if (Input.GetButtonDown("Crouch") && horizontalMovement != 0f)
            //{
            //    performADodge = true;
            //}

            if (horizontalInput < 0) isFacingRight = false;
            else if (horizontalInput > 0) isFacingRight = true;
            FlipSprite();


            if (Input.GetButton("Sprint") && isOnGround) playerSpeed = speed * speedMultiplier;
            else playerSpeed = speed;
        }

    }


    private void FixedUpdate()
    {
        MoveLR();
        if (performJump) Jump();
    }

    private void MoveLR()
    {
        playerRb.AddForce(new Vector2(horizontalInput * playerSpeed, 0f));
    }

    private void Sprint()
    {
        
    }

    private void Jump()
    {
        isOnGround = false;
        isJumping = true;
        performJump = false;
        playerRb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        Debug.Log("Jump : " + "performJump = " + performJump);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            Debug.Log("Collision With Floor");
            isOnGround = CheckFloor(2f);
            isJumping = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor") && !isJumping)
        {
            isOnGround = CheckFloor(2f);
            if (!isOnGround) Debug.Log("Not on Floor");
        }
    }

    public bool CheckFloor(float distance)
    {
        RaycastHit2D raycastHit2D;
        raycastHit2D = Physics2D.Raycast(transform.position + triggerColliderOffset, Vector2.down, 10000f, 8);
        Debug.DrawLine(transform.position + triggerColliderOffset, transform.position + triggerColliderOffset + (Vector3.down * 10000f), Color.green, 2f, false);
        if (raycastHit2D.collider != null)
        {
            if (Mathf.Abs(raycastHit2D.point.y - transform.position.y) < distance) return true;
            else return false;
        }
        else return false;
    }

    void FlipSprite()
    {
        if (isFacingRight)
        {
            GetComponentInChildren<Transform>().localScale = Vector3.one;
        }
        else GetComponentInChildren<Transform>().localScale = new Vector3(-1f, 1f, 1f);
    }
}
