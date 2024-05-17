using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class End : MonoBehaviour
{
    public Canvas congratulations;
    public GameObject foofa;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the object that entered the trigger is the player character
        if (collision.CompareTag("Player"))
        { 
                Invoke("RestartGame", 0.1f);
        }
    }

    private void RestartGame()
    {
    foofa.GetComponent<AudioSource>().Pause();
    foofa.transform.Find("Success").GetComponent<AudioSource>().Play();
    Time.timeScale = 0;
    congratulations.gameObject.SetActive(true);
    }
}
