using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    public float spinSpeed = 75f;

    void Update()
    {
        if (Input.GetKey("q"))
        {
            transform.Rotate(new Vector3(0, 1, 0), spinSpeed * Time.deltaTime);
        }
        if (Input.GetKey("e"))
        {
            transform.Rotate(new Vector3(0, 1, 0), - spinSpeed * Time.deltaTime);
        }
    }
}
