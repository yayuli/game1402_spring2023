using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Unit
{
    private Camera playerCam;// this is the camera in our game
    private Transform camContainer;// this is the container which we are going to use for rotating the camera
    [SerializeField]
    float speed = 5;
    [SerializeField] 
    float mouseXSensitivity = 1;
    [SerializeField]
    float mouseYSensitivity = 1;
    [SerializeField] 
    float jumpHeight = 15.0f;
    [SerializeField] 
    float invert = 1.0f;
    [SerializeField] 
    LayerMask obstacleLayerMask;//LayerMask for camera collision detection
    [SerializeField] 
    float maxDistance = 2f;//This variable indicates the maximum distance between the camera and the player
    [SerializeField] 
    float zoomPullback = 1f;//This variable determines how far back the camera should pull after shooting an object
    [SerializeField]
    float cameraSmoothing = 5f; // Added camera smoothing parameter

    // Animation changes
    private Vector3 animatorInput;
    private Animator animator;
    
    private float currentDistance; // Distance between the camera and the player

    protected override void Start()
    {
        base.Start();
        playerCam = GetComponentInChildren<Camera>();
        camContainer = playerCam.transform.parent;
        animator = GetComponent<Animator>();

        currentDistance = maxDistance;
        obstacleLayerMask = LayerMask.GetMask("Obstacle");//Determine what this layer should be considered as an obstacle for the camera

    }

    void Update()
    {
        camContainer.Rotate(invert * Input.GetAxis("Mouse Y") * mouseYSensitivity, 0, 0);
        float rotationX = Input.GetAxis("Mouse X") * mouseXSensitivity;
        transform.Rotate(0, rotationX, 0);
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(horizontal, 0, vertical).normalized * speed;
        animatorInput = Vector3.Lerp(animatorInput, input, 0.4f);
        animator.SetFloat("HorizontalSpeed", animatorInput.x * 2);
        animator.SetFloat("VerticalSpeed", animatorInput.z * 2);


        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Debug.Log("Jump jump. Kriss kross will make you jump jump");
            input.y = jumpHeight;
            animator.SetTrigger("Jumping");
        }
        else
        {
            input.y = GetComponent<Rigidbody>().velocity.y;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            Ray ray = new Ray(GetEyesPosition(), playerCam.transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, obstacleLayerMask))
            {
                ShootAt(hit);
                PullCameraIn();
            }
            else
            {
                Vector3 targetPos = playerCam.transform.position + playerCam.transform.forward * maxDistance;
                ShowLasers(targetPos);
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            animator.SetTrigger("Taunt");
        }

        GetComponent<Rigidbody>().velocity = transform.TransformVector(input);
        HandleCameraCollision();
    }

    //This variable tracks the current distance between the camera and the player
    private void HandleCameraCollision()
    {
        RaycastHit hit;
        Vector3 desiredPosition = camContainer.position + camContainer.forward * currentDistance;

        if (Physics.Linecast(camContainer.position, desiredPosition, out hit, obstacleLayerMask))
        {
            currentDistance = Mathf.Clamp(hit.distance - zoomPullback, 0f, maxDistance);
        }
        else
        {
            currentDistance = maxDistance;
        }

        // Smoothly move the camera position
        Vector3 targetCamPos = camContainer.localPosition;
        targetCamPos.z = -currentDistance;
        playerCam.transform.localPosition = Vector3.Lerp(playerCam.transform.localPosition, targetCamPos, cameraSmoothing * Time.deltaTime);
    }

    //Called when shooting an object. It pulls the camera zoomPullback by reducing the current distance according to the value
    private void PullCameraIn()
    {
        currentDistance = Mathf.Clamp(currentDistance - zoomPullback, 0f, maxDistance);
        Vector3 targetCamPos = camContainer.localPosition;
        targetCamPos.z = -currentDistance;//Modified to set the required distance between the camera and the player

        playerCam.transform.localPosition = new Vector3(0f, 0f, -currentDistance);
    }
}
