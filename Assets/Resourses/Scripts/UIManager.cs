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

    public void SetQuality(UnityEngine.UI.Dropdown dropdown) {
        QualitySettings.SetQualityLevel(dropdown.value, true);
    }

    public void SetMotionBlur(UnityEngine.UI.Slider slider) {
        volume.TryGet<MotionBlur>(out var motionblur);
        motionblur.intensity.value = slider.value/100;
        motionblurlable.GetComponent<Text>().text = "Motion Blur - "+slider.value.ToString();
    }
}
