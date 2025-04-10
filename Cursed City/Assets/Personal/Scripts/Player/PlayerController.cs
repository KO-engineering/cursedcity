using UnityEngine;
using NaughtyAttributes;
public class PlayerController : Singleton<PlayerController>
{
    public Animator animator;
    public CharacterController controller;
    public Transform cameraTransform;
    public GameObject camera;

    public float speed = 5f;
    public float runMultiplier = 2;
    public float turnSmoothTime = 0.1f;
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode punchKey = KeyCode.Q;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode aimKey = KeyCode.Mouse1;

    float horizontal;
    float vertical;
    float turnSmoothVelocity;
    bool isAiming;

    public bool allowMove = true;

    void Start()
    {
        camera.SetActive(true);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save(); 
    }
    void Update()
    {
        if (Input.GetKeyDown(aimKey))
        {
            isAiming = !isAiming;
            CameraOrbit.Instance.AllowAimMode(isAiming);
            PlayerShooting.Instance.ActivateAimRig(isAiming);
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
        bool punching = Input.GetKey(punchKey);
        bool crouching = Input.GetKey(crouchKey);

        float movementSpeed = running ? speed * runMultiplier : speed;

        animator?.SetBool("Running", running);
        animator?.SetBool("Walking", moving);
        animator?.SetBool("Punching", punching);
        animator?.SetBool("Crouching", crouching);
        if (jumping)
        {
            animator.SetTrigger("Jumping");
            jumping = false;
        }

        if (moving)
        {
            if (isAiming == false)
            {
                float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cameraTransform.eulerAngles.y;

                // Smooth the rotation
                float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);

                // Rotate the player
                transform.rotation = Quaternion.Euler(0f, angle, 0f);

                // Move in the direction the player is facing
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
        Vector3 gravity = new Vector3(0, -9.81f, 0);
        controller.Move(gravity * Time.deltaTime);
    }

    public void LockPlayer(bool doLock)
    {
        allowMove = doLock == false;
    }
    public void Driving(){
        camera.SetActive(false);
        allowMove = false;
    }
}
