using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] float boundary = 8f;
    [SerializeField] float smoothTime = 0.5f;
    private Vector3 velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(player.transform.position.x, transform.position.y, -10f);
        if (Mathf.Abs(transform.position.y - player.transform.position.y) > boundary)
        {
            if (transform.position.y > player.transform.position.y) transform.position = Vector3.SmoothDamp(transform.position, new Vector3(player.transform.position.x, transform.position.y - boundary, transform.position.z),ref velocity, smoothTime);
            else transform.position = Vector3.SmoothDamp(transform.position, new Vector3(player.transform.position.x, transform.position.y + boundary, transform.position.z), ref velocity, smoothTime);
        }

    }
}
