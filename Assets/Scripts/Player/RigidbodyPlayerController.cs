using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyPlayerController : MonoBehaviour
{
    // public vars
	public float mouseSensitivityX = 1;
	public float mouseSensitivityY = 1;
	public float walkSpeed = 6;
	public float jumpForce = 220;
	public LayerMask groundedMask;

    float rotationX = 0;
    public float lookXLimit = 85.0f;
    float rotationY = 0;
	
	// System vars
	bool grounded;
	Vector3 moveAmount;
	Vector3 smoothMoveVelocity;
	float verticalLookRotation;
	Transform cameraTransform;
	Rigidbody rigidbody;


    [HideInInspector]
    public bool canMove = true;

    private Light torchLight;
    private float torchIntensity;
	
	
	void Awake() {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		cameraTransform = GetComponentInChildren<Camera>().transform;
		rigidbody = GetComponent<Rigidbody>();

        torchLight = cameraTransform.GetComponentInChildren<Light>();
        torchIntensity = torchLight.intensity;
        torchLight.intensity = 0;
	}
	
	void Update() {

        if (canMove)
        {
            // Look rotation:
            rotationX += -Universe.Instance.RewiredPlayer.GetAxis("Look Vertical") * mouseSensitivityX;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            cameraTransform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            transform.rotation *= Quaternion.Euler(0, Universe.Instance.RewiredPlayer.GetAxis("Look Horizontal") * mouseSensitivityY, 0);
            
            // Calculate movement:
            float inputX = Universe.Instance.RewiredPlayer.GetAxis("Horizontal");
            float inputY = Universe.Instance.RewiredPlayer.GetAxis("Vertical");
            
            Vector3 moveDir = new Vector3(inputX, 0, inputY);
            if(moveDir.sqrMagnitude > 1) {
                moveDir.Normalize();
            }
            Vector3 targetMoveAmount = moveDir * walkSpeed;
            moveAmount = targetMoveAmount; //Vector3.SmoothDamp(moveAmount, targetMoveAmount, ref smoothMoveVelocity,.15f);
            
            // Jump
            if (Universe.Instance.RewiredPlayer.GetButtonDown("Jump")) {
                if (grounded) {
                    rigidbody.AddForce(transform.up * jumpForce);
                }
            }
        }
		
		// Grounded check
		Ray ray = new Ray(transform.position, -transform.up);
		RaycastHit hit;
		
		if (Physics.Raycast(ray, out hit, 1 + .1f, groundedMask)) {
			grounded = true;
		}
		else {
			grounded = false;
		}

        if(Universe.Instance.RewiredPlayer.GetButtonDown("Torch")) {
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
	
	void FixedUpdate() {
        if (canMove)
        {
		    // Apply movement to rigidbody
            Vector3 localMove = transform.TransformDirection(moveAmount) * Time.fixedDeltaTime;
            rigidbody.MovePosition(rigidbody.position + localMove);
        }
	}
}
