using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class Shape : MonoBehaviour
{
    
    public Block[] blocks;

    public bool inPlay = true;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!inPlay) return;
        
        if (Input.GetKeyDown("up") && IsMovementAllowed(new Vector3(0f, 0f, 1f)))
        {
            transform.Translate(Vector3.forward * 1);
        }
        if (Input.GetKeyDown("down") && IsMovementAllowed(new Vector3(0f, 0f, -1f)))
        {
            transform.Translate(Vector3.back * 1);
        }
        if (Input.GetKeyDown("left") && IsMovementAllowed(new Vector3(-1f, 0f, 0f)))
        {
            transform.Translate(Vector3.left * 1);
        }
        if (Input.GetKeyDown("right") && IsMovementAllowed(new Vector3(1f, 0f, 0f)))
        {
            transform.Translate(Vector3.right * 1);
        }
            
        // slowly move down over time
        transform.Translate(Vector3.down * Time.deltaTime);
    }
    
    private bool IsMovementAllowed(Vector3 movement)
    {
        return blocks.Select(block => 
            transform.localPosition + block.transform.localPosition + movement)
            .All(newPosition => 
                !(newPosition.z >= 4) 
                && !(newPosition.z <= -4) 
                && !(newPosition.x >= 4) 
                && !(newPosition.x <= -4));
    }
}
