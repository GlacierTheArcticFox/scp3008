using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour {
    //public NetworkVariable<Vector3> PlayerPosition = new NetworkVariable<Vector3>();
    public NetworkVariable<Quaternion> PlayerHeadRotation = new NetworkVariable<Quaternion>();

    public CharacterController controller;
    public GameObject camera;
    public GameObject playerModel;
    public GameObject headBone;
    
    public float defaultSpeed = 3f;
    public float sprintSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public AudioSource walkingSound;

    Vector3 velocity;
    Vector2 direction;

    float speed;
    bool isGrounded;

    [ServerRpc]
    void SetHeadRotationServerRpc(Quaternion rotation) {
        PlayerHeadRotation.Value = rotation;
    }


    public override void OnNetworkSpawn() {
        if (IsOwner) {
            gameObject.transform.position = new Vector3(4, 0.75f, 4);
            playerModel.SetActive(false);
            gameObject.name = "LocalPlayer";
        } else {
            /*camera.GetComponent<Camera>().enabled = false;//SetActive(false);
            camera.GetComponent<AudioListener>().enabled = false;
            camera.GetComponent<PlayerHead>().enabled = false;*/
            camera.SetActive(false);
            //headBone.GetComponent<RotationConstraint>().enabled = false;
        }
    }

    void Update() {
            headBone.transform.rotation = Quaternion.Lerp(headBone.transform.rotation, PlayerHeadRotation.Value, 0.1f);
            if (!IsOwner){ return; }

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded && velocity.y < 0) {
                velocity.y = -2f;
            }

            if (Cursor.lockState != CursorLockMode.None){
                direction.x = Input.GetAxis("Horizontal");
                direction.y = Input.GetAxis("Vertical");
            }

            direction = Vector2.ClampMagnitude(direction, 1);

            if ((direction != Vector2.zero) && isGrounded) {
                if (!walkingSound.isPlaying) {
                    walkingSound.pitch = Random.Range(0.9f, 1.1f);
                    if (speed == sprintSpeed) {
                        walkingSound.PlayDelayed(0.1f);
                    } else {
                        walkingSound.PlayDelayed(0.3f);
                    }
                }
            } else {
                //walkingSound.Stop();
            }

            Vector3 move = transform.right * direction.x + transform.forward * direction.y;

            if (Input.GetButton("Sprint") && move != Vector3.zero && Cursor.lockState != CursorLockMode.None/* && GetComponent<player_logic>().stamina > 0f*/) {
                speed = sprintSpeed;
                camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(camera.GetComponent<Camera>().fieldOfView, 70, 0.1f);
                //GetComponent<player_logic>().stamina -= 10f*Time.deltaTime;
                //GetComponent<player_logic>().energy -= 0.5f*Time.deltaTime;
            } else {
                speed = defaultSpeed;
                camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(camera.GetComponent<Camera>().fieldOfView, 60, 0.1f);
            }

            controller.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump") && isGrounded && Cursor.lockState != CursorLockMode.None) {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }

            velocity.y += gravity * Time.deltaTime;
            controller.Move(velocity * Time.deltaTime);

            /*if (NetworkManager.Singleton.IsServer){
                PlayerPosition.Value = transform.position;
                PlayerRotation.Value = transform.rotation;
            } else {
                UpdatePlayerTransformServerRpc(transform.position, transform.rotation);
            }*/

            SetHeadRotationServerRpc(camera.transform.rotation);
            //Mathf.Lerp(PlayerHeadRotation.Value, headBone.transform.rotation, 0.1f);
    }
}
