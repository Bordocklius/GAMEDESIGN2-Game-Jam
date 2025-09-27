using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField]
    private LayerMask _layerMask;

    private bool _isHit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if (obj != null && _layerMask == (_layerMask | (1 << obj.layer)))
        {
            _isHit = true;
        }
    }


}
