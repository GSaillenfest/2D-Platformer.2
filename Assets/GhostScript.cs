using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{
    [SerializeField] Vector3 startPos;


    private void OnEnable()
    {
        transform.position = startPos;
    }

    private void OnDisable()
    {
        transform.position = startPos;
    }
}
