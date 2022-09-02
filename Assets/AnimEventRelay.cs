using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventRelay : MonoBehaviour
{

    [SerializeField] PlayerControllerRefactored playerScript;

    public void CheckFallHeight()
    {
        playerScript.Falling();
    }

    public void HighGravity()
    {
        playerScript.HighGravity();
    }

}
