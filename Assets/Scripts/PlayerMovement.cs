using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform; // Drag your camera here
    private float mouseSensitivity = 5f;
    private float moveSpeed = 5f;

    private InputAction lookAction;
    private InputAction moveAction;
    private float xRotation = 0f;

    private void Start()
    {
        var playerInput = GetComponent<PlayerInput>();
        lookAction = playerInput.actions["Look"];
        moveAction = playerInput.actions["Move"];
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Rotate();
        Movement();
    }
    private void Movement()
    {
        if (moveAction == null) return;

        Vector2 input = moveAction.ReadValue<Vector2>();
        Vector3 move = transform.right * input.x + transform.forward * input.y;
        transform.position += move * moveSpeed * Time.deltaTime;
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
    }
    private void OnDisable()
    {
        lookAction?.Disable();
        moveAction?.Disable();
    }
}
