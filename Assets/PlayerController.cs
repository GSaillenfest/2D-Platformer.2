using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header ("Objects")]
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] JumpAndDodgeManager jumpAndDogdeManager;
    
    [Header ("Settings")]
    [SerializeField] float jumpForce = 5f;
    [SerializeField] float speed = 5f;
    [SerializeField] float speedMultiplier = 3f;


    float horizontalMovement;
    float multiplier = 1f;
    bool isFacingRight = true;
    bool isJumping = false;
    bool isDodging = false;
    bool canJump = true;
    bool performAJump = false;
    bool performADodge = false;
    bool sprint = false;
    bool fall = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        horizontalMovement = Input.GetAxis("Horizontal");
        isJumping = jumpAndDogdeManager.isJumping;
        isDodging = jumpAndDogdeManager.isDodging;
        canJump = jumpAndDogdeManager.canJump;
        fall = jumpAndDogdeManager.fall;
        sprint = false;

        if (fall) Fall();

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
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                sprint = true;
                animator.SetBool("isRunning", true);
                multiplier = speedMultiplier;
            }
            else
            {
                animator.SetBool("isRunning", false);
                multiplier = 1f;
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
    }

    void Fall()
    {
        animator.SetBool("fall", true);
    }

     public void Land()
    {
        animator.SetBool("fall", false);
    }

}
