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

    [SerializeField]
    private TextMeshProUGUI _currentAmmoText;
    [SerializeField]
    private TextMeshProUGUI _scoreText;
    [SerializeField]
    private TextMeshProUGUI _enemiesLeft;

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

}
