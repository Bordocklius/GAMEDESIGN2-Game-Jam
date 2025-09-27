using UnityEngine;

public class BallpitBall : MonoBehaviour
{
    public bool IsShot = false;

    public Rigidbody Rb;

    [SerializeField]
    private float _shotLifetime;

    private float _shotTimer;

    private void Start()
    {
        Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsShot)
        {
            _shotTimer += Time.deltaTime;
            if(_shotTimer >= _shotLifetime )
            {
                IsShot = false;
                Rb.collisionDetectionMode = CollisionDetectionMode.Discrete;
            }
        }
    }
}
