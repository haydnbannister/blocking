using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MobileController : MonoBehaviour
{
    public GameObject desktopControlInstructions;
    public GameObject mobileControls;
    public PowerupToggle powerupToggle;
    public GameObject mobileSpaceText;

    // Start is called before the first frame update
    void Start()
    {
        powerupToggle = GameObject.Find("Canvas Powerups Toggle").GetComponent<PowerupToggle>();
        
        if (!powerupToggle.UiControls) {
            var offButton = GameObject.Find("UiControlsOff");
            if(offButton != null) offButton.SetActive(false);
        }

        if (isMobile() || powerupToggle.UiControls)
        {
            desktopControlInstructions.SetActive(false);
            if (mobileControls != null) mobileControls.SetActive(true);

            if(mobileSpaceText != null) mobileSpaceText.SetActive(!powerupToggle.spacePressed);
        }
    }

    public void ToggleUiControls(bool mode) {
        powerupToggle.UiControls = mode;
    }
    public void ToggleSpaceUi(bool mode) {
        powerupToggle.spacePressed = mode;
    }

    public bool isMobile()
    {
#if UNITY_ANDROID || UNITY_IOS
        return true;
#endif

#if !UNITY_EDITOR && UNITY_WEBGL
        return IsMobileCheck.IsMobile();
#endif
        return false;
    }
}
