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

    // Start is called before the first frame update
    void Start()
    {
        baseGravityValue = playerRb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRb.velocity.y < 0 && !isFreeFalling && !highGravity && !isDodging) HighGravity();
        else if (!baseGravity && !isFreeFalling && playerRb.velocity.y == 0 && !baseGravity)
        {
            BaseGravity();
            floorChecked = false;
        }


        if (!fallAnimDone && playerRb.velocity.y < 0)
        {
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

    void ReduceCollider()
    {
        playerCollider.size = new Vector2(1.5f, 2.74f / 2f);
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
        if (!floorChecked)
        {
            playerController.CheckForFloor();
            floorChecked = true;
            Debug.Log("checkingFloor");
        }
    }

    public void HighGravity()
    {
        playerRb.gravityScale = gravityMod;
        highGravity = true;
        baseGravity = false;
        Debug.Log("HighGravity");
        playerRb.drag = baseDrag;
    }

    public void ZeroGravity()
    {
        playerRb.gravityScale = 0f;
        highGravity = false;
        baseGravity = false;
        playerRb.velocity = new Vector2(0f, 0f);
        playerRb.drag = moddedDrag;
        Debug.Log("ZeroGravity");
    }

    public void BaseGravity()
    {
        highGravity = false;
        baseGravity = true;
        playerRb.gravityScale = baseGravityValue;
        playerRb.drag = baseDrag;
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

}
