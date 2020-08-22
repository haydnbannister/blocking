﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeSpawner : MonoBehaviour
{
    public List<GameObject> normalShapeOptions;
    public List<GameObject> powerupShapeOptions;
    public GameObject blockingLogoPrefab;
    public GameObject gameOverEffect;
    public Toggle powerupsEnabled;
    public GameGrid gameGrid;

    public bool gameOver = false;

    void Start()
    {
        SpawnNormal();
    }
    
    public void SpawnShape()
    {
        if (!gameOver)
        {
            var randomNumber = Random.Range(1, 101);

            
            // max amount of blocks on board is 7x7x12=588, so if you somehow fill up board then +~75% chance of powerup each spawn
            if (randomNumber > 5 + gameGrid.numBlocksOnGrid / 8/* || !powerupsEnabled.isOn*/) 
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

        Transform cameraRig = GameObject.Find("CameraRig").transform;
        GameObject logo = Instantiate(blockingLogoPrefab, new Vector3(3, -2, 3), cameraRig.rotation);
        logo.transform.parent = cameraRig;
    } 
}