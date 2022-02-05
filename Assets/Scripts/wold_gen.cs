using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this is a alot better implementation of world generation ngl

public class wold_gen : MonoBehaviour {

    public GameObject[] plots;
    public GameObject[] signs;
    public GameObject[] walls;
    public GameObject pillar;

    public int worldSize = 10;
    public float dayTime = 120f;

    public GameObject player;
    public GameObject camera;
    public int maxDistance = 10;

    public GameObject musicPlayer;
    public GameObject worldLight;

    private List<GameObject> groups = new List<GameObject>();

    

    

    void Start() {
        //GenGroup(0, 0);
        GenWorld();
    }

    void Update() {

        dayTime -= Time.deltaTime;
        if(dayTime < 0) {
            LightsOut();
        }

        if (groups != null) {
            foreach (GameObject group in groups) {
                if (Vector3.Distance(player.transform.position, group.transform.position) > maxDistance) {
                    group.gameObject.SetActive(false);
                } else {
                    group.gameObject.SetActive(true);
                }
            }
        }
    }

    void GenWorld() {
        for (int x = -(worldSize/2); x < worldSize/2; x++) {
            for (int y = -(worldSize/2); y < worldSize/2; y++) {
                GenGroup(x, y);
            }
        }

        walls[0].transform.position = new Vector3(((((worldSize/2)*10)*8)), 50, 0);
        walls[1].transform.position = new Vector3(-((((worldSize/2)*10)*8)), 50, 0);
        walls[2].transform.position = new Vector3(0, 50, ((((worldSize/2)*10)*8)));
        walls[3].transform.position = new Vector3(0, 50, -((((worldSize/2)*10)*8)));
    }

    void GenGroup(int xpos, int ypos) {
        GameObject newGroup = new GameObject();
        newGroup.name = "Group ("+xpos+", "+ypos+")";
        newGroup.transform.parent = transform;
        newGroup.transform.position = new Vector3(((xpos*10)*8)+36, 0, ((ypos*10)*8)+36);

        //GameObject newWorldFloor = Instantiate(worldFloor, new Vector3(0, -0.001f, 0), Quaternion.identity);
        //newWorldFloor.transform.parent = newGroup.transform;

        groups.Add(newGroup);

        for (int x = 0; x < 10; x++) {
            for (int y = 0; y < 10; y++) {
                if (x % 10 == 0 && y % 10 == 0) {
                    GameObject newPillar = Instantiate(pillar, new Vector3(((xpos*10)+x)*8, 0, ((ypos*10)+y)*8), Quaternion.identity);
                    newPillar.transform.parent = newGroup.transform;
                } else {
                    if (x % 3 == 1 && y % 3 == 1) {
                        GameObject newSign = Instantiate(signs[Random.Range(0, signs.Length)], new Vector3(((xpos*10)+x)*8, 10, ((ypos*10)+y)*8), Quaternion.Euler(0, Random.Range(0, 2)*90, 0));
                        newSign.transform.parent = newGroup.transform;
                    }
                    GameObject newPlot = Instantiate(plots[Random.Range(0, plots.Length)], new Vector3(((xpos*10)+x)*8, 0, ((ypos*10)+y)*8), Quaternion.Euler(0, Random.Range(0, 3)*90, 0));
                    newPlot.transform.parent = newGroup.transform;
                }
            }
        }
    }

    void LightsOut() {
        musicPlayer.SetActive(false);
        worldLight.SetActive(false);
        RenderSettings.ambientIntensity = 0.1f;
        RenderSettings.reflectionIntensity = 0.1f;
        RenderSettings.fogColor = new Color(0.05f, 0.05f, 0.05f, 0.05f);
        camera.GetComponent<Camera>().backgroundColor = new Color(0.05f, 0.05f, 0.05f, 0.05f);
    }
}
