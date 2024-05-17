using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
 
    public GameObject platformObject;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Invoke("DeactivatePlatform", 2.0f); 
        }
    }

    // Deactivates the platform game object
    private void DeactivatePlatform()
    {
        platformObject.SetActive(false);
    }

}
