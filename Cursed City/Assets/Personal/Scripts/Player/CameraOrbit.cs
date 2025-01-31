using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform player;
    public float mouseSensitivity = 2f;
    public float distanceFromPlayer = 5f;                       // Default distance from player
    public Vector2 pitchLimits = new Vector2(-30, 60);
    public LayerMask collisionLayers;                           // Define which layers the camera should collide with
    public float collisionOffset = 0.2f;                        // Minimum distance from obstacles to prevent clipping
    public bool allowLook = true;                               // Toggle camera control

    float yaw = 0f;
    float pitch = 0f;
    bool isDragging = false;

    public void AllowLook(bool allow)
    {
        allowLook = allow;
    }

    void LateUpdate()
    {
        if (!allowLook) return;

        // Check if the left or right mouse button is being held to enable dragging
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            isDragging = true;
        }
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            // Get mouse input for camera rotation
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);
        }

        // Calculate desired camera position
        Vector3 desiredDirection = new Vector3(0, 0, -distanceFromPlayer);
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = player.position + rotation * desiredDirection;

        // Handle collision
        Vector3 adjustedPosition = HandleCameraCollision(player.position, desiredPosition);

        // Set the camera position and rotation
        transform.position = adjustedPosition;
        transform.LookAt(player.position);
    }

    Vector3 HandleCameraCollision(Vector3 playerPosition, Vector3 desiredPosition)
    {
        RaycastHit hit;

        // Cast a ray from the player to the desired camera position
        if (Physics.Linecast(playerPosition, desiredPosition, out hit, collisionLayers))
        {
            // Adjust the camera position to the collision point with an offset
            return hit.point + hit.normal * collisionOffset;
        }

        // If no collision, return the desired position
        return desiredPosition;
    }
}
