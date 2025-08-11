using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAnimation : MonoBehaviour
{
    private Animator _animator;
    private string _speed = "Speed";
    private string _hide = "Hiding";
    private string _death = "Death";

    private void OnEnable()
    {
        _animator = GetComponent<Animator>();
        if(_animator == null)
        {
            Debug.LogError("Animator is NULL");
        }
       
    }

    public void Run(float speed)
    {
        _animator.SetFloat(_speed, speed);
    }

    public void HidingAnimation(bool isHiding)
    {
        _animator.SetBool(_hide, isHiding);
    }

    public void DeathAnimation()
    {
        _animator.SetTrigger(_death);
    }
}
