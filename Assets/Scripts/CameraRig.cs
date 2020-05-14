using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("q"))
        {
            transform.Rotate(new Vector3(0, 1, 0), 2f);
        }
        if (Input.GetKey("e"))
        {
            transform.Rotate(new Vector3(0, 1, 0), -2f);
        }
    }
}
