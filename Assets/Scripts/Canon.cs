using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Canon : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> AmmoList = new List<GameObject>();
    [SerializeField]
    public int MaxAmmoCount;

    [SerializeField]
    private Transform _barrelPoint;
    [SerializeField]
    private float _suctionRange;
    [SerializeField]
    private float _suctionForce;
    [SerializeField]
    private LayerMask _layerMask;

    [SerializeField]
    private Camera _mainCamera;
    [SerializeField]
    private Transform _crosshairTransform;
    [SerializeField]
    private float _range;
    [SerializeField]
    private float _offsetStrengthMax;
    [SerializeField]
    private float _shootForce;

    public bool IsCannonSucking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(IsCannonSucking)
        {
            SuckIn();
        }
    }


    private void OnCannonSuck(InputValue inputValue)
    {
        IsCannonSucking = !IsCannonSucking;
    }

    private void SuckIn()
    {
        if (AmmoList.Count == MaxAmmoCount)
            return;

        List<Collider> colliders = Physics.OverlapSphere(_barrelPoint.position, _suctionRange, _layerMask).ToList();
        foreach(Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if(rb != null)
            {
                Vector3 direction = (_barrelPoint.position - rb.position).normalized;
                rb.AddForce(direction * _suctionForce, ForceMode.Acceleration);
            }
        }
    }

    private void OnCannonShoot(InputValue inputValue)
    {
        if (IsCannonSucking || AmmoList.Count == 0)
            return;

        GameObject obj = AmmoList[AmmoList.Count - 1].gameObject;
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if(rb != null )
        {
            Vector3 direction = GetAimDirection();
            obj.transform.position = _barrelPoint.position;
            obj.SetActive(true);
            rb.AddForce(direction * _shootForce, ForceMode.Impulse);
            AmmoList.Remove(obj);
        }

    }

    private Vector3 GetAimDirection()
    {
        // Shoot ray to crosshair
        Ray ray = _mainCamera.ScreenPointToRay(_crosshairTransform.position);
        RaycastHit hit;
        Debug.DrawRay(ray.origin, ray.direction * _range, Color.red, 2f, true);
        Physics.Raycast(ray, out hit, _range);

        if (hit.collider == null)
        {
            hit.point = ray.origin + ray.direction * _range;
        }

        
        Debug.DrawLine(ray.origin, hit.point, Color.blue, 2f, true);

        // Get direction from shootpoint to hitpoint
        Vector3 direction = (hit.point - _barrelPoint.position).normalized;
        float randomStrength = Random.Range(0, _offsetStrengthMax);
        direction = (direction + Random.insideUnitSphere * randomStrength).normalized;
        Debug.DrawLine(_barrelPoint.position, direction * _range, Color.green, 2f);

        return direction;

    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    GameObject obj = collision.gameObject;
    //    if(obj != null && _layerMask == (_layerMask | (1 << obj.layer)) && _isCannonSucking)
    //    {
    //        if(AmmoList.Count < MaxAmmoCount)
    //        {
    //            AmmoList.Add(obj);
    //            obj.SetActive(false);
    //        }
    //    }
    //}


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_barrelPoint.position, _suctionRange);
    }
}
