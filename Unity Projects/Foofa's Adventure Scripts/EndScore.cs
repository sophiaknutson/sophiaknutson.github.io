using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScore : MonoBehaviour
{
    //Start is called before the first frame update
    public CoinCollector coinCollector;  // reference to the CoinCollector script on the player object
    public TextMeshProUGUI scoreText;  // reference to the Text component on this object

    void Start()
    {
        scoreText.text = "Final Score:" + coinCollector.score.ToString();  // get a reference to the Text component on this object
    }

    void Update()
    {
        scoreText.text = "Final Score:" + coinCollector.score.ToString();  // update the text to display the current score
    }
}
