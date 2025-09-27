using UnityEngine;

public class BallpitBall : MonoBehaviour
{
    public bool IsShot = false;

    [SerializeField]
    private float _shotLifetime;

    private float _shotTimer;

    // Update is called once per frame
    void Update()
    {
        if(IsShot)
        {
            _shotTimer += Time.deltaTime;
            if(_shotTimer >= _shotLifetime )
            {
                IsShot = false;                
            }
        }
    }
}
