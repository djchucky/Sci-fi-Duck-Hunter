using System.Collections;
using System.Collections.Generic;
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

    private int _currentPoint = 0;
    private bool _isHiding;
    private Coroutine _hidingRoutine;

    [SerializeField] private AIState _aiState;
    [SerializeField] private List<Transform> _hidingWaypoints;

    private void OnEnable()
    {
        _agent = GetComponent<NavMeshAgent>();
        if (_agent == null)
        {
            Debug.LogError("Agent is NULL on ENEMY AI");
        }

        _currentPoint = 0;
        transform.position = PoolManager.Instance.OnActive().position;
        _hidingWaypoints = SpawnManager.Instance.GetHidingWaypoints();
        _agent.SetDestination(_hidingWaypoints[_currentPoint].position);
    }

    private void Update()
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
                Death();
                break;
        }
    }

    private void SetEnemyDestination()
    {
        _agent.SetDestination(SpawnManager.Instance.SetEndPosition().position);
    }

    private void CalculateMovement()
    {
        if (_agent.remainingDistance < 0.5f && !_agent.pathPending)
        {
            _currentPoint++;

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

    IEnumerator HidingRoutine()
    {
        _isHiding = true;
        _agent.isStopped = true;
        yield return new WaitForSeconds(Random.Range(0.5f, 4f));
        _isHiding = false;
        _agent.isStopped = false;
        _aiState = AIState.Run;
        _hidingRoutine = null;
    }

    public void Death()
    {
        Debug.Log("Score + 50");
        Debug.Log("Animation death");
        Debug.Log("Stop moving - disable navmesh");
    }

}