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


    private void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        _currentScore = 0;
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
        Debug.Log($"Current Damage: {_damageReceived}");
        if(_damageReceived >= 10)
        {
            Debug.Log("You lose");
        }
    }
}
