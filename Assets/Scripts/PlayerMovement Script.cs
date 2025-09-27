using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovementScript : MonoBehaviour
{
    [SerializeField]
    private float mouseSensitivity = 2f;
    [SerializeField]
    private float movementSpeed = 5f;
    [SerializeField]
    private float acceleration = 10f;
    [SerializeField]
    private float jumpForce = 50f;
    [SerializeField]
    private LayerMask groundLayer;
    [SerializeField]
    private float groundCheckDistance = 0.3f;

    private Rigidbody _rb;
    private Transform _cameraTransform;
    private Vector2 _inputMovement;
    private float _xRotation = 0f;
    private bool _jumpRequested = false;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;

        _cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMouseLook();
        ReadInput();
        HandleJumpInput();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        ApplyJump();
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
        if (_jumpRequested)
        {
            _rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            _jumpRequested = false;
        }
    }
    private bool IsGrounded()
    {
        // Raycast down slightly below the player's position to check for ground
        return Physics.Raycast(transform.position, Vector3.down, 0.6f);
    }

}
