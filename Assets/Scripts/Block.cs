using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetAbsoluteCoordinates()
    {
        return transform.position;
    }
    
    private void OnTriggerEnter(Collider other)
    {

        if (transform.parent == other.transform.parent)
            return;

        print("shape " + gameObject.name + " has collided with " + other.gameObject.name);
        
        if (other.transform.parent.name == "GameGrid")
        {
            GetComponentInParent<Shape>().inPlay = false;
            return;
        }
        
        // only hit blocks directly below, otherwise corners and edges count
        var thisPosition = transform.position;
        var otherPosition = other.transform.position;
        if (thisPosition.x != otherPosition.x || thisPosition.z != otherPosition.z)
        {
            return;
        }
        
        GetComponentInParent<Shape>().inPlay = false;
    }
}
