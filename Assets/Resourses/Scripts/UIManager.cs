using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class UIManager : MonoBehaviour {

    public VolumeProfile volume;
    public GameObject motionblurlable;
    public GameObject mousebensitivitylable;
    public GameObject inGameMenu;
    public GameObject settingsMenu;

    public float mouseSensitivity = 5;

    public bool inGame = false;

    public void SetQuality(UnityEngine.UI.Dropdown dropdown) {
        QualitySettings.SetQualityLevel(dropdown.value, true);
    }

    public void SetMotionBlur(UnityEngine.UI.Slider slider) {
        volume.TryGet<MotionBlur>(out var motionblur);
        motionblur.intensity.value = slider.value/100;
        motionblurlable.GetComponent<Text>().text = $"Motion Blur - {slider.value.ToString()}";
    }

    public void SetMouseSensitivity(UnityEngine.UI.Slider slider) {
        mouseSensitivity = slider.value/10;
        mousebensitivitylable.GetComponent<Text>().text = $"Mouse sensitivity - {slider.value.ToString()}";
    }

    public void BackToGame(){
        Cursor.lockState = CursorLockMode.Locked;
        inGameMenu.SetActive(false);
    }

    void Update() {
        if (Input.GetButtonDown("Cancel") && inGame) {
            settingsMenu.SetActive(false);
            if (inGameMenu.active){
                BackToGame();
            } else {
                Cursor.lockState = CursorLockMode.None;
                inGameMenu.SetActive(true);
            }
        }
    }
}
