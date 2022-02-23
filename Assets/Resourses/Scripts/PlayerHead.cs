using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stolen from https://www.youtube.com/watch?v=_QajrabyTJc

public class PlayerHead : MonoBehaviour {
    
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public GameObject UIManager;

    float xRotation = 0f;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        UIManager = GameObject.Find("Canvas");
    }

    void Update() {
        mouseSensitivity = UIManager.GetComponent<UIManager>().mouseSensitivity;
        if (Cursor.lockState != CursorLockMode.None) {
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }
    
}
