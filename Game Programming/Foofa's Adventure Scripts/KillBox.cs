using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillBox : MonoBehaviour
{
    
    // Start is called before the first frame update
    public PlayerMovement playerMovement;
    public GameObject foofa;
    public Canvas restart;
    
     // Reference to the GameController script

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is the player character
        if (collision.CompareTag("Player"))
        {
             // Restart the game after a short delay
            playerMovement.lives--;
            if (playerMovement.lives == 0)
            {
             Invoke("RestartGame", 0.5f);
            }
            else
            {
                Invoke("SetFoofaPosition", 1f);
            }
         }
    }

     private void RestartGame()
     {
        foofa.GetComponent<AudioSource>().Pause();
        foofa.transform.Find("Fail").GetComponent<AudioSource>().Play();
        restart.gameObject.SetActive(true);
        Time.timeScale = 0;
     }
     
     private void SetFoofaPosition()
    {
    foofa.transform.position = new Vector3(-0.05f, 0.09f, -12.21079f);
    }

}
