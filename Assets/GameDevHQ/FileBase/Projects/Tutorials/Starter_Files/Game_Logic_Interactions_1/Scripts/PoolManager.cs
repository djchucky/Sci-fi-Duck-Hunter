using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private static PoolManager _instance;
    public static PoolManager Instance
    { get
        {
            if (_instance == null)
            {
                Debug.Log("Instance is NULL on Pool Manager");
            }
            return _instance;
        }
    }

    private int _startingEnemies = 10;
    [Header("PoolManager Settings")]
    [SerializeField] private GameObject _enemyPrefab;
    [SerializeField] private Transform _spawnPos;
    [SerializeField] private GameObject _container;
    [SerializeField] private List<GameObject> _enemiesPool;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        //Generate Enemies at the beginning

        GenerateEnemies(_startingEnemies);        
    }

    public List<GameObject> GenerateEnemies(int amountOfEnemies)
    {
        //Generate Amount of Enemies and add to the Pool List

        for (int i = 0; i < amountOfEnemies; i++)
        {
            GameObject enemy = Instantiate(_enemyPrefab,_spawnPos.position,Quaternion.identity);
            enemy.transform.parent = _container.transform;
            enemy.SetActive(false);
            _enemiesPool.Add(enemy);
        }
        
        return null;     
    }

    public GameObject RequestEnemy()
    {
        //Loop through all list
        //checking for in-active Enemies
        //found one? Set it active and return to player
        

        for (int i = 0; i < _enemiesPool.Count; i++)
        {
            if (_enemiesPool[i].activeInHierarchy == false)
            {
                _enemiesPool[i].SetActive(true);
                return _enemiesPool[i];
            }    
        }

        //not found? (All are active) 
        //Generate x amount of Enemies and run the RequestEnemy method

        GameObject newEnemy = Instantiate(_enemyPrefab,_spawnPos.position,Quaternion.identity);
        newEnemy.transform.parent = _container.transform;
        _enemiesPool.Add(newEnemy);

        return newEnemy;
    }

    public Transform OnActive()
    {
        return _spawnPos;
    }


}
