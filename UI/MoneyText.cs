using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyText : MonoBehaviour
{
    TextMeshProUGUI _text;
    private void Awake() => _text = GetComponent<TextMeshProUGUI>();
    private void Start()
    {
        Resource.Instance.MoneyChangeHandler += OnMoneyChange;
        _text.text = Resource.Instance.Money.ToString("F0");
    }
    private void OnDisable() => Resource.Instance.MoneyChangeHandler -= OnMoneyChange;
    bool OnMoneyChange(float value)
    {
        _text.text = "$" + value.ToString("F2");
        return true;
    }
}
