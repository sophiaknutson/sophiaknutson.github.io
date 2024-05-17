using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public int lives = 3;

    public float Speed = 5.0f;
    public Animator anim;
    const string PRESS_ANIM = "press";
    const string CLICK_ANIM = "dclick";
    public Canvas restart;
   
    public float JumpForce;

    Rigidbody2D rb;

    bool Grounded = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float Movex = Input.GetAxis("Horizontal");

         anim.SetFloat("walk", Movex);
         anim.SetFloat("walkleft", Movex);
        
        rb.velocity = new Vector2(Movex * Speed, rb.velocity.y);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && Grounded == true)
        {
            rb.AddForce(transform.up * JumpForce);
            Grounded = false;
            transform.Find("JumpSound").GetComponent<AudioSource>().Play();
        }

        if(Input.GetMouseButtonDown(0))
        {
        anim.SetTrigger(CLICK_ANIM);
        transform.Find("Tickling").GetComponent<AudioSource>().Play();
        }

        if(Input.GetMouseButtonDown(1))
        {
        anim.SetTrigger(PRESS_ANIM);
        transform.Find("Rolling").GetComponent<AudioSource>().Play();
        }
    }

    void OnCollisionEnter2D(Collision2D Col)
    {
        if(Col.gameObject.tag == "Ground")
        {
            Grounded = true;
        }

        if(Col.gameObject.tag == "RainDrop")
        {
            transform.Find("Oof").GetComponent<AudioSource>().Play();
            lives--;

            if(lives==0)
            {
            Invoke("RestartGame", 0.1f);
            }
        }

    }

    void OnTriggerEnter2D(Collider2D Col)
    {

        if (Col.gameObject.tag == "Lives")
        {
            transform.Find("LifeCollect").GetComponent<AudioSource>().Play();
            Destroy(Col.gameObject);
            lives++;
        }

    }
    
    private void RestartGame()
     {
        GetComponent<AudioSource>().Pause();
        transform.Find("Fail").GetComponent<AudioSource>().Play();
        restart.gameObject.SetActive(true);
        Time.timeScale = 0;
     }

}
