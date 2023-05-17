using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public float Money { get { return currentMoney; } }
    public event Func<float, bool> MoneyChangeHandler;
    float currentMoney;
    public static Resource Instance;
    void Awake()
    {
        InitSingleton();
        Init();
    }
    private void InitSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private void Init()
    {
        if (!PlayerPrefs.HasKey("Money"))
        {
            PlayerPrefs.SetInt("Money", References.Instance.GameConfig.StartMoney);
            currentMoney = PlayerPrefs.GetInt("Money");
        }
        else
            currentMoney = PlayerPrefs.GetInt("Money");
    }

    public void ZeroMoney()
    {
        PlayerPrefs.SetInt("Money", 0);
        currentMoney = 0;
        MoneyChangeHandler?.Invoke(currentMoney);
    }
    public void OnMoneyChange(float change)
    {
        currentMoney += change;
        PlayerPrefs.SetInt("Money", (int)currentMoney);
        MoneyChangeHandler?.Invoke(currentMoney);
    }
}
