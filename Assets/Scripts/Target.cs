using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layerMask;

    private bool _isHit;
    private bool _isDown;

    [SerializeField]
    private float _slerpDuration;
    [SerializeField]
    private float _targetAngle;
    [SerializeField]
    private Transform _transform;
    [SerializeField]
    private float _rotationSpeed;

    private Quaternion _targetRotation;

    // Update is called once per frame
    void Update()
    {
        if(_isHit && !_isDown)
        {
            _transform.rotation = Quaternion.Slerp(_transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);
            if(Quaternion.Angle(_transform.rotation, _targetRotation) < 0.1f)
            {
                _isDown = true;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj != null && _layerMask == (_layerMask | (1 << obj.layer)) && !_isHit)
        {
            BallpitBall ball = obj.GetComponent<BallpitBall>();
            if (ball != null && ball.IsShot)
            {
                Debug.Log("Hit target");
                _isHit = true;
                _isDown = false;
                _targetRotation = Quaternion.Euler(_targetAngle, 0f, 0f);
            }            
        }
    }


}
