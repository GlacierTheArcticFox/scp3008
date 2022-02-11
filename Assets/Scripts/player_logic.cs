using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine;
using UnityEngine.AI;

public class player_logic : MonoBehaviour {

    public GameObject camera;
    public GameObject point;
    public GameObject worldFloor;
    public GameObject world;

    public VolumeProfile profile;

    RaycastHit hit;
    bool isHit;

    Vector3 rot;
    byte rotMode;

    public float stamina = 100;
    public float energy = 100;
    public float health = 100;

    private bool staminaRefilling = false;
    private bool iFrame = false;

    void Start() {
        
    }

    void Update() {
        energy -= 0.1f*Time.deltaTime;

        profile.TryGet<Bloom>(out var bloom);
        profile.TryGet<MotionBlur>(out var motionBlur); 

        if (health <= 0){
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }

        if (energy <= 0) {
            energy = 0;
            Damage(5, 1f);
            bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, 0.9f, 0.1f);
            motionBlur.intensity.value = Mathf.Lerp(bloom.intensity.value, 50f, 0.1f);
        } else {
            bloom.intensity.value = Mathf.Lerp(bloom.intensity.value, 0.2f, 0.1f);
            motionBlur.intensity.value = Mathf.Lerp(bloom.intensity.value, 0.5f, 0.1f);
        }

        if (stamina != 100f && !staminaRefilling) {
            staminaRefilling = true;
            StartCoroutine(RefillStamina());
        }

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

    public void Damage(int damage, float iframe) {
        if (!iFrame){
            iFrame = true;
            StartCoroutine(TakeDamage(damage, iframe));
        }
    }

    IEnumerator TakeDamage(int damage, float iframe) {
        health -= damage;
        yield return new WaitForSeconds(iframe);
        iFrame = false;
    }

    IEnumerator RefillStamina() {
        yield return new WaitForSeconds(10);
        for (int i = (int)stamina; i != 101; i++){
            yield return new WaitForSeconds(0.03f);
            stamina = i;
            if (Input.GetButton("Sprint")){
                break;
            }
        }
        //stamina = 100;
        staminaRefilling = false;
    }
}
