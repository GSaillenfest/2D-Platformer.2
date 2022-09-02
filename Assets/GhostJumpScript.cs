using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostJumpScript : MonoBehaviour
{
    [SerializeField] Vector3 startPos;


    private void OnEnable()
    {
        transform.GetComponentInChildren<Animator>().enabled = true;
        transform.GetComponentInChildren<Animator>().SetBool("animate", true);
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = true;
        }
    }

    private void OnDisable()
    {
        transform.GetComponentInChildren<Animator>().SetBool("animate", false);
        transform.GetComponentInChildren<Animator>().enabled = false;
        foreach (SpriteRenderer renderer in transform.GetComponentsInChildren<SpriteRenderer>())
        {
            renderer.enabled = false;
        }
    }
}
