using System.Data.SqlTypes;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    //-- Rotation Variables --//
    [SerializeField] private Transform cameraTransform; // Drag your camera here
    float xRotation = 0f;
    float mouseSensitivity = 5f;

    //--Gravity --//
    private Vector3 velocity;
    readonly float gravity = -9.81f;
    readonly float jumpHeight = 1f;

    //-- Movement Variables --//
    readonly float moveSpeed = 5f;
    Vector3 inputMove;
    bool isGrounded;

    //-- Input Actions --//
    private InputAction lookAction;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction escAction;

    private void Start()
    {
        var playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions["Look"];
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        escAction = playerInput.actions["Pause"];
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        // -- UI Button Pushes -- //
        if(escAction.triggered)
        {
            UIManager.Instance.TogglePauseMenu();
        }


        // -- Player Button Pushes -- //
        Rotate();
        Movement();
        Jump();
        controller.Move((inputMove + velocity) * Time.deltaTime);
    }
    private void Jump()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y <= 0)
        {
            velocity.y = -2f;
        }
        if (jumpAction == null) return;
        if (jumpAction.triggered && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        // Gravity always applies
        velocity.y += gravity * Time.deltaTime;
    }
    private void Movement()
    {        
        if (moveAction == null) return;

        Vector2 input = moveAction.ReadValue<Vector2>();
        inputMove = (transform.right * input.x + transform.forward * input.y) * moveSpeed;        

    }
    private void Rotate()
    {

        if (lookAction == null) return;

        Vector2 mouseDelta = lookAction.ReadValue<Vector2>() * mouseSensitivity * Time.deltaTime;

        // Horizontal rotation (Y-axis)
        transform.Rotate(Vector3.up * mouseDelta.x);

        // Vertical rotation (X-axis)
        xRotation -= mouseDelta.y;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
    private void OnEnable()
    {
        lookAction?.Enable();
        moveAction?.Enable();
        jumpAction?.Enable();
    }
    private void OnDisable()
    {        
        lookAction?.Disable();
        moveAction?.Disable();
        jumpAction?.Disable();
    }
}
