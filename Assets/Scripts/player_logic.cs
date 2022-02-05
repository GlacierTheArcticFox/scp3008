using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_logic : MonoBehaviour {

    public GameObject camera;
    public GameObject point;
    public GameObject worldFloor;

    RaycastHit hit;
    bool isHit;

    Vector3 rot;
    byte rotMode;

    void Start() {
        
    }

    void Update() {

        worldFloor.transform.position = new Vector3(transform.position.x, worldFloor.transform.position.y, transform.position.z);

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

                if (Input.GetButtonDown("ResetAxis")){
                    rotMode = 0;
                    rot = Vector3.zero;
                    if(hit.collider.tag == "Parent"){
                        hit.collider.gameObject.transform.parent.rotation = Quaternion.identity;
                    } else {
                        hit.collider.gameObject.transform.rotation = Quaternion.identity;
                    }
                }

                if (Input.GetButtonDown("RotateAxis")){
                    rotMode += 1;
                    if (rotMode > 2){
                        rotMode = 0;
                    }
                }

                if (Input.GetButtonDown("Rotate")){
                    if (rotMode == 0){
                        rot.x = 45;
                    } else if (rotMode == 1){
                        rot.y = 45;
                    } else {
                        rot.z = 45;
                    }
                }

                if(hit.collider.tag == "Parent"){
                    hit.collider.gameObject.transform.parent.position = point.transform.position;
                    hit.collider.gameObject.transform.parent.Rotate(rot);
                    hit.collider.gameObject.transform.parent.parent = GameObject.Find("Moved Objects").transform;
                } else {
                    hit.collider.gameObject.transform.position = point.transform.position;
                    hit.collider.gameObject.transform.Rotate(rot);
                    hit.collider.gameObject.transform.parent = GameObject.Find("Moved Objects").transform;
                }

                rot = Vector3.zero;
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
