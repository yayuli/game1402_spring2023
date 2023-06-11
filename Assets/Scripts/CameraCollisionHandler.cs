using UnityEngine;

public class CameraCollisionHandler : MonoBehaviour
{
    [SerializeField] private Transform mainCameraTransform; // Reference to the main camera's transform component
    [SerializeField] private LayerMask obstacleLayerMask; // Layers to consider as obstacles
    [SerializeField] private float maxDistance = 2f; // Maximum distance to move the camera backward

    private Vector3 initialCameraPosition; // Initial local position of the camera

    private void Start()
    {
        initialCameraPosition = mainCameraTransform.localPosition;
    }

    private void LateUpdate()
    {
        // Perform a raycast from the camera pivot position to the camera's position
        RaycastHit hit;
        if (Physics.Linecast(transform.position, mainCameraTransform.position, out hit, obstacleLayerMask))
        {
            // Adjust the camera position to the hit point, plus an offset to prevent clipping
            mainCameraTransform.localPosition = new Vector3(0f, 0f, -hit.distance + maxDistance);
        }
        else
        {
            // Move the camera back to its initial position if no obstacle is hit
            mainCameraTransform.localPosition = initialCameraPosition;
        }
    }
}
