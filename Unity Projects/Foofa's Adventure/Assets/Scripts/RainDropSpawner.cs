using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RainDropSpawner : MonoBehaviour
{
    public GameObject raindrop;
    public float spawnRate;
    private Camera mainCamera;

    void Start () {
        mainCamera = Camera.main;
        InvokeRepeating("SpawnRaindrop", 0, spawnRate);
        //StartCoroutine(disappear());
    }

    void SpawnRaindrop () {
        float randomX = Random.Range(mainCamera.ViewportToWorldPoint(new Vector3(0, 0, 0)).x, mainCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x);
        float randomY = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, 0)).y;
        GameObject newRaindrop = Instantiate(raindrop, new Vector2(randomX, randomY), Quaternion.identity);
        // Set the raindrop game object to destroy itself when it falls below the bottom edge of the screen
        Destroy(newRaindrop, 3f);
    }

    IEnumerator disappear(){
        yield return new WaitForSeconds(3f);
        raindrop.SetActive(false);
    }


}


