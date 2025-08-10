using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private NavMeshAgent _agent;
    [SerializeField] private Transform[] _waypoints;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        if(_agent == null )
        {
            Debug.LogError("Agent is NULL on ENEMY AI");
        }

        _agent.SetDestination(_waypoints[1].position);
    }

    void Update()
    {
        
    }
}
