using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    Ray _ray;
    RaycastHit _hitInfo;
    private bool _isShooting;
    private bool _isReloading;
    private float _nextFire;
    private int _currentAmmo;
    private AudioSource _audioSource;
    private Coroutine _fireRoutine;
    private Coroutine _reloadRoutine;

    [Header("Settings")]
    [SerializeField] private int _maxAmmo = 15;
    [SerializeField] private float _fireRate = 0.75f;
    [SerializeField] private int _damage = 1;
    [SerializeField] private AudioClip _fireClip;
    [SerializeField] private AudioClip _emptyClip;
    [SerializeField] private AudioClip _reloadClip;
    [SerializeField] private GameObject _light;

    [SerializeField] Camera _camera;

    private void OnEnable()
    {
        GameManager.OnGameOver += OnGameOver;
        SpawnManager.OnWinGame += WinGame;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= OnGameOver;
        SpawnManager.OnWinGame -= WinGame;
    }

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
        Shooting();

        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadAmmo();
        }
    }

    private void Shooting()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && Time.time > _nextFire)
        {
            if (_currentAmmo > 0)
            {
                _currentAmmo--;
                UIManager.Instance.UpdateAmmoCount(_currentAmmo);
                _nextFire = Time.time + _fireRate;
                _ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));

                Fire();
                Raycast();
            }

            else
            {
                if(!_isShooting && !_isReloading)
                {
                    if(!_audioSource.isPlaying)
                    {
                        _audioSource.PlayOneShot(_emptyClip);
                    }
                    Invoke("ReloadAmmo", 0.3f);
                }
            }

        }
    }

    private void Raycast()
    {
        if (Physics.Raycast(_ray, out _hitInfo, Mathf.Infinity, 1 << 6 | 1 << 7))
        {
            Health health = _hitInfo.collider.GetComponent<Health>();
            if (health != null)
            {
                health.Damage(_damage);
            }
        }
    }

    private void Fire()
    {
        if(_fireRoutine == null)
        _fireRoutine = StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        _isShooting = true;
        _audioSource.PlayOneShot(_fireClip);
        _light.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        _light.SetActive(false);
        _isShooting = false;
        _fireRoutine = null;

    }

    private void ReloadAmmo()
    {
        if(_reloadRoutine == null && _currentAmmo != _maxAmmo && Time.time > _nextFire)
        {
            _reloadRoutine = StartCoroutine(ReloadRoutine());
        }
    }

    IEnumerator ReloadRoutine()
    {
        _isReloading = true;
        _audioSource.PlayOneShot(_reloadClip);
        yield return new WaitForSeconds(3.5f);
        _currentAmmo = _maxAmmo;
        UIManager.Instance.UpdateAmmoCount(_currentAmmo);
        _isReloading = false;
        _reloadRoutine = null;
    }

    private void OnGameOver()
    {
        this.enabled = false;
    }

    private void WinGame()
    {
        this.enabled = false;
    }
}
