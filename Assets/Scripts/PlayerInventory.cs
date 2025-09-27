using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _ammoList;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void AddBallToInventory(GameObject gameObject)
    {
        _ammoList.Add(gameObject);
    }

    private void RemoveBallFromInventory(GameObject gameObject)
    {
        _ammoList.Remove(gameObject);
    }
}
