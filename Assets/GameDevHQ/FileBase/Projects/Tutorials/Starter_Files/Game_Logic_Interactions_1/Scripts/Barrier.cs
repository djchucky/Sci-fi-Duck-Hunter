using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrier : MonoBehaviour
{
    private Health _health;
    private AudioSource _audioSource;
    private Coroutine _barrierRoutine;

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
            _health.OnDead += OnBarrierDestroyed;
        }
    }

    private void OnDisable()
    {
        if (_health != null)
            _health.OnDead -= OnBarrierDestroyed;
    }


    private void OnBarrierDestroyed()
    {
        _barrierRoutine  = StartCoroutine(BarrierRoutine());   
    }

    IEnumerator BarrierRoutine()
    {
        yield return null;
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = false;
        }

        GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(Random.Range(2f, 6f));
        foreach (MeshRenderer renderer in GetComponentsInChildren<MeshRenderer>())
        {
            renderer.enabled = true;
        }
        GetComponent<BoxCollider>().enabled = true;
        
        _barrierRoutine = null;
    }


}
