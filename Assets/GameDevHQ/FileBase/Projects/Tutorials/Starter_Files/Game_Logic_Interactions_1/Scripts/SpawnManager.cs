using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    private static SpawnManager _instance;
    public static SpawnManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.Log("Instance is null on Spawn Manager");
            }
            return _instance;
        }
    }

    private Coroutine _spawnEnemyRoutine;
    [SerializeField] private Transform _endPosition;

    [Header("Enemy")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private List<Transform> _hidingWaypoints;

    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
      _spawnEnemyRoutine = StartCoroutine(SpawnEnemiesRoutine());
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        yield return null;
        while(true)
        {
            PoolManager.Instance.RequestEnemy();   
            yield return new WaitForSeconds(Random.Range(2, 5f));
        }
       
        _spawnEnemyRoutine = null;
    }

    public Transform SetEndPosition()
    {
        return _endPosition;
    }

    public List<Transform> GetHidingWaypoints()
    {
        return _hidingWaypoints;
    }
}
