using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour {
    void Update() {
        transform.LookAt(GameObject.Find("Head").transform.position, Vector3.up);
    }
}