using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float startTimer = 100.0f;
    public float currentTimer;
    public float timerPercent;
    public Image image;
    private bool gameActive = true;
    public Canvas restart;
    public GameObject foofa;


    // Start is called before the first frame update
    void Start()
    {
        currentTimer = startTimer;
        image = GetComponent<Image>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(gameActive)
        {
        currentTimer -= Time.deltaTime;
        timerPercent = currentTimer/startTimer;
        image.fillAmount = timerPercent;
        }
        
        if(currentTimer <= 0.0f)
        {
            gameActive = false;
            Invoke ("RestartGame", 0.1f);
        }
    }

     private void RestartGame()
     {
        foofa.GetComponent<AudioSource>().Pause();
        foofa.transform.Find("Fail").GetComponent<AudioSource>().Play();
        restart.gameObject.SetActive(true);
        Time.timeScale = 0;
     }
}
