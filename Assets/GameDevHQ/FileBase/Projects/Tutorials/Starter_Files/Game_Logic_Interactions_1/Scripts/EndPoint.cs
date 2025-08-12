using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EndPoint : MonoBehaviour
{

    private AudioSource _audioSource;
    [SerializeField]
    private AudioClip _clip;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        if(_audioSource == null )
        {
            Debug.Log("AudioSource is NULL on EndPoint");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            _audioSource.clip = _clip;
            _audioSource.Play();
            GameManager.Instance.ReceiveDamage();
            other.gameObject.SetActive(false);
        }
    }
}
