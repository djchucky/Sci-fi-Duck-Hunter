using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int _health = 2;
    [SerializeField] private AudioClip _clip;
    private AudioSource _source;

    public Action OnDead;

    private void Start()
    {
        _source = GetComponent<AudioSource>();
        if( _source == null )
        {
            Debug.LogError("Audiosource is NULL on ENEMY");
        }
    }

    public void Damage()
    {
        _health--;
        if(_health <= 0)
        {
            _source.clip = _clip;
            _source.Play();
            OnDead?.Invoke();
        }
    }
}
