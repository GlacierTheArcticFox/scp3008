using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
//using Unity.Netcode.NetworkVariable;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player : NetworkBehaviour {
    //public NetworkVariable<Vector3> PlayerPosition = new NetworkVariable<Vector3>();
    public NetworkVariable<Quaternion> PlayerHeadRotation = new NetworkVariable<Quaternion>();
    public NetworkVariable<FixedString64Bytes> PlayerName = new NetworkVariable<FixedString64Bytes>();
    public NetworkVariable<int> AnimState = new NetworkVariable<int>();

    public CharacterController controller;
    public GameObject camera;
    public GameObject playerModel;
    public GameObject headBone;
    public GameObject nameTag;
    public GameObject mAnimation;
    
    public float defaultSpeed = 3f;
    public float sprintSpeed = 5f;
    public float gravity = -9.81f;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;
    Vector2 direction;

    float speed;
    public bool isGrounded;

    [ServerRpc(Delivery = RpcDelivery.Unreliable)]
    void SetHeadRotationServerRpc(Quaternion rotation) {
        PlayerHeadRotation.Value = rotation;
    }

    [ServerRpc]
    public void SetPlayerNameServerRpc(string name) {
        PlayerName.Value = name;
    }

    [ServerRpc]
    public void SetPlayerAnimStateServerRpc(int state) {
        AnimState.Value = state;
    }

    public override void OnNetworkSpawn() {
        if (IsOwner) {
            gameObject.transform.position = new Vector3(4, 1f, 4);
            gameObject.name = "LocalPlayer";
            GameObject.Find("LocalPlayer/Body/Male_Standing").SetActive(false);
            nameTag.SetActive(false);
        } else {
            gameObject.transform.position = new Vector3(4, 1f, 4);
            camera.SetActive(false);
        }
    }

    void Update() {
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (!IsOwner){
                camera.transform.rotation = Quaternion.Lerp(camera.transform.rotation, PlayerHeadRotation.Value, 0.1f);
                nameTag.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = false;
                nameTag.transform.parent.GetComponent<VerticalLayoutGroup>().enabled = true;
                mAnimation.GetComponent<Animator>().SetInteger("AnimState", AnimState.Value);
                if (nameTag.GetComponent<TextMeshProUGUI>().text != PlayerName.Value) {
                    gameObject.name = PlayerName.Value.ToString();
                    nameTag.GetComponent<TextMeshProUGUI>().text = PlayerName.Value.ToString();
                    
                }
                return;
            }

            if (isGrounded && velocity.y < 0) {
                velocity.y = -2f;
            }

            if (Cursor.lockState != CursorLockMode.None){
                direction.x = Input.GetAxis("Horizontal");
                direction.y = Input.GetAxis("Vertical");
            }

            direction = Vector2.ClampMagnitude(direction, 1);

            /*if ((direction != Vector2.zero) && isGrounded) {
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
            }*/

            if ((direction != Vector2.zero) && isGrounded) {
                if (speed == sprintSpeed) {
                    mAnimation.GetComponent<Animator>().SetInteger("AnimState", 2);
                    if(AnimState.Value != 2) {
                        SetPlayerAnimStateServerRpc(2);
                    }
                } else {
                    mAnimation.GetComponent<Animator>().SetInteger("AnimState", 1);
                    if(AnimState.Value != 1) {
                        SetPlayerAnimStateServerRpc(1);
                    }
                }
            } else {
                mAnimation.GetComponent<Animator>().SetInteger("AnimState", 0);
                if(AnimState.Value != 0) {
                    SetPlayerAnimStateServerRpc(0);
                }
            }

            Vector3 move = transform.right * direction.x + transform.forward * direction.y;

            if (Input.GetButton("Sprint") && move != Vector3.zero && Cursor.lockState != CursorLockMode.None/* && GetComponent<player_logic>().stamina > 0f*/) {
                speed = sprintSpeed;
                camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(camera.GetComponent<Camera>().fieldOfView, 70, 0.1f);
                //GetComponent<player_logic>().stamina -= 10f*Time.deltaTime;
                //GetComponent<player_logic>().energy -= 0.5f*Time.deltaTime;
            } else if ( move != Vector3.zero && Cursor.lockState != CursorLockMode.None ) {
                speed = defaultSpeed;
                camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(camera.GetComponent<Camera>().fieldOfView, 60, 0.1f);
            } else {
                
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
