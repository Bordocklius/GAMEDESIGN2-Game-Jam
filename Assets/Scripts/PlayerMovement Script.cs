using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerInput))] // Ensure PlayerInput is attached
public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 6f;
    [SerializeField]
    private float controllerSensitivity = 200f; // 100x lower

    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private float acceleration = 10f;
    [SerializeField]
    private float jumpForce = 50f;
    [SerializeField]
    private LayerMask groundLayer;

    private Rigidbody _rb;
    private Transform _cameraTransform;
    private Vector2 _inputMovement;
    private float _xRotation = 0f;
    private bool _jumpRequested = false;

    private PlayerInput _playerInput;
    private float _currentLookSensitivity;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;

        _cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;

        _playerInput = GetComponent<PlayerInput>();
        SetLookSensitivity(_playerInput.currentControlScheme);
        _playerInput.onControlsChanged += OnControlsChanged;
    }

    private void Update()
    {
        //HandleMouseLook();
        //ReadInput();
        //HandleJumpInput();
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        Vector2 lookInput = _playerInput.actions["Look"].ReadValue<Vector2>();

        float lookX = lookInput.x * _currentLookSensitivity;
        float lookY = lookInput.y * _currentLookSensitivity;

        _xRotation -= lookY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * lookX);

    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyJump();
    }
    private void OnDestroy()
    {
        _playerInput.onControlsChanged -= OnControlsChanged;
    }

    private void OnControlsChanged(PlayerInput input)
    {
        SetLookSensitivity(input.currentControlScheme);
    }
    private void SetLookSensitivity(string controlScheme)
    {
        if (controlScheme == "Gamepad")
        {
            _currentLookSensitivity = controllerSensitivity;
        }
        else // Keyboard&Mouse
        {
            _currentLookSensitivity = mouseSensitivity;
        }

        Debug.Log($"Control scheme changed to {controlScheme}, sensitivity set to {_currentLookSensitivity}");
    }
    private void OnMove(InputValue inputValue)
    {
        _inputMovement = inputValue.Get<Vector2>();
    }

    
    private void OnJump(InputValue inputValue)
    {
        _jumpRequested = true;
    }

    private void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        _cameraTransform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void ReadInput()
    {
        _inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _inputMovement = Vector2.ClampMagnitude(_inputMovement, 1f);
    }

    private void HandleJumpInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            _jumpRequested = true;
        }
    }
    private void ApplyMovement()
    {
        Vector3 moveDirection = transform.right * _inputMovement.x + transform.forward * _inputMovement.y;
        Vector3 targetVelocity = moveDirection.normalized * movementSpeed;

        Vector3 velocityChange = targetVelocity - _rb.linearVelocity;
        velocityChange.y = 0f;

        _rb.AddForce(velocityChange * acceleration, ForceMode.Acceleration);
    }
    private void ApplyJump()
    {
        if (_jumpRequested && IsGrounded())
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _jumpRequested = false;
        }
    }
    private bool IsGrounded()
    {
        // Raycast down slightly below the player's position to check for ground
        return Physics.Raycast(transform.position, Vector3.down, 0.7f);
    }

}
