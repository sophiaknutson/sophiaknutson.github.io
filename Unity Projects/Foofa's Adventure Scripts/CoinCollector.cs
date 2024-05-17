using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    public void Start()
    {

    }
    // Start is called before the first frame update
    public int score = 0;  // the player's score
    

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))  // check if the collider's tag is "Coin"
        {
            transform.Find("Collect").GetComponent<AudioSource>().Play();
            Destroy(collision.gameObject);  // destroy the coin object
            score++; 
           
        }
    }
}
