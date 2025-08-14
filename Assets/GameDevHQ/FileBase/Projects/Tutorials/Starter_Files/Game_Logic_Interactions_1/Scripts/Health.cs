using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] 
    private int _health = 2;
    [SerializeField]
    private int _currentHealth;
    [SerializeField] 
    private AudioClip _clip;
    private AudioSource _source;

    public Action OnDead;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        if( _source == null )
        {
            Debug.LogError("Audiosource is NULL on ENEMY");
        }
        _currentHealth = _health;
    }

    public void Damage(int damageAmount)
    {
        _currentHealth -= damageAmount;
        if(_currentHealth <= 0)
        {
            _source.clip = _clip;
            _source.Play();
            _currentHealth = _health;
            OnDead?.Invoke();
        }
    }
}
