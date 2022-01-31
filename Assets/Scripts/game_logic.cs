using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_logic : MonoBehaviour {

    public GameObject[] plot;
    public GameObject[] advert;
    public GameObject pillar;

    void Start() {
        for (int x = 0; x < 20; x++) {
            for (int y = 0; y < 20; y++) {
                GenerateWorld(x, y);
            }
        }
    }

    void Update() {
        
    }

    void GenerateWorld(int posx, int posy) {
        GameObject group = Instantiate(new GameObject(), new Vector3((posx*10)*8, 0, (posy*10)*8), Quaternion.identity);
        group.name = "Group-"+posx.ToString()+"-"+posy.ToString();
        group.tag = "Group";

        for (int x = posx*10; x < (posx*10)+10; x++) {
            for (int y = posy*10; y < (posy*10)+10; y++) {

                if (x % 9 == 0 && y % 9 == 0){
                    GameObject newObj = Instantiate(pillar, new Vector3(x * 8.0f, 0, y * 8.0f), Quaternion.identity);
                    newObj.transform.parent = group.transform; 
                } else {
                    GameObject newObj = Instantiate(plot[Random.Range(0, plot.Length)], new Vector3(x * 8.0f, 0, y * 8.0f), Quaternion.Euler(0, 90*Random.Range(0, 3), 0));
                    newObj.transform.parent = group.transform; 
                }

                if (x % 3 == 0 && y % 3 == 0) {
                    GameObject newObj = Instantiate(advert[Random.Range(0, advert.Length)], new Vector3(x * 8.0f, 10, y * 8.0f), Quaternion.Euler(0, 90*Random.Range(0, 2), 0));
                    newObj.transform.parent = group.transform; 
                }
            }
        }
    }
}
