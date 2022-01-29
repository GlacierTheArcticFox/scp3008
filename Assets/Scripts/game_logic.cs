using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class game_logic : MonoBehaviour {

    public GameObject[] plot;
    public GameObject pillar;

    void Start() {
        GenerateWorld();
    }

    void Update() {
        
    }

    void GenerateWorld() {
        for (int x = 0; x < 50; x++) {
            for (int y = 0; y < 50; y++) {
                if (x % 9 == 0 && y % 9 == 0){
                    Instantiate(pillar, new Vector3((x - 25f) * 8.0f, 0, (y - 25f) * 8.0f), Quaternion.identity);
                } else {
                    Instantiate(plot[Random.Range(0, plot.Length)], new Vector3((x - 25f) * 8.0f, 0, (y - 25f) * 8.0f), Quaternion.identity);
                }
            }
        }
    }
}
