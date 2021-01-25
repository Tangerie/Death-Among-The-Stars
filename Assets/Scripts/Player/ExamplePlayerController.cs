using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

[RequireComponent(typeof(CharacterController))]

public class ExamplePlayerController : MonoBehaviour
{
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Camera playerCamera;

    [Range(0.2f, 3)]
    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;

    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero;
    float rotationX = 0;

    [HideInInspector]
    public bool canMove = true;

    private Player player;

    private Light torchLight;
    private float torchIntensity;

    void Start()
    {
        characterController = GetComponent<CharacterController>();

        torchLight = playerCamera.GetComponentInChildren<Light>();
        torchIntensity = torchLight.intensity;
        torchLight.intensity = 0;

        // Lock cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        player = Universe.Instance.RewiredPlayer;
    }

    void LateUpdate()
    {
        if(Universe.Instance.isPaused) {
            return;
        }
        // We are grounded, so recalculate move direction based on axes
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);
        // Press Left Shift to run
        bool isRunning = player.GetButton("Sprint");
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * player.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * player.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (player.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Player and Camera rotation
        if (canMove)
        {
            rotationX += -player.GetAxis("Look Vertical") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            transform.rotation *= Quaternion.Euler(0, player.GetAxis("Look Horizontal") * lookSpeed, 0);
        }

        if(player.GetButtonDown("Torch")) {
            if(torchLight.intensity == 0) {
                LeanTween.value(torchLight.gameObject, (intensity) => {
                    torchLight.intensity = intensity;
                }, 0, torchIntensity, 0.2f);
            } else {
                LeanTween.value(torchLight.gameObject, (intensity) => {
                    torchLight.intensity = intensity;
                }, torchIntensity, 0, 0.2f);
            }
        }
    }
}