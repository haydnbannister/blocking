﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using OSSC;

public class SoundControlsUI : MonoBehaviour
{
    public SoundController soundController;
    public Toggle effectsToggle;
    public Toggle musicToggle;

    // Start is called before the first frame update
    void Start()
    {

        if (SceneManager.GetActiveScene().name == "MenuScene") 
        {
            soundController.SetMute("Effects", false);
            soundController.SetMute("Music", false);
        }

        effectsToggle.SetIsOnWithoutNotify(soundController.IsMute("Effects"));
        musicToggle.SetIsOnWithoutNotify(soundController.IsMute("Music"));
        
        PlaySoundSettings settings = new PlaySoundSettings();
        settings.Init();
        settings.name = "Music";
        settings.isLooped = true;
        //settings.fadeInTime = 3f;
        //settings.fadeOutTime = 3f;
        soundController.Play(settings);
    }


}
