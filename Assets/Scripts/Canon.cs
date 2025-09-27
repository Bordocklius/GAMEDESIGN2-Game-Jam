using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Canon : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _ammoList;
    [SerializeField]
    private int _maxAmmoCount;

    [SerializeField]
    private Transform _barrelPoint;
    [SerializeField]
    private float _suctionRange;
    [SerializeField]
    private float _suctionForce;
    [SerializeField]
    private LayerMask _layerMask;

    private bool _isCannonSucking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _ammoList = new List<GameObject>(_maxAmmoCount);
    }

    // Update is called once per frame
    void Update()
    {
        if(_isCannonSucking)
        {
            SuckIn();
        }
    }

    private void OnCannonSuck(InputValue inputValue)
    {
        _isCannonSucking = !_isCannonSucking;
    }

    private void SuckIn()
    {        
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

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.gameObject;
        if(obj != null && _layerMask == (_layerMask | (1 << obj.layer)) && _isCannonSucking)
        {
            if(_ammoList.Count < _maxAmmoCount)
            {
                _ammoList.Add(obj);
                obj.SetActive(false);
            }
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_barrelPoint.position, _suctionRange);
    }
}
