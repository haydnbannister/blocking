using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    public List<GameObject> shapeOptions;
    public GameObject gameOverEffect;

    public bool gameOver = false;

    void Start()
    {
        var randomNumber = Random.Range(0, shapeOptions.Count);
        Instantiate(shapeOptions[randomNumber], transform.position, Quaternion.identity);
    }
    
    public void SpawnShape()
    {
        if (!gameOver)
        {
            var randomNumber = Random.Range(0, shapeOptions.Count);
            Instantiate(shapeOptions[randomNumber], transform.position, Quaternion.identity);
        } else {
            Instantiate(gameOverEffect, new Vector3(3, 1, 3), Quaternion.identity);
        }
    }
}