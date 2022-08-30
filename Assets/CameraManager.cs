using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] float boundary = 8f;
    [SerializeField] float smoothTime = 0.5f;
    private Vector3 velocity = Vector3.zero;
    Vector3 targetPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if (Mathf.Abs(transform.position.x - player.transform.position.x) > boundary)
        //{
            targetPos = new Vector3 (player.transform.position.x, transform.position.y , -10f);
        //}
        
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);


    }
}
