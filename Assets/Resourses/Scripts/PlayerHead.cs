using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stolen from https://www.youtube.com/watch?v=_QajrabyTJc

public class PlayerHead : MonoBehaviour {
    
    public float mouseSensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        if (Cursor.lockState != CursorLockMode.None) {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }

        if (Input.GetButton("Cancel")){
            if (Cursor.lockState != CursorLockMode.None){
                Cursor.lockState = CursorLockMode.None;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
    
}
