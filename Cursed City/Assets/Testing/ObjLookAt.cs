using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjLookAt : MonoBehaviour
{
    public enum Axis
    {
        X, X_NEG,
        Y, Y_NEG,
        Z, Z_NEG
    }

    [Header("Target Settings")]
    public Transform target;

    [Header("Axis Settings")]
    public Axis forwardAxis = Axis.Z;
    public Axis upAxis = Axis.Y;

    [Header("World Up Settings")]
    public Transform worldUpObject;  // Optional world up object
    public Axis worldUpAxis = Axis.Y;

    [Header("Rotation Control")]
    public bool maintainOffset = true;
    public float rotationSpeed = 5f;

    [Header("Debug Options")]
    public bool showDebugLine = true;
    public Color debugLineColor = Color.green;
    public float debugLineLength = 2f;

    private Quaternion initialRotation;

    void Start()
    {
        if (maintainOffset)
        {
            initialRotation = transform.rotation;
        }
    }

    void Update()
    {
        if (!target) return;

        Vector3 forwardDirection = ConvertAxis(forwardAxis);
        Vector3 upDirection = ConvertAxis(upAxis);

        Vector3 targetDirection = (target.position - transform.position).normalized;
        Vector3 worldUpDir = worldUpObject ? worldUpObject.TransformDirection(ConvertAxis(worldUpAxis)) : Vector3.up;

        Quaternion targetRotation = Quaternion.LookRotation(targetDirection, worldUpDir);

        if (maintainOffset)
        {
            targetRotation *= initialRotation;
        }

        // Smooth Rotation
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    void OnDrawGizmos()
    {
        if (showDebugLine && target)
        {
            Gizmos.color = debugLineColor;
            Vector3 debugDirection = transform.TransformDirection(ConvertAxis(forwardAxis)) * debugLineLength;
            Gizmos.DrawLine(transform.position, transform.position + debugDirection);

            // Show target direction as well
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }

    // Convert Axis Enum to Vector3
    private Vector3 ConvertAxis(Axis axis)
    {
        switch (axis)
        {
            case Axis.X:     return Vector3.right;
            case Axis.X_NEG: return Vector3.left;
            case Axis.Y:     return Vector3.up;
            case Axis.Y_NEG: return Vector3.down;
            case Axis.Z:     return Vector3.forward;
            case Axis.Z_NEG: return Vector3.back;
            default:         return Vector3.forward;
        }
    }
}
