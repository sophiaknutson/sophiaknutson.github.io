using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    public float cloudSpeed = 1.0f;  // the speed of the cloud

    void Update()
    {
        // Move the cloud in the x direction based on the cloudSpeed variable
        transform.Translate(Vector3.right * Time.deltaTime * cloudSpeed);
        
        // If the cloud moves off the screen, reset its position to the left side of the screen
        if (transform.position.x > 100.0f)
        {
            transform.position = new Vector3(-20.0f, transform.position.y, transform.position.z);
        }
    }
}
