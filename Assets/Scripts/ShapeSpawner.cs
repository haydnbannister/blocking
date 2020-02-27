using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeSpawner : MonoBehaviour
{

    public List<GameObject> shapeOptions;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(shapeOptions[0], transform.position, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SpawnShape()
    {
        Instantiate(shapeOptions[0], transform.position, Quaternion.identity);
    }
}
