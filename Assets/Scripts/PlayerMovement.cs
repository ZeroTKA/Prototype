using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    Vector3 spawnPoint = new Vector3(6.85f, 0.9f, -2f);
    Vector3 spawnRotation = new Vector3(0, 180, 0);

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
    private float stepDistanceForSound = 1;
    private Vector3 lastStepPosition;

    //-- Input Actions --//
    private InputAction lookAction;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction escAction;
    private InputAction readyAction;

    // -- Specialty Methods -- //
    private void Start()
    {
        if (controller == null)
            Debug.LogError("[PlayerMovement] CharacterController not assigned.");
        if (cameraTransform == null)
            Debug.LogError("[PlayerMovement] Camera Transform not assigned.");
        if (TheDirector.Instance == null)
        {
            Debug.LogError("[PlayerMovment] TheDirector is null. That's not a good START");
        }
        else {TheDirector.Instance.OnGameStateChanged += Restart; }

        if (TryGetComponent<PlayerInput>(out var playerInput))
        {
            lookAction = playerInput.actions["Look"];
            moveAction = playerInput.actions["Move"];
            jumpAction = playerInput.actions["Jump"];
            escAction = playerInput.actions["Pause"];
            readyAction = playerInput.actions["Ready"];
        }
        else
        {
            Debug.LogError("[PlayerMovement] Unable to find PlayerInput.");
        }
        Cursor.lockState = CursorLockMode.Locked;
        lastStepPosition = transform.position;
    }

    private void Update()
    {
        // -- UI Button Pushes -- //
        if (escAction.triggered)
        {
            if (UIManager.Instance == null)
            {
                Debug.LogError("[PlayerMovement] Can't find the UIManager.");
            }
            else { UIManager.Instance.TogglePauseMenu(); }
        }
        if (readyAction.triggered && TheDirector.Instance.CurrentState == TheDirector.GameState.Shop)
        {
            TheDirector.Instance.SetGameState(TheDirector.GameState.Wave);
        }


        // -- Player Button Pushes -- //
        if(TheDirector.Instance.CurrentState == TheDirector.GameState.Wave || TheDirector.Instance.CurrentState == TheDirector.GameState.Shop)
        {
            Rotate();
            Movement();
            Jump();
            if (controller == null)
            {
                Debug.LogError("[PlayerMovment] Can't find the UIManager in Update");
            }
            else { controller.Move((inputMove + velocity) * Time.deltaTime); }
            if (isGrounded)
            {
                float distanceMoved = Vector3.Distance(transform.position, lastStepPosition);
                if (distanceMoved > stepDistanceForSound)
                {
                    SoundManager.instance.PlaySound(SoundManager.SoundType.Stone, transform.position);
                    lastStepPosition = transform.position; // reset last step so we can measure from the new position and not the old.)
                }
            }
        }

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
        if (TheDirector.Instance == null)
        {
            Debug.LogError("[PlayerMovment] TheDirector is null. That's not a good way to DISABLE");
        }
        else {TheDirector.Instance.OnGameStateChanged -= Restart;}

    }
    // -- Main Methods -- //
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
    private void Restart(TheDirector.GameState gameState)
    {
        if(gameState == TheDirector.GameState.Restart)
        {
            velocity = Vector3.zero;
            isGrounded = true;
            gameObject.transform.position = spawnPoint; // a hair above the original spawn in Y axis. 
            gameObject.transform.rotation = Quaternion.Euler(spawnRotation); // camera horizontal reset
            
            xRotation = 0f; // camera vertical look reset
            cameraTransform.localRotation = Quaternion.Euler(0f, 0f, 0f); // camera vertical look reset
            if (SyncCoordinator.Instance == null)
            {
                Debug.LogError("[PlayerMovement] SyncCooridnator is null. Can't restart properly");
            }
            SyncCoordinator.Instance.RestartReady();
        }
    }

}
