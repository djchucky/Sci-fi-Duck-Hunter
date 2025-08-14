using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    private AudioSource _source;
    private Coroutine _explosionRoutine;
    private Health _health;
    private bool _hasActivated;
    private Vector3 _explosionPos;
    private Collider[] _colliders;

    [Header("Explosion Settings")]
    [SerializeField] private float _radious = 5f;
    [SerializeField] private int _power = 10;

    [Header("Settings Animations")]
    [SerializeField] private AudioClip _explosionClip;
    [SerializeField] private GameObject _flames;
    [SerializeField] private GameObject _explosion;

    private void OnEnable()
    {
        _source = GetComponent<AudioSource>();
        if(_source == null)
        {
            Debug.Log("Audiosource is NULL on Barrel");
        }
        _health = GetComponent<Health>();
        if(_health != null )
        {
            _health.OnDead += Explosion;
        }

    }

    private void Explosion()
    {
        if(_hasActivated && _explosionRoutine != null)
        {
            Debug.Log("Already hit");
            StopCoroutine(_explosionRoutine);
            _explosionRoutine = null;
            _flames.SetActive(true);
            ExplosionBarrel();
        }

        else if(_explosionRoutine == null && !_hasActivated)
        {
            _hasActivated = true;
            _flames.SetActive(true);           
            _explosionRoutine = StartCoroutine(ExplosionRoutine());
        }
    }

    IEnumerator ExplosionRoutine()
    {
        yield return new WaitForSeconds(3.5f);
        _explosion.SetActive(true);
        _source.Stop();
        _source.clip = _explosionClip;
        yield return new WaitForSeconds(0.55f);
        _source.Play();
        _flames.SetActive(false);
        GetComponent<MeshRenderer>().enabled = false;
        ExplosionForce();
        _explosionRoutine = null;
        yield return new WaitForSeconds(2f);
        DestroyBarrell();

    }

    private void ExplosionBarrel()
    {
        _explosion.SetActive(true);
        _source.clip = _explosionClip;
        _flames.SetActive(false);
        _source.Play();
        ExplosionForce();
        Invoke("DisableMeshRenderer", 0.5f);
        Invoke("DestroyBarrell", 2f);
    }

    private void DestroyBarrell()
    {
        Destroy(this.gameObject,0.5f);
    }

    private void ExplosionForce()
    {
        _explosionPos = transform.position;
        _colliders = Physics.OverlapSphere(_explosionPos, _radious);

        foreach(Collider hit in _colliders)
        {
            Health health = hit.GetComponent<Health>();
            if(health != null )
            {
                health.Damage(_power);
            }
        }
    }

    private void DisableMeshRenderer()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }

}
