using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] JumpAndDodgeManager jumpAndDogdeManager;

    [Header("Settings")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float speed = 5f;
    [SerializeField] float speedMultiplier = 3f;
    [SerializeField] float mediumFall = 15f;
    [SerializeField] float highFall = 10f;


    float horizontalMovement;
    float multiplier = 1f;
    bool isFacingRight = true;
    bool isJumping = false;
    public bool isDodging = false;
    bool canJump = true;
    bool performAJump = false;
    bool performADodge = false;
    bool sprint = false;
    Vector3 rayCastOffset;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        isJumping = jumpAndDogdeManager.isJumping;
        isDodging = animator.GetBool("isDodging");
        canJump = jumpAndDogdeManager.canJump;
        sprint = false;


        if (!isJumping)
        {
            if (!isDodging)
            {
                if (Input.GetKeyDown(KeyCode.S) && horizontalMovement != 0f)
                {
                    performADodge = true;
                }

                if (horizontalMovement < 0) isFacingRight = false;
                else if (horizontalMovement > 0) isFacingRight = true;
                FlipSprite();
            }


            if (Input.GetKeyDown(KeyCode.Space) && canJump)
            {
                performAJump = true;
                jumpAndDogdeManager.SetCanJumpFalse();
            }

            if (Input.GetKey(KeyCode.LeftShift) && horizontalMovement != 0f)
            {
                sprint = true;
                animator.SetBool("isRunning", true);
                multiplier = speedMultiplier;
                GetComponent<TrailRenderer>().enabled = true;
            }
            else
            {
                animator.SetBool("isRunning", false);
                multiplier = 1f;
                GetComponent<TrailRenderer>().enabled = false;
            }

        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (horizontalMovement != 0)
        {
            animator.SetBool("isWalking", true);
            if (sprint)
            {
                playerRb.AddForce(horizontalMovement * speed * speedMultiplier * Vector2.right);
            }
            else
            {
                playerRb.AddForce(horizontalMovement * speed * Vector2.right);
            }
            animator.SetFloat("multiplier", multiplier);
        }
        else animator.SetBool("isWalking", false);

        if (performAJump)
        {
            Jump();
            performAJump = false;
        }

        if (performADodge)
        {
            Dodge();
            performADodge = false;
        }
    }

    void FlipSprite()
    {
        if (!isFacingRight)
        {
            GetComponentInChildren<Transform>().localScale = new Vector3(-1f, 1f, 1f);
        }
        else GetComponentInChildren<Transform>().localScale = new Vector3(1f, 1f, 1f);
    }

    void Jump()
    {
        playerRb.AddForce(Vector2.up * jumpForce);
        animator.SetTrigger("jump");
        animator.SetBool("isJumping", true);
    }

    void Dodge()
    {
        animator.SetTrigger("dodge");
        Debug.Log("Dodge");
    }

    void FreeFalling()
    {
        if (!jumpAndDogdeManager.isFreeFalling)
        {
            jumpAndDogdeManager.isFreeFalling = true;
            animator.SetTrigger("isFreeFalling");
        }
    }
    void MediumFalling()
    {
        animator.SetBool("mediumFall", true);
    }

    public void StopFalling()
    {
        animator.SetBool("mediumFall", false);
    }

    public void CheckForFloor()
    {
        if (isFacingRight) rayCastOffset = new Vector3(1.5f, -1.5f, 0f);
        else rayCastOffset = new Vector3(-1.5f, -1.5f, 0f);

        RaycastHit2D raycastHit2D;
        raycastHit2D = Physics2D.Raycast(transform.position + rayCastOffset, Vector2.down, mediumFall);
        if (raycastHit2D.collider == null)
        {
            raycastHit2D = Physics2D.Raycast(transform.position + rayCastOffset, Vector2.down, highFall);
            if (raycastHit2D.collider == null)
            {

                FreeFalling();
            }
            else if (raycastHit2D.collider.CompareTag("Floor"))
            { 
                MediumFalling(); 
            }
        }
    }
}
