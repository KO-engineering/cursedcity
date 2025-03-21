using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraOrbit : Singleton<CameraOrbit>
{
    public Transform player;
    public float mouseSensitivity = 2f;
    public float distanceFromPlayer = 5f;                       
    public Vector3 offset;
    public Vector2 pitchLimits = new Vector2(-30, 60);
    public LayerMask collisionLayers;                           
    public float collisionOffset = 0.2f;                        
    public bool allowLook = true;                               

    [Header("Smoothing Settings")]
    public float positionSmoothingSpeed = 5f;
    public float rotationSmoothingSpeed = 5f;

    float yaw = 0f;
    float pitch = 0f;
    bool isDragging = false;
    bool isAiming;

    Vector3 currentPosition;
    Quaternion currentRotation;

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

            Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);

            Vector3 aimPosition = player.position - targetRotation * Vector3.forward * distanceFromPlayer;
            aimPosition += transform.right * offset.x + transform.up * offset.y + transform.forward * offset.z;
            Vector3 targetPosition = HandleCameraCollision(player.position, aimPosition);

            // Smooth transition
            currentRotation = Quaternion.Lerp(currentRotation, targetRotation, rotationSmoothingSpeed * Time.deltaTime);
            currentPosition = Vector3.Lerp(currentPosition, targetPosition, positionSmoothingSpeed * Time.deltaTime);

            transform.rotation = currentRotation;
            transform.position = currentPosition;
        }
        else
        {
            if (Input.GetMouseButtonDown(0)) isDragging = true;
            if (Input.GetMouseButtonUp(0)) isDragging = false;

            if (isDragging)
            {
                yaw += Input.GetAxis("Mouse X") * mouseSensitivity;
                pitch -= Input.GetAxis("Mouse Y") * mouseSensitivity;
                pitch = Mathf.Clamp(pitch, pitchLimits.x, pitchLimits.y);
            }

            Quaternion targetRotation = Quaternion.Euler(pitch, yaw, 0);
            Vector3 desiredDirection = new Vector3(0, 0, -distanceFromPlayer);
            Vector3 desiredPosition = player.position + targetRotation * desiredDirection;
            Vector3 adjustedPosition = HandleCameraCollision(player.position, desiredPosition);

            // Smooth transition
            currentRotation = Quaternion.Lerp(currentRotation, targetRotation, rotationSmoothingSpeed * Time.deltaTime);
            currentPosition = Vector3.Lerp(currentPosition, adjustedPosition, positionSmoothingSpeed * Time.deltaTime);

            transform.rotation = currentRotation;
            transform.position = currentPosition;

            transform.LookAt(player.position);
        }
    }

    Vector3 HandleCameraCollision(Vector3 playerPosition, Vector3 desiredPosition)
    {
        RaycastHit hit;
        if (Physics.Linecast(playerPosition, desiredPosition, out hit, collisionLayers))
        {
            return hit.point + hit.normal * collisionOffset;
        }

        return desiredPosition;
    }
}
