using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if(_instance == null)
            {
                Debug.Log("UIManager is NULL");
            }
            return _instance;
        }
    }

    [Header("UI Settings")]
    [SerializeField]
    private TextMeshProUGUI _currentAmmoText;
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _enemiesLeft;
    [SerializeField]
    private TextMeshProUGUI _damageReceived;
    [SerializeField]
    private GameObject _gameOverPanel;
    [SerializeField]
    private GameObject _winPanel;
    [SerializeField]
    private GameObject _crossHair;

    private void OnEnable()
    {
        GameManager.OnGameOver += OnGameOver;
        SpawnManager.OnWinGame += OnWin;
    }

    private void OnDisable()
    {
        GameManager.OnGameOver -= OnGameOver;
        SpawnManager.OnWinGame -= OnWin;
    }

    private void Awake()
    {
        _instance = this;
        UpdateScore();
    }
    
    public void UpdateAmmoCount(int currentAmmo)
    {
        _currentAmmoText.text = currentAmmo.ToString();
    }

    public void UpdateScore()
    {
        _scoreText.text = (GameManager.Instance.CurrentScore()).ToString();
    }

    public void UpdateEnemiesLeft(int enemiesLeft)
    {
        _enemiesLeft.text = enemiesLeft.ToString();
    }

    public void UpdateDamageUI(int currentDamage, int maxDamage)
    {
        _damageReceived.text = currentDamage.ToString() + " / " + maxDamage.ToString();
    }

    private void OnGameOver()
    {
        _gameOverPanel.SetActive(true);
        _crossHair.SetActive(false);
    }

    private void OnWin()
    {
        _winPanel.SetActive(true);
        _crossHair.SetActive(false);
    }

}
