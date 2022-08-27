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

    // Start is called before the first frame update
    void Start()
    {
        baseGravityValue = playerRb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRb.velocity.y < 0 && isJumping && !isFreeFalling) HighGravity();
        //else if (!isJumping && !isFreeFalling) BaseGravity();

        if (!fallAnimDone && playerRb.velocity.y < 0)
        {
            if (isJumping && transform.position.y < beforeJumpPos + 1f) CheckForFloor();
            else if (!isJumping && !isDodging) CheckForFloor();
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
            Debug.Log("floorContact");
            StopFreeFalling();
        }
    }

    void UnableCollider()
    {
        playerCollider.enabled = false;
        Debug.Log("UncheckedColl");
    }

    public void EnableCollider()
    {
        playerCollider.enabled = true;
    }

    public void CheckForFloor()
    {
        playerController.CheckForFloor();
        Debug.Log("checkingFloor");
    }
    
    public void HighGravity()
    {
        playerRb.gravityScale = gravityMod;
        Debug.Log("HighGravity");

    }

    public void ZeroGravity()
    {
        playerRb.gravityScale = 0f;
        playerRb.velocity = new Vector2(0f, 0f);
        Debug.Log("ZeroGravity");
    }

    public void BaseGravity()
    {
        playerRb.gravityScale = baseGravityValue;
        Debug.Log("BaseGravity");
    }

    public void StopFreeFalling()
    {
        playerController.StopFreeFalling();
        isFreeFalling = false;
    }

    public void SetFallAnimTrue()
    {
        fallAnimDone = true;
    }
}
