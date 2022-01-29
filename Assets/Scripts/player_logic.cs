using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_logic : MonoBehaviour {

    public GameObject camera;
    public GameObject point;

    RaycastHit hit;
    bool isHit;

    void Start() {
        
    }

    void Update() {

        if ( Input.GetMouseButton(0) ) {
            if (!isHit) {
                Ray ray = camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 2)) {
                    if (hit.collider.tag != "Static") {
                        isHit = true;
                        hit.collider.gameObject.GetComponent<MeshCollider>().enabled = false;
                    }
                }
            } else {
                hit.collider.gameObject.transform.parent.position = point.transform.position;
            }
            
        } else {
            if (isHit){
                hit.collider.gameObject.GetComponent<MeshCollider>().enabled = true;
                point.transform.localPosition = new Vector3(0, 0, 1.5f);
                isHit = false;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") != 0f) {
            point.transform.localPosition = new Vector3(0, 0, Mathf.Clamp(point.transform.localPosition.z + Input.GetAxis("Mouse ScrollWheel")/2, 1, 3));
        }

    }
}
