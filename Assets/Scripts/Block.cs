﻿using System;
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
        print("shape " + gameObject.name + " has collided with " + other.gameObject.name);
        GetComponentInParent<Shape>().inPlay = false;
    }
}
