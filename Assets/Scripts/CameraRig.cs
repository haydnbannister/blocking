using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour
{
    public float[,] angs = new float[,] {
        {30,45,60},
        {120,135,150},
        {210,225,240},
        {300,315,330}
        };

    public int i = 0;
    public int j = 1;

    public float speed = 150f;
    private float to = -1f;

    public Vector3[,] controlTransforms = new Vector3[,] {
        {new Vector3(0f, 0f, 1f), new Vector3(0f, 0f, -1f), new Vector3(-1f, 0f, 0f), new Vector3(1f, 0f, 0f)},
        {new Vector3(1f, 0f, 0f), new Vector3(-1f, 0f, 0f), new Vector3(0f, 0f, 1f), new Vector3(0f, 0f, -1f)},
        {new Vector3(0f, 0f, -1f), new Vector3(0f, 0f, 1f), new Vector3(1f, 0f, 0f), new Vector3(-1f, 0f, 0f)},
        {new Vector3(-1f, 0f, 0f), new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, -1f), new Vector3(0f, 0f, 1f)}
    };

    public Vector3[,] rotationTransforms = new Vector3[,] {
        {new Vector3(1f, 0f, 0f), new Vector3(-1f, 0f, 0f), new Vector3(0, -1, 0), new Vector3(0, 1, 0)},
        {new Vector3(0, 0, -1), new Vector3(0, 0, 1), new Vector3(0f, 1f, 0f), new Vector3(0f, -1f, 0f)},
        {new Vector3(-1f, 0f, 0f), new Vector3(1f, 0f, 0f), new Vector3(0, 1, 0), new Vector3(0, -1, 0)},
        {new Vector3(0, 0, 1), new Vector3(0, 0, -1), new Vector3(0f, -1f, 0f), new Vector3(0f, 1f, 0f)}
    };

    public Vector3 GetDirection(string direction)
    {
        switch (direction)
        {
            case "up":
                return controlTransforms[i, 0];
            case "down":
                return controlTransforms[i, 1];
            case "left":
                return controlTransforms[i, 2];
            default: //right
                return controlTransforms[i, 3];
        }
    }

    public Vector3 GetRotation(string direction)
    {
        switch (direction)
        {
            case "w":
                return rotationTransforms[i, 0];
            case "s":
                return rotationTransforms[i, 1];
            case "a":
                return rotationTransforms[i, 2];
            default: //d
                return rotationTransforms[i, 3];
        }
    }

    void Update()
    {
        HandleRotation("");
    }
    public void HandleRotation(string direction) {
        if (to != -1)
        {
            Vector3 targetDirection = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, to, 0), speed * Time.deltaTime);
        }

        if (Input.GetKeyDown("q") || direction == "q")
        {
            if (j == 2)
            {
                j = 0;
                if (i == 3)
                {
                    i = 0;
                }
                else
                {
                    i++;
                }
            }
            else
            {
                j++;
            }
            Debug.Log(angs[i, j]);
            to = angs[i, j];
        }
        if (Input.GetKeyDown("e") || direction == "e")
        {
            Debug.Log(transform.rotation.eulerAngles);
            if (j == 0)
            {
                j = 2;
                if (i == 0)
                {
                    i = 3;
                }
                else
                {
                    i--;
                }
            }
            else
            {
                j--;
            }
            Debug.Log(angs[i, j]);
            to = angs[i, j];
        }
    }
}