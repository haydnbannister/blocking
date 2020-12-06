using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupToggle : MonoBehaviour
{
    public bool UiControls = false;
    public bool spacePressed = false;
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }


}
