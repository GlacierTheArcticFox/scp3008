using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//stolen from https://www.youtube.com/watch?v=_QajrabyTJc and modified by me

public class player_movement : MonoBehaviour {
    
    public CharacterController controller;

    public GameObject camera;

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
    bool isMoving;
    bool isHitGround;

    void Update() {
        isHitGround = isGrounded;

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        if (Input.GetButton("Sprint")) {
            speed = sprintSpeed;
            camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(camera.GetComponent<Camera>().fieldOfView, 70, 0.1f);;
        } else {
            speed = defaultSpeed;
            camera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(camera.GetComponent<Camera>().fieldOfView, 60, 0.1f);
        }

        direction.x = Input.GetAxis("Horizontal");
        direction.y = Input.GetAxis("Vertical");

        direction = Vector2.ClampMagnitude(direction, 1);

        if ((direction != Vector2.zero) && isGrounded) {
            if (!walkingSound.isPlaying) {
                walkingSound.pitch = Random.Range(0.6f, 0.7f);
                walkingSound.PlayDelayed(0.1f); //0.30f
            }
        } else {
            walkingSound.Stop();
        }

        Vector3 move = transform.right * direction.x + transform.forward * direction.y;

        controller.Move(move * speed * Time.deltaTime);
        
        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
