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

    [Header("Settings")]
    [SerializeField] float gravityMod = 3f;
    [SerializeField] float baseDrag = 5f;
    [SerializeField] float moddedDrag = 10f;

    float baseGravityValue;
    bool highGravity = false;
    bool baseGravity = true;
    bool floorChecked = false;
    bool colliderRestored = false;

    // Start is called before the first frame update
    void Start()
    {
        baseGravityValue = playerRb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRb.velocity.y < 0f && !isFreeFalling && !highGravity && !isDodging)
        {
            Debug.Log(playerRb.velocity.y);
            HighGravity();
        }
        else if (!baseGravity && !isFreeFalling && playerRb.velocity.y == 0 && !baseGravity)
        {
            BaseGravity();
            floorChecked = false;
        }


        if (!fallAnimDone && playerRb.velocity.y < 0)
        {
            if (!isJumping && !isDodging) CheckForFloor();
        }

        isDodging = playerController.isDodging;
        if (!isDodging && !colliderRestored) RestoreCollider();
    }

    public void SetIsJumpingFalse()
    {
        isJumping = false;
    }

    public void SetIsJumpingTrue()
    {
        isJumping = true;
    }

    public void SetCanJumpFalse()
    {
        canJump = false;
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

    public void CheckForFloor()
    {
        if (!floorChecked)
        {
            playerController.CheckForFloor();
            floorChecked = true;
        }
    }

    public void HighGravity()
    {
        playerRb.gravityScale = gravityMod;
        highGravity = true;
        baseGravity = false;
        playerRb.drag = baseDrag;
    }

    public void ZeroGravity()
    {
        playerRb.gravityScale = 0f;
        highGravity = false;
        baseGravity = false;
        playerRb.velocity = Vector2.zero;
        playerRb.drag = moddedDrag;
    }

    public void BaseGravity()
    {
        highGravity = false;
        baseGravity = true;
        playerRb.gravityScale = baseGravityValue;
        playerRb.drag = baseDrag;
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
    public void SetFallAnimFalse()
    {
        fallAnimDone = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            SetFallAnimFalse();
            SetCanJumpTrue();
            SetIsJumpingFalse();
            BaseGravity();
            StopFalling();
            SetIsDodgingFalse();
            Debug.Log("floorContact");
            floorChecked = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor") && playerRb.velocity.y < -.5f) SetCanJumpFalse();
    }

}
