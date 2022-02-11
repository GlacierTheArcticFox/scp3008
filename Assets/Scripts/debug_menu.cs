using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class debug_menu : MonoBehaviour {

    public GameObject world;
    public GameObject musicPlayer;

    public AudioClip fortynightClip;
    public AudioClip backOnTrackClip;

    public GameObject menu;

    public Button close;
    public Button day;
    public Button night;
    public Button fortynight;
    public Button backOnTrack;

    public GameObject time;

    private bool toggled = false;

    void Start() {
        Button close_btn = close.GetComponent<Button>();
        Button day_btn = day.GetComponent<Button>();
        Button night_btn = night.GetComponent<Button>();
        Button fortynight_btn = fortynight.GetComponent<Button>();
        Button backOnTrack_btn = backOnTrack.GetComponent<Button>();
        close_btn.onClick.AddListener(CloseMenu);
        day_btn.onClick.AddListener(SetTimeDay);
        night_btn.onClick.AddListener(SetTimeNight);
        fortynight_btn.onClick.AddListener(Fortynight);
        backOnTrack_btn.onClick.AddListener(BackOnTrack);
        //Cursor.lockState = CursorLockMode.None;
    }

    void Update() {
        if (Input.GetButtonDown("DebugMenu")){
            toggled = !toggled;
        }

        if (toggled){
            menu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        } else {
            Cursor.lockState = CursorLockMode.Locked;
            menu.SetActive(false);
        }
    }

    void SetTimeDay() {
        world.GetComponent<wold_logic>().LightsOn();
    }

    void SetTimeNight() {
        world.GetComponent<wold_logic>().LightsOut();
    }

    void CloseMenu() {
        toggled = !toggled;
    }

    void Fortynight() {
        musicPlayer.GetComponent<AudioSource>().clip = fortynightClip;
        musicPlayer.GetComponent<AudioSource>().Play();
    }

    void BackOnTrack() {
        musicPlayer.GetComponent<AudioSource>().clip = backOnTrackClip;
        musicPlayer.GetComponent<AudioSource>().Play();
    }
}
