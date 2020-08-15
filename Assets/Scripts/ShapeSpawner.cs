using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    public List<GameObject> shapeOptions;
    public GameObject gameOverEffect;

    public bool gameOver = false;

    void Start()
    {
        SpawnRandom();
    }
    
    public void SpawnShape()
    {
        if (!gameOver)
        {
            SpawnRandom();
        }
    }

    private void SpawnRandom() 
    {
        var randomNumber = Random.Range(0, shapeOptions.Count);
        Instantiate(shapeOptions[randomNumber], transform.position, Quaternion.identity);
    }

    public void EndGame() {
            gameOver = true;
            EndGameEffect();
    }

    private void EndGameEffect()
    { 
            Instantiate(gameOverEffect, new Vector3(3, 1, 3), Quaternion.identity);
    }
}