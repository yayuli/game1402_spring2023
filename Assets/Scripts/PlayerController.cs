using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Camera playerCam; //this is the camera in our game
    private Transform camContainer; //this is the container which we are going to use for rotating the camera
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
    // Start is called before the first frame update
    void Start()
    {
        playerCam = GetComponentInChildren<Camera>(); //This gets us the camera
        camContainer = playerCam.transform.parent; //this gets us the camera's parent's transform
    }

    // Update is called once per frame
    void Update()
    {
        //we are going to rotate our camera based on our mouse movement
        camContainer.Rotate(invert * Input.GetAxis("Mouse Y") * mouseYSensitivity, 0, 0); //Input.GetAxis("Mouse X/Y") gives us the mouse movement up or down of our character
        float rotationX = Input.GetAxis("Mouse X") * mouseXSensitivity;
        this.transform.Rotate(0, rotationX, 0);
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 input = new Vector3(horizontal, 0, vertical).normalized * speed;
        GetComponent<Rigidbody>().velocity = transform.TransformVector(input);
    }
}
