using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class ClientLogic : NetworkBehaviour {

    public GameObject world;
    public GameObject playerModel;
    public GameObject camera;
    public GameObject point;

    RaycastHit hit;
    bool isHit;

    int rotMode = 2;
    Vector3 rot;

    //public NetworkVariable<int> worldSeed = new NetworkVariable<int>();

    void Update() {
        if (!IsOwner) {return;}

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
                    GameObject.Find("NetworkLogic").GetComponent<NetworkLogic>().MoveObjectServerRpc(int.Parse(hit.collider.gameObject.transform.parent.gameObject.name), hit.collider.gameObject.transform.parent.position, hit.collider.gameObject.transform.parent.rotation);
                    //hit.collider.gameObject.transform.parent.Rotate(rot);
                    //hit.collider.gameObject.transform.parent.parent = GameObject.Find("MovedObjects").transform;
                } else {
                    hit.collider.gameObject.transform.position = point.transform.position;
                    hit.collider.gameObject.transform.Rotate(rot);
                    GameObject.Find("NetworkLogic").GetComponent<NetworkLogic>().MoveObjectServerRpc(int.Parse(hit.collider.gameObject.name), hit.collider.gameObject.transform.position, hit.collider.gameObject.transform.rotation);
                    //MoveObjectServerRpc(hit.collider.gameObject.GetInstanceID() ,point.transform.position);
                    //hit.collider.gameObject.transform.Rotate(rot);
                    //hit.collider.gameObject.transform.parent = GameObject.Find("MovedObjects").transform;
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
