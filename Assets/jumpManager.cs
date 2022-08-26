using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpManager : MonoBehaviour
{

    public bool isJumping = false;
    public bool canJump = true;
    [SerializeField] Rigidbody2D playerRb;
    [SerializeField] float gravityMod = 3f;
    float baseGravityValue;

    // Start is called before the first frame update
    void Start()
    {
        baseGravityValue = playerRb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerRb.velocity.y < 0) playerRb.gravityScale = gravityMod;
        else playerRb.gravityScale = baseGravityValue;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor"))
        {
            SetCanJumpTrue();
            SetIsJumpingFalse();
        }
    }
}
