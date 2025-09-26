using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : MonoBehaviour
{
    [SerializeField]
    private float _movementSpeed;
    [SerializeField]
    private CharacterController _characterController;
    [SerializeField]
    private Transform _playerTransform;

    private Vector2 _movement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(0, 0, 0);
        movement += _movement.x * _playerTransform.right * _movementSpeed * Time.deltaTime;
        movement += _movement.y * _playerTransform.forward * _movementSpeed * Time.deltaTime;

        //Vector3 movement = new Vector3(_movement.x, 0, _movement.y) * _movementSpeed * Time.deltaTime;
        _characterController.Move(movement);
    }

    // OnMove method called by inputsystem
    private void OnMove(InputValue inputValue)
    {
        Debug.Log(inputValue.Get<Vector2>().ToString());
        _movement = inputValue.Get<Vector2>();
    }    

}
