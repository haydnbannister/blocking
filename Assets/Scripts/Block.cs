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

    private void OnTriggerEnter(Collider other)
    {
        if (transform.parent == other.transform.parent)
            return;
        
        if (other.transform.parent.name == "GameGrid")
        {
            //print("shape " + gameObject.name + " has collided with " + other.gameObject.name);
            GetComponentInParent<Shape>().Land();
            return;
        }

        // only hit blocks directly below, otherwise corners and edges count
        var thisPosition = transform.position;
        var otherPosition = other.transform.position;
        if ((int) Math.Round(thisPosition.x, 0) != (int) Math.Round(otherPosition.x, 0) 
            || (int) Math.Round(thisPosition.z, 0) != (int) Math.Round(otherPosition.z, 0) )
        {
            return;
        }

        //print("shape " + gameObject.name + " has collided with " + other.gameObject.name);
        GetComponentInParent<Shape>().Land();
    }
}