// PlayerMovement.cs

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 5f;
    public float runSpeed = 9f;
    public float jumpForce = 2f;
    public float gravity = -9.81f;

    [Header("Mouse Settings")]
    public float mouseSensitivity = 200f;
    public float crouchHeight = 1f;

    private CharacterController controller;
    private Vector3 velocity;
    private float originalHeight;
    private bool isCrouching = false;

    private Transform playerCamera;
    private float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (Camera.main != null)
        {
            playerCamera = Camera.main.transform;
        }
        else
        {
            Debug.LogError("MainCamera not found. Please ensure a camera is tagged as MainCamera.");
        }

        originalHeight = controller.height;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        RotateView();
        HandleCrouch();
    }

    void Move()
    {
        float speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        controller.Move(move * speed * Time.deltaTime);

        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }

        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void RotateView()
    {
        if (playerCamera == null)
            return;

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -75f, 75f);

        playerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            StartCoroutine(CrouchRoutine(isCrouching));
        }
    }

    IEnumerator CrouchRoutine(bool crouch)
    {
        float targetHeight = crouch ? crouchHeight : originalHeight;
        Vector3 targetCenter = crouch ? new Vector3(0, crouchHeight / 2, 0) : new Vector3(0, originalHeight / 2, 0);
        float currentHeight = controller.height;
        Vector3 currentCenter = controller.center;
        float elapsed = 0f;
        float duration = 0.2f; // Adjust duration as needed

        while (elapsed < duration)
        {
            controller.height = Mathf.Lerp(currentHeight, targetHeight, elapsed / duration);
            controller.center = Vector3.Lerp(currentCenter, targetCenter, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        controller.height = targetHeight;
        controller.center = targetCenter;
    }
}
