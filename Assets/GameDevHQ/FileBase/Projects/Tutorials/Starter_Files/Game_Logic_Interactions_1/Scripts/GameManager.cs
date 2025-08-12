using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.Log("Instance is null on Game Manager");
            }
            return _instance;
        }
    }

    private int _currentScore;
    private int _damageReceived;
    private bool _isGameOver;
    public static Action OnGameOver;

    public bool IsGameOver
    {
        get { return _isGameOver; }
    }
    [SerializeField]
    private int _maxDamage = 5;


    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _currentScore = 0;
        UIManager.Instance.UpdateDamageUI(_damageReceived, _maxDamage);
    }

    public int CurrentScore()
    {
        return _currentScore;
    }

    public void AddScore(int score)
    {
        _currentScore += score;
    }

    public void ReceiveDamage()
    {
        _damageReceived++;
        UIManager.Instance.UpdateDamageUI(_damageReceived, _maxDamage);
        if(_damageReceived >= _maxDamage)
        {
            Debug.Log("You lose");
            _isGameOver = true;
            OnGameOver?.Invoke();
        }
    }
}
