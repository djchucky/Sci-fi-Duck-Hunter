using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    private NavMeshAgent _agent;

    private void OnEnable()
    {
        transform.position = PoolManager.Instance.OnActive().position;

        Invoke("SetEnemyDestination", 0.5f);     
    }

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        if(_agent == null )
        {
            Debug.LogError("Agent is NULL on ENEMY AI");
        }        
    }

    private void SetEnemyDestination()
    {
        _agent.SetDestination(SpawnManager.Instance.SetEndPosition().position);
    }

}
