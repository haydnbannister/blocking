using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    public List<GameObject> normalShapeOptions;
    public List<GameObject> powerupShapeOptions;
    public GameObject gameOverEffect;

    public bool gameOver = false;

    void Start()
    {
        SpawnShape();
    }
    
    public void SpawnShape()
    {
        if (!gameOver)
        {
            var randomNumber = Random.Range(1, 101);
            if (randomNumber > 10) 
            {
                SpawnNormal();
            } else 
            {
                SpawnPowerup();
            }
        }
    }

    private void SpawnNormal() {
        var randomNumber = Random.Range(0, normalShapeOptions.Count);
        Instantiate(normalShapeOptions[randomNumber], transform.position, Quaternion.identity);
    }

    private void SpawnPowerup() {
        var randomNumber = Random.Range(0, powerupShapeOptions.Count);
        Instantiate(powerupShapeOptions[randomNumber], transform.position, Quaternion.identity);
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