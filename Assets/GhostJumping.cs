using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostJumping : MonoBehaviour
{

    [SerializeField] GameObject ghost;
    [SerializeField] Animator ghostAnimator;


    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (ghostAnimator.GetBool("animDone"))
            {
                ghostAnimator.SetBool("animDone", false);
            }
            ghost.SetActive(true);
            ghostAnimator.SetTrigger("jump");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ghost.SetActive(false);
        }
    }
}
