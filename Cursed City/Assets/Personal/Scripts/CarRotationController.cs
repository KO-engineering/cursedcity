using UnityEngine;

public class CarRotationController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 30f;
    private bool isDragging = false;
    private float lastMouseX;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button pressed
        {
            isDragging = true;
            lastMouseX = Input.mousePosition.x;
        }
        else if (Input.GetMouseButtonUp(0)) // Left mouse button released
        {
            isDragging = false;
        }

        if (isDragging)
        {
            float deltaX = Input.mousePosition.x - lastMouseX;
            transform.Rotate(Vector3.up, -deltaX * rotationSpeed * Time.deltaTime, Space.World);
            lastMouseX = Input.mousePosition.x;
        }
    }
}