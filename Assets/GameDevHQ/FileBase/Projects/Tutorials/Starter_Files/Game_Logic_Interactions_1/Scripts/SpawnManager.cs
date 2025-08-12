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
    [SerializeField] private int _enemiesToSpawn = 5;
    private int _enemiesLeft;

    [Header("Enemy")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private List<Transform> _hidingWaypoints;

    private void OnEnable()
    {
        GameManager.OnGameOver += OnGameOver;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= OnGameOver;
    }

    private void Awake()
    {
        _instance = this;       
    }

    void Start()
    {
        _enemiesLeft = _enemiesToSpawn;
        UIManager.Instance.UpdateEnemiesLeft(_enemiesLeft);
        _spawnEnemyRoutine = StartCoroutine(SpawnEnemiesRoutine());
    }

    IEnumerator SpawnEnemiesRoutine()
    {
        while (!GameManager.Instance.IsGameOver)
        {
            for (int i = 0; i < _enemiesToSpawn; i++)
            {
                if(GameManager.Instance.IsGameOver)
                {
                    yield break;
                }

                PoolManager.Instance.RequestEnemy();   
                yield return new WaitForSeconds(Random.Range(2, 5f));
            
            }
        }
        Debug.Log("End Spawn");
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

    public void ReduceEnemyCount()
    {
        _enemiesLeft--;
        UIManager.Instance.UpdateEnemiesLeft(_enemiesLeft);
        if(_enemiesLeft <= 0)
        {
            Debug.Log("You Win!");
        }
    }

    private void OnGameOver()
    {
        if(_spawnEnemyRoutine != null)
        {
            Debug.Log("Stop Spawning");
            StopCoroutine(_spawnEnemyRoutine);
            _spawnEnemyRoutine = null;
        }
    }
}
