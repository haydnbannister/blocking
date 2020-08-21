using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCameraSpinner : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(new Vector3(0, 1, 0), -0.05f);
    }
}
