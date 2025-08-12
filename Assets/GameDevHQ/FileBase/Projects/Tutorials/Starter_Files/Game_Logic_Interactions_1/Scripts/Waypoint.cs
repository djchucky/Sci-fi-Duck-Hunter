using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private bool _isOccupied;
    public bool IsOccupied
    {
        get {  return _isOccupied; }
    }

    [SerializeField] private List<AIController> _enemies = new List<AIController>();


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _enemies.Add(other.GetComponent<AIController>());
            _isOccupied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            _enemies.Remove(other.GetComponent<AIController>());
            _isOccupied = _enemies.Count > 0;
        }
    }

    public List<AIController> EnemiesReturn() 
    {
        return _enemies;
    }
}