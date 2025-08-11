using UnityEngine;

public class Waypoint : MonoBehaviour
{
    private bool _isOccupied;
    public bool IsOccupied
    {
        get {  return _isOccupied; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            _isOccupied = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isOccupied = false;
    }
}