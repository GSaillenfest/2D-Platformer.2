using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAndDodgeManager : MonoBehaviour
{

    public bool isJumping = false;
    public bool isDodging = false;
    public bool canJump = true;
    public bool isFreeFalling = false;
    public bool fallAnimDone = false;

    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] CapsuleCollider2D playerCollider;
    [SerializeField] PlayerController playerController;
    
    [Header ("Settings")]
    [SerializeField] float gravityMod = 3f;
    
    float baseGravityValue;
    float beforeJumpPos;
    bool highGravity = false;

    // Start is called before the first frame update
    void Start()
    {
        baseGravityValue = playerRb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRb.velocity.y < 0 && isJumping && !isFreeFalling && !highGravity) HighGravity();
        //else if (!isJumping && !isFreeFalling) BaseGravity();

        if (!fallAnimDone && playerRb.velocity.y < 0)
        {
            //if (isJumping && transform.position.y < beforeJumpPos) CheckForFloor();
            if (!isJumping && !isDodging) CheckForFloor();
        }
    }

    public void SetIsJumpingFalse()
    {
        isJumping = false;
    }

    public void SetIsJumpingTrue()
    {
        isJumping = true;
        beforeJumpPos = transform.position.y;
    }

    public void SetCanJumpFalse()
    {
        canJump = false;
        Debug.Log("stopJump");
    }

    public void SetCanJumpTrue()
    {
        canJump = true;
    }

    public void SetIsDodgingFalse()
    {
        isDodging = false;
    }

    public void SetIsDodgingTrue()
    {
        isDodging = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            fallAnimDone = false;
            SetCanJumpTrue();
            SetIsJumpingFalse();
            BaseGravity();
            StopFalling();
            SetIsDodgingFalse();
            Debug.Log("floorContact");
        }
    }

    void ReduceCollider()
    {
        playerCollider.size = new Vector2(1.5f, 2.74f/2f);
        playerCollider.offset = new Vector2(0.18f, -.42f);
        Debug.Log("ReducedColl");
    }

    public void RestoreCollider()
    {
        playerCollider.size = new Vector2(1.5f, 2.74f);
        playerCollider.offset = new Vector2(0.18f, 0.25f);
        Debug.Log("RestoredColl");

    }

    public void CheckForFloor()
    {
        playerController.CheckForFloor();
        Debug.Log("checkingFloor");
    }
    
    public void HighGravity()
    {
        playerRb.gravityScale = gravityMod;
        highGravity = true;
        Debug.Log("HighGravity");
        playerRb.drag = 1f;
    }

    public void ZeroGravity()
    {
        playerRb.gravityScale = 0f;
        highGravity = false;
        playerRb.velocity = new Vector2(0f, 0f);
        playerRb.drag = 5f;
        Debug.Log("ZeroGravity");
    }

    public void BaseGravity()
    {
        highGravity = false;
        playerRb.gravityScale = baseGravityValue;
        playerRb.drag = 1f;
        Debug.Log("BaseGravity");
    }

    public void StopFalling()
    {
        playerController.StopFalling();
        isFreeFalling = false;
    }

    public void SetFallAnimTrue()
    {
        fallAnimDone = true;
    }
}
