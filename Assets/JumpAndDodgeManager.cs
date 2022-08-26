using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAndDodgeManager : MonoBehaviour
{

    public bool isJumping = false;
    public bool isDodging = false;
    public bool canJump = true;
    public bool fall;
    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] CapsuleCollider2D playerCollider;
    [SerializeField] float gravityMod = 3f;
    float baseGravityValue;
    bool floor = true;

    // Start is called before the first frame update
    void Start()
    {
        baseGravityValue = playerRb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRb.velocity.y < 0 && isJumping && !fall) HighGravity();
        else if (!isJumping && !fall) BaseGravity();
    }

    public void SetIsJumpingFalse()
    {
        isJumping = false;
    }

    public void SetIsJumpingTrue()
    {
        isJumping = true;
        floor = false;
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
        floor = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            SetCanJumpTrue();
            SetIsJumpingFalse();
            floor = true;
            if (fall) GetComponentInParent<PlayerController>().Land();
            fall = false;
            BaseGravity();
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
        if (!floor) fall = true;
    }
    
    public void HighGravity()
    {
        playerRb.gravityScale = gravityMod;
    }

    public void ZeroGravity()
    {
        playerRb.gravityScale = 0;
        Debug.Log("ZeroGravity");
    }

    public void BaseGravity()
    {
        playerRb.gravityScale = baseGravityValue;
    }
}
