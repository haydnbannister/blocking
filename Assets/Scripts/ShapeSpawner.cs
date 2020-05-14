using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{
    public List<GameObject> shapeOptions;

    public bool gameOver = false;

    void Start()
    {
        Instantiate(shapeOptions[0], transform.position, Quaternion.identity);
    }
    
    public void SpawnShape()
    {
        if (!gameOver)
        {
            Instantiate(shapeOptions[0], transform.position, Quaternion.identity);
        }
    }
}