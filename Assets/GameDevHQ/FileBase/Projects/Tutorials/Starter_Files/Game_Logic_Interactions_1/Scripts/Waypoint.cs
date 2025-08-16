using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private bool _isOccupied;
    public bool IsOccupied => _isOccupied;

    private List<AIController> _enemies = new List<AIController>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            AIController enemy = other.GetComponent<AIController>();
            if (!_enemies.Contains(enemy))
            {
                _enemies.Add(enemy);
                _isOccupied = true;

                // Segnala al nemico di entrare in stato Hide
                enemy.EnterHiding(this);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            AIController enemy = other.GetComponent<AIController>();
            _enemies.Remove(enemy);
            _isOccupied = _enemies.Count > 0;
        }
    }

    public List<AIController> EnemiesReturn()
    {
        return _enemies;
    }
}
