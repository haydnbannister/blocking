﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShapeSpawner : MonoBehaviour
{
    public List<GameObject> normalShapeOptions;
    public List<GameObject> powerupShapeOptions;
    public GameObject blockingLogoPrefab;
    public GameObject gameOverEffect;
    public GameGrid gameGrid;
    public Toggle powerupToggle;

    public bool gameOver = false;

    void Start()
    {
        SpawnNormal();

        GameObject powerUpToggleCanvas = GameObject.Find("Canvas Powerups Toggle");

        if (powerUpToggleCanvas == null)
        {
            return;
        }

        Transform[] trs = powerUpToggleCanvas.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in trs)
        {
            if (t.name == "TogglePowerups")
            {
                powerupToggle = t.gameObject.GetComponent<Toggle>();
                powerupToggle.gameObject.SetActive(false);
                return;
            }
        }
    }

    public void SpawnShape()
    {
        if (!gameOver)
        {
            var randomNumber = Random.Range(1, 101);

            // max amount of blocks on board is 7x7x12=588, so if you somehow fill up board then +~50% chance of powerup each spawn
            if (powerupToggle == null || !powerupToggle.isOn || randomNumber > 4 + gameGrid.numBlocksOnGrid / 12)
            {
                SpawnNormal();
            }
            else
            {
                SpawnPowerup();
            }
        }
    }

    private void SpawnNormal()
    {
        var randomNumber = Random.Range(0, normalShapeOptions.Count);
        gameGrid.currentShape = Instantiate(normalShapeOptions[randomNumber], transform.position, Quaternion.identity).GetComponent<Shape>();
    }

    private void SpawnPowerup()
    {
        var randomNumber = Random.Range(0, powerupShapeOptions.Count);
        gameGrid.currentShape = Instantiate(powerupShapeOptions[randomNumber], transform.position, Quaternion.identity).GetComponent<Shape>();
    }

    public void EndGame()
    {
        if (!gameOver)
        {
            gameOver = true;
            EndGameEffect();
        }
    }

    private void EndGameEffect()
    {
        Instantiate(gameOverEffect, new Vector3(3, 1, 3), Quaternion.identity);

        Transform cameraRig = GameObject.Find("CameraRig").transform;
        GameObject logo = Instantiate(blockingLogoPrefab, new Vector3(3, -2, 3), cameraRig.rotation);
        logo.transform.parent = cameraRig;
    }
}