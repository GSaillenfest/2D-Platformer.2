using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    [SerializeField] Vector3 startPos;


    private void OnEnable()
    {
        transform.position = startPos;
        transform.GetComponentInChildren<Animator>().enabled = true;
    }

    private void OnDisable()
    {
        transform.position = startPos;
        transform.GetComponentInChildren<Animator>().enabled = false;
    }
}
