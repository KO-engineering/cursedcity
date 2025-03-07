using UnityEngine;

public class CameraOrbit : Singleton<CameraOrbit>
{
    public Transform player;
    public float mouseSensitivity = 2f;
    public float distanceFromPlayer = 5f;                       // Default distance from player\
    public Vector3 offset;
    public Vector2 pitchLimits = new Vector2(-30, 60);
    public LayerMask collisionLayers;                           // Define which layers the camera should collide with
    public float collisionOffset = 0.2f;                        // Minimum distance from obstacles to prevent clipping
    public bool allowLook = true;                               // Toggle camera control

    float yaw = 0f;
    float pitch = 0f;
    bool isDragging = false;
    bool isAiming;

    public void AllowLook(bool allow)
    {
        allowLook = allow;
    }

    public void AllowAimMode(bool aim)
    {
        isAiming = aim;
        Cursor.lockState = isAiming ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isAiming;
    }

    void LateUpdate()
    {
        if (!allowLook) return;

        if (isAiming)
        {
            yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
            pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);

            transform.rotation = Quaternion.Euler(pitch, yaw, 0);

            Vector3 aimPosition = player.position - transform.forward * distanceFromPlayer;
            aimPosition += transform. right * offset.x + transform.up * offset.y + transform.forward * offset.z;
            transform.position = HandleCameraCollision(player.position, aimPosition);
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
            }
            if (Input.GetMouseButtonUp(0))
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
