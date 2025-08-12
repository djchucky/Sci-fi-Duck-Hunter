using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    Ray _ray;
    RaycastHit _hitInfo;
    private float _nextFire;
    private int _maxAmmo = 15;
    private int _currentAmmo;
    private AudioSource _audioSource;
    private Coroutine _fireRoutine;
    private Coroutine _reloadRoutine;

    [Header("Settings")]
    [SerializeField] private float _fireRate = 0.75f;
    [SerializeField] private AudioClip _fireClip;
    [SerializeField] private GameObject _light;

    [SerializeField] Camera _camera;

    void Start()
    {
        _currentAmmo = _maxAmmo;
        UIManager.Instance.UpdateAmmoCount(_currentAmmo);
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null )
        {
            Debug.Log("Audiosource is null");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > _nextFire)
        {
            if(_currentAmmo > 0)
            {
                _currentAmmo--;
                UIManager.Instance.UpdateAmmoCount(_currentAmmo);
                _nextFire = Time.time + _fireRate;
                _ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));

                Fire();
                if(Physics.Raycast(_ray,out _hitInfo,Mathf.Infinity, 1 << 6 | 1 << 7))
                {
                    Health health = _hitInfo.collider.GetComponent<Health>();
                    if (health != null)
                    {
                        health.Damage();
                    }
                }
            }

            else
            {
                ReloadAmmo();
            }

        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            ReloadAmmo();
        }
    }

    private void Fire()
    {
        _fireRoutine = StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        _audioSource.clip = _fireClip;
        _audioSource.Play();
        _light.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _light.SetActive(false);
        _fireRoutine = null;

    }

    private void ReloadAmmo()
    {
        if(_reloadRoutine == null && _currentAmmo != _maxAmmo)
        {
            _reloadRoutine = StartCoroutine(ReloadRoutine());
        }
    }

    IEnumerator ReloadRoutine()
    {
        yield return new WaitForSeconds(1.5f);
        _currentAmmo = _maxAmmo;
        UIManager.Instance.UpdateAmmoCount(_currentAmmo);
        _reloadRoutine = null;
    }
}
