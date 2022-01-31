using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class object_hider : MonoBehaviour {

    public GameObject player;

    public int maxDistance;

    public GameObject[] plots;


    void Start() {
        StartCoroutine(scan());
        
    }

    void Update() {
        if (plots != null){
            foreach (GameObject plot in plots) {
                if (Vector3.Distance(player.transform.position, plot.transform.position+new Vector3(40, 0, 40)) > maxDistance) {
                    plot.SetActive(false);
                } else {
                    plot.SetActive(true);
                }
            }
        }
    }

    IEnumerator scan() {
        yield return new WaitForSeconds(0);
        plots = GameObject.FindGameObjectsWithTag("Group");
    }
}
