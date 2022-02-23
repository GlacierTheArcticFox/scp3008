using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour {
    public AudioSource walkingSound;
    public GameObject playerScript;

    public void PlayFootStep() {
        if(playerScript.GetComponent<Player>().isGrounded){
            walkingSound.pitch = Random.Range(0.9f, 1.1f);
            walkingSound.Play();
        }
    }
}
