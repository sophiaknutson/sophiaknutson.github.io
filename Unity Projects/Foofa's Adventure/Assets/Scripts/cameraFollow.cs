using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public GameObject target;

    public Vector3 offset;
    public float Smoothness = 0.125f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       Vector3 camPos = transform.position;
       Vector3 PlayerPos = target.transform.position;
       Vector3 desiredPosition = PlayerPos;
       Vector3 smoothPos = Vector3.Lerp(camPos, desiredPosition, Smoothness);

        // if (camPos.x<-12.0f)
        // {
        //     camPos.x = -12.0f;
        // }
        // else if (camPos.x>-220.0f)
        // {
        //     camPos.x = 220.0f;
        // }
       transform.position = smoothPos + offset;
    }
}
