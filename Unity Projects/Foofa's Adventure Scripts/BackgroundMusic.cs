using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    public bool isMusicPlaying = true;
    public AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) {
        isMusicPlaying = !isMusicPlaying;
        if (isMusicPlaying) {
            audioSource.Play();
        } else {
            audioSource.Stop();
        }
    }
    }
}
