using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    Ray _ray;
    RaycastHit _hitInfo;
    [SerializeField] private float _fireRate = 0.75f;
    private float _nextFire;

    [SerializeField] Camera _camera;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            _ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));

            if(Physics.Raycast(_ray,out _hitInfo,Mathf.Infinity, 1 << 6 | 1 << 7))
            {
                Debug.Log("Hit: " + _hitInfo.collider.name);
            }

        }
    }
}
