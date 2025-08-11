using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private enum AIState
    {
        Run,
        Hide,
        Death
    }

    private NavMeshAgent _agent;
    private AIAnimation _animator;

    private int _currentPoint = 0;
    private bool _isHiding;
    private bool _isDead;
    private float _velocity;
    private Vector3 _lastPosition;
    private Coroutine _hidingRoutine;
    private Coroutine _deathRoutine;

    [SerializeField] private AIState _aiState;
    [SerializeField] private List<Transform> _hidingWaypoints;

    private void OnEnable()
    {
        Initialization();
    }

    private void Initialization()
    {
        _isDead = false;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<AIAnimation>();
        _agent.enabled = true;
        GetComponent<CapsuleCollider>().enabled = true;
        _aiState = AIState.Run;
        _currentPoint = 0;

        transform.position = PoolManager.Instance.OnActive().position;
        _lastPosition = transform.position;
        _hidingWaypoints = SpawnManager.Instance.GetHidingWaypoints();

        _agent.SetDestination(_hidingWaypoints[_currentPoint].position);
        _agent.Warp(transform.position); //Set position for NavMeshAgent

        if (_agent == null)
        {
            Debug.LogError("Agent is NULL on ENEMY AI");
        }
    }

    private void Update()
    {
        
        HandleAnimations();
        HandleStatus();
    }

    private void FixedUpdate()
    {
        CalculateVelocity();
    }


    private void HandleAnimations()
    {
        _animator.Run(_velocity);
        _animator.HidingAnimation(_isHiding);
    }

    private void CalculateVelocity()
    {
        _velocity = Mathf.Lerp(_velocity, (transform.position - _lastPosition).magnitude / Time.deltaTime, 0.75f);
        _lastPosition = transform.position;
    }

    private void HandleStatus()
    {
        switch (_aiState)
        {
            case AIState.Run:
                CalculateMovement();
                break;

            case AIState.Hide:
                if (_isHiding == false && _hidingRoutine == null)
                {
                    _hidingRoutine = StartCoroutine(HidingRoutine());
                }
                break;

            case AIState.Death:
                if(!_isDead && _deathRoutine == null)
                {
                    Death();
                }
                break;
        }
    }

    private void SetEnemyDestination()
    {
        _agent.SetDestination(SpawnManager.Instance.SetEndPosition().position);
    }

    private void CalculateMovement()
    {
        if (_isDead) return;

        if (_agent.remainingDistance < 0.3f && !_agent.pathPending)
        {
            while (_currentPoint < _hidingWaypoints.Count)
            {
                Waypoint waypoint = _hidingWaypoints[_currentPoint].GetComponent<Waypoint>();

                if (waypoint != null && waypoint.IsOccupied)
                {
                    _currentPoint++;
                }
                else
                {
                    break;
                }
            }

            if (_currentPoint >= _hidingWaypoints.Count)
            {
                SetEnemyDestination();
                _aiState = AIState.Run;
                return;
            }

            _agent.SetDestination(_hidingWaypoints[_currentPoint].position);

            Waypoint targetWaypoint = _hidingWaypoints[_currentPoint].GetComponent<Waypoint>();
            if (targetWaypoint != null && !targetWaypoint.IsOccupied)
            {
                _aiState = AIState.Hide;
            }
            else
            {
                _aiState = AIState.Run;
            }
        }
    }

    private IEnumerator HidingRoutine()
    {
        _isHiding = true;
        _agent.isStopped = true;

        Debug.Log("Is hiding is "+  _isHiding);
        yield return new WaitForSeconds(Random.Range(1.5f, 4f));

        _isHiding = false;
        _agent.isStopped = false;

        // Reset destinazione attuale per forzare il NavMeshAgent a calcolare il percorso
        if (_currentPoint < _hidingWaypoints.Count)
        {
            _agent.SetDestination(_hidingWaypoints[_currentPoint].position);
        }
        else
        {
            SetEnemyDestination();
        }

        _aiState = AIState.Run;
        _hidingRoutine = null;
    }

    private void Death()
    {
        _deathRoutine = StartCoroutine(DeathSequence());   
    }

    IEnumerator DeathSequence()
    {
        _isDead = true;
        _agent.isStopped = true;
        _aiState = AIState.Death;
        _animator.DeathAnimation();
        _agent.enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(4.5f);
        gameObject.SetActive(false);
        _deathRoutine = null;
    }
}