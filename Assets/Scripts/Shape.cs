using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("up"))
        {
            transform.Translate(Vector3.forward * 1);
        }
        if (Input.GetKeyDown("down"))
        {
            transform.Translate(Vector3.back * 1);
        }
        if (Input.GetKeyDown("left"))
        {
            transform.Translate(Vector3.left * 1);
        }
        if (Input.GetKeyDown("right"))
        {
            transform.Translate(Vector3.right * 1);
        }
    }
}
