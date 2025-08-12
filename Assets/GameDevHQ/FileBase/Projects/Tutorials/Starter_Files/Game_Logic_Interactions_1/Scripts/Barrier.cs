using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private Health _health;
    private AudioSource _audioSource;

    private void OnEnable()
    {
        _health = GetComponent<Health>();
        _audioSource = GetComponent<AudioSource>();
        if (_health == null)
        {
            Debug.LogError("Health is NULL on BARRIER");
        }

        if (_health != null)
        {
            _health.OnDead += OnDeath;
        }
    }

    private void OnDisable()
    {
        if (_health != null)
            _health.OnDead -= OnDeath;
    }


    private void OnDeath()
    {
        Destroy(this.gameObject, 0.2f);
    }


}
