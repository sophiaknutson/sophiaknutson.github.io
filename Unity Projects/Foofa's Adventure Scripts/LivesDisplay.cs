using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LivesDisplay : MonoBehaviour
{
    // Start is called before the first frame update
    public PlayerMovement playerMovement;
    public TextMeshProUGUI livesText;

    void Start()
    {
        livesText.text = ":" + playerMovement.lives.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        livesText.text = ":" + playerMovement.lives.ToString();
    }
}
