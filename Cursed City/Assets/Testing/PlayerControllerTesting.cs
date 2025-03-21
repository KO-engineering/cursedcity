using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTesting : Singleton<PlayerControllerTesting>
{
    public Animator animator;
    public CharacterController controller;
    public Transform cameraTransform;

    public float speed = 5f;
    public float runMultiplier = 2;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;
    public float turnSmoothTime = 0.1f;
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode aimKey = KeyCode.Mouse1;

    float horizontal;
    float vertical;
    float turnSmoothVelocity;
    bool isAiming;

    public bool allowMove = true;
    Vector3 velocity;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            GetComponent<PlayerShootingTesting>().enabled = false;
            GetComponentInChildren<CameraOrbit>().enabled = false;
            this.enabled = false;
        }
        if (Input.GetKeyDown(aimKey) && controller.isGrounded)
        {
            isAiming = !isAiming;
            CameraOrbit.Instance.AllowAimMode(isAiming);
            PlayerShootingTesting.Instance.ActivateAimRig(isAiming);
            animator?.SetBool("Idle Gun", isAiming);
        }
        if (allowMove)
        {
            // Get input
            horizontal = Input.GetAxisRaw("Horizontal");
            vertical = Input.GetAxisRaw("Vertical");
        }
        else
        {
            horizontal = 0;
            vertical = 0;
        }

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        bool moving = direction.magnitude >= 0.1f;
        bool running = Input.GetKey(runKey);
        bool jumping = Input.GetKey(jumpKey);

        float movementSpeed = running ? speed * runMultiplier : speed;

        animator?.SetBool("Running", running);
        animator?.SetBool("Walking", moving);
        animator?.SetBool("Grounded", controller.isGrounded);
        if (controller.isGrounded)
        {
            velocity.y = -2f; // Reset gravity when grounded

            if (jumping)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); // Correct jump calculation
                animator.SetTrigger("Jumping");
            }
        }

        // Apply movement
        if (moving)
        {
            if (!isAiming)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                transform.rotation = Quaternion.Euler(0f, angle, 0f);
                Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                controller.Move(moveDirection.normalized * movementSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 moveDirection = cameraTransform.right * direction.x + cameraTransform.forward * direction.z;
                moveDirection.y = 0;
                controller.Move(moveDirection.normalized * movementSpeed * Time.deltaTime);
            }
        }

        if (isAiming)
        {
            Vector3 cameraFwd = cameraTransform.forward;
            cameraFwd.y = 0;

            transform.rotation = Quaternion.LookRotation(cameraFwd);
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void LockPlayer(bool doLock)
    {
        allowMove = doLock == false;
    }
}
