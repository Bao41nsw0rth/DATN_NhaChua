using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Di chuyển")]
    public float speed = 5f;
    public float jumpHeight = 2f;
    public float gravity = -9.81f;

    [Header("Camera")]
    public Transform cameraTransform;
    public float mouseSensitivity = 2f;

    [SerializeField] GameObject lightSource;

    [Header("Âm thanh bước chân")]
    [SerializeField] private float stepDelay = 0.7f;
    private float stepTimer = 0f;

    [Header("Túi đồ")] [SerializeField] private GameObject Inventory;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;
    private float xRotation = 0f;
    private bool lightOn = false;

    private float baseSpeed;
    private float baseStepDelay;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        lightSource.SetActive(false);

        baseSpeed = speed;
        baseStepDelay = stepDelay;
    }

    void Update()
    {
        HandleMovementAndJump();
        ApplyGravity();
        HandleCamera();
        HandleFlashlightToggle();
        HandleFootsteps();
    }

    void HandleMovementAndJump()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        HandlePlayerRun();
    }

    void HandlePlayerRun()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            speed = baseSpeed * 2f;
            stepDelay = baseStepDelay / 2f;
            stepTimer = 0f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speed = baseSpeed;
            stepDelay = baseStepDelay;
            stepTimer = 0f;
        }
    }

    void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleCamera()
    {
        if (Inventory.activeSelf) return;
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.Rotate(Vector3.up * mouseX);
        cameraTransform.rotation = Quaternion.Euler(xRotation, transform.eulerAngles.y, 0f);
        cameraTransform.position = transform.position + Vector3.up * 6f;
    }

    void HandleFlashlightToggle()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            lightOn = !lightOn;
            lightSource.SetActive(lightOn);
            AudioManager.instance.TryPlayTorchClick();
        }
    }

    void HandleFootsteps()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        bool isMoving = new Vector2(moveX, moveZ).magnitude > 0.1f;

        stepTimer -= Time.deltaTime;

        if (isMoving && isGrounded && stepTimer <= 0f)
        {
            AudioManager.instance.TryPlayFootSteps();
            stepTimer = stepDelay;
        }
    }
}