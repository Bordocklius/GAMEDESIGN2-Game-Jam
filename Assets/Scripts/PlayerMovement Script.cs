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

    private Rigidbody _rb;
    private Transform _cameraTransform;
    private Vector2 _inputMovement;
    private float _xRotation = 0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation; // Prevent unwanted rotation

        _cameraTransform = Camera.main.transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMouseLook();
        ReadInput(); // Optional if using new Input System
    }

    private void FixedUpdate()
    {
        ApplyMovement();
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
        // Old Input System
        _inputMovement = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        _inputMovement = Vector2.ClampMagnitude(_inputMovement, 1f);
    }

    private void ApplyMovement()
    {
        Vector3 moveDirection = transform.right * _inputMovement.x + transform.forward * _inputMovement.y;
        Vector3 targetVelocity = moveDirection.normalized * movementSpeed;

        Vector3 velocityChange = targetVelocity - _rb.linearVelocity;
        velocityChange.y = 0f; // Prevent jumping or flying

        _rb.AddForce(velocityChange * acceleration, ForceMode.Acceleration);
    }
}
