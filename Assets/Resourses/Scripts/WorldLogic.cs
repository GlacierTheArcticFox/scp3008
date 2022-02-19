using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.AI;
using UnityEngine;
using Unity.Netcode;

public class WorldLogic : NetworkBehaviour {

    public GameObject[] plots;
    public GameObject[] signs;
    public GameObject[] walls;
    public GameObject pillar;

    public int worldSize = 10;
    public float dayTime = 120f;

    public GameObject player;
    public GameObject camera;
    public int maxDistance = 10;

    public GameObject audio;
    public AudioClip[] songs;
    int songIndex = 0;

    public GameObject musicPlayer;
    public GameObject lightsPlayer;

    public GameObject worldLight;
    public GameObject worldFloor;

    private List<GameObject> groups = new List<GameObject>();
    public List<GameObject> objects = new List<GameObject>();
    public List<GameObject> movedObjects = new List<GameObject>();

    public List<GameObject> prefabs;

    public VolumeProfile profile;

    private float timer;
    private bool lightsState = true;

    private bool loadedWorld = false;

    public GameObject networkLogic;

    private int objctCounter = 0;


    void Start() {
        //timer = dayTime;
        //LightsOn();
        //worldLight.SetActive(true);
        //profile.TryGet<HDRISky>(out var sky);
        //sky.exposure.value = 15;
    }

    void Update() {
        if (songIndex >= songs.Length) {
            songIndex = 0;
        }

        if(!audio.GetComponent<AudioSource>().isPlaying && audio.GetComponent<AudioSource>().enabled){
            audio.GetComponent<AudioSource>().clip = songs[songIndex];
            audio.GetComponent<AudioSource>().Play();
            songIndex++;
        }
        /*
        timer -= Time.deltaTime;
        if(timer < 0) {
            timer = dayTime;
            if (lightsState) {
                LightsOut();
            } else {
                LightsOn();
            }

            lightsState = !lightsState;
        }
        */
        
        if (groups != null) {
            player = GameObject.Find("LocalPlayer");
            foreach (GameObject group in groups) {
                if (Vector3.Distance(player.transform.position, group.transform.position) > maxDistance) {
                    group.gameObject.SetActive(false);
                } else {
                    group.gameObject.SetActive(true);
                }
            }
        }
    }

    public void GenWorld() {
        for (int x = -(worldSize/2); x < worldSize/2; x++) {
            for (int y = -(worldSize/2); y < worldSize/2; y++) {
                GenGroup(x, y);
            }
        }

        walls[0].SetActive(true);
        walls[1].SetActive(true);
        walls[2].SetActive(true);
        walls[3].SetActive(true);

        walls[0].transform.position = new Vector3(((((worldSize/2)*10)*8)), 500, 0);
        walls[1].transform.position = new Vector3(-((((worldSize/2)*10)*8)+8), 500, 0);
        walls[2].transform.position = new Vector3(0, 500, ((((worldSize/2)*10)*8)));
        walls[3].transform.position = new Vector3(0, 500, -((((worldSize/2)*10)*8)+8));
    }

    void GenGroup(int xpos, int ypos) {
        GameObject newGroup = new GameObject();
        newGroup.name = "Group ("+xpos+", "+ypos+")";
        newGroup.transform.parent = transform;
        newGroup.transform.position = new Vector3(((xpos*10)*8)+36, 0, ((ypos*10)*8)+36);

        for (int x = 0; x < 10; x++) {
            for (int y = 0; y < 10; y++) {
                if (x % 10 == 0 && y % 10 == 0 && -ypos != worldSize/2 && -xpos != worldSize/2) {
                    GameObject newPillar = Instantiate(pillar, new Vector3(((xpos*10)+x)*8, 0, ((ypos*10)+y)*8), Quaternion.identity);
                    newPillar.transform.parent = newGroup.transform;
                } else {
                    if (x % 3 == 1 && y % 3 == 1) {
                        GameObject newSign = Instantiate(signs[Random.Range(0, signs.Length)], new Vector3(((xpos*10)+x)*8, 10, ((ypos*10)+y)*8), Quaternion.Euler(0, Random.Range(0, 2)*90, 0));
                        newSign.transform.parent = newGroup.transform;
                    }

                    GameObject newPlot = Instantiate(plots[Random.Range(0, plots.Length)], new Vector3(((xpos*10)+x)*8, 0, ((ypos*10)+y)*8), Quaternion.Euler(0, Random.Range(0, 3)*90, 0));
                    foreach (Transform childObject in newPlot.transform){
                        if (childObject.tag == "Object") {
                            childObject.name = objctCounter.ToString();
                            objects.Add(childObject.gameObject);
                            objctCounter++;
                        }
                    }
                    
                    newPlot.transform.parent = newGroup.transform;
                }
            }
        }

        groups.Add(newGroup);
    }

    public void LightsOut() {
        //lightsPlayer.GetComponent<AudioSource>().Play();
        //musicPlayer.SetActive(false);
        //worldLight.SetActive(false);
        /*profile.TryGet<HDRISky>(out var sky);
        profile.TryGet<Fog>(out var fog);
        sky.exposure.value = -5;
        camera.GetComponent<HDAdditionalCameraData>().backgroundColorHDR = Color.black;*/
    }

    public void LightsOn() {
        //lightsPlayer.GetComponent<AudioSource>().Play();
        //musicPlayer.SetActive(true);
        //worldLight.SetActive(true);
        /*profile.TryGet<HDRISky>(out var sky);
        profile.TryGet<Fog>(out var fog);
        sky.exposure.value = 15;
        camera.GetComponent<HDAdditionalCameraData>().backgroundColorHDR = new Color(0.871f, 0.871f, 0.871f, 0.0f);*/
    }
}
