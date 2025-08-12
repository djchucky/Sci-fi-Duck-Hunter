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
    private Health _health;

    private int _currentPoint = 0;
    private bool _isHiding;
    private bool _isDead;
    private float _velocity;
    private Vector3 _lastPosition;
    private Coroutine _hidingRoutine;
    private Coroutine _deathRoutine;

    [SerializeField] private int _points = 20;
    [SerializeField] private AIState _aiState;
    [SerializeField] private List<Transform> _hidingWaypoints;

    private void OnEnable()
    {
        Initialization();
    }

    private void OnDisable()
    {
        _health.OnDead -= OnDead;
    }

    private void Initialization()
    {
        _health = GetComponent<Health>();
        if(_health != null )
        {
            _health.OnDead += OnDead;
        }
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

        _agent.Warp(transform.position); //Set position for NavMeshAgent
        _agent.SetDestination(_hidingWaypoints[_currentPoint].position);

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
        if (_isDead || _aiState == AIState.Hide) return;

        // Se il nemico ha raggiunto la destinazione attuale
        if (_agent.remainingDistance < 0.5f && !_agent.pathPending)
        {
            // Cerca il prossimo waypoint libero
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

            // Se non ci sono waypoint liberi → scappa
            if (_currentPoint >= _hidingWaypoints.Count)
            {
                SetEnemyDestination();
                _aiState = AIState.Run;
                return;
            }

            //Controlla il waypoint target
            Waypoint targetWaypoint = _hidingWaypoints[_currentPoint].GetComponent<Waypoint>();

            if (_aiState != AIState.Hide && targetWaypoint != null && !targetWaypoint.IsOccupied && _agent.remainingDistance < 0.5f)
            {
                _aiState = AIState.Hide;
                return;
            }

            _agent.SetDestination(_hidingWaypoints[_currentPoint].position);

            if (targetWaypoint == null)
            {
                _aiState = AIState.Run;
            }
        }
    }

    private IEnumerator HidingRoutine()
    {
        _isHiding = true;
        _agent.isStopped = true;

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

    private void OnDead()
    {
        Death();
    }
    private void Death()
    {
        _deathRoutine = StartCoroutine(DeathSequence());   
    }

    IEnumerator DeathSequence()
    {
        GameManager.Instance.AddScore(_points);
        UIManager.Instance.UpdateScore();
        SpawnManager.Instance.ReduceEnemyCount();
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