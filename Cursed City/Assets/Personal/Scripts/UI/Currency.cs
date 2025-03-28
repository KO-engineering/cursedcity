using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.IO;
using System.Linq;
public class Currency : Singleton<Currency>
{
    public int currencyAmount;
    public TMP_Text currencyText;

    public UnityEvent<int> OnCurrencyChange;

    void Start()
    {
        currencyAmount = PlayerPrefs.GetInt("Currency", 100000);
        OnCurrencyChange?.AddListener(OnChangeCallback);
    }

    [Button]
    public void ResetMoney()
    {
        PlayerPrefs.DeleteKey("Currency");
    }

    void OnChangeCallback(int value)
    {
        PlayerPrefs.SetInt("Currency", value);
    }

    void Update()
    {
        currencyText.text = currencyAmount.ToString();
    }
    public void TakeAway(int amount)
    {
        currencyAmount -= amount;

        OnCurrencyChange?.Invoke(currencyAmount);
    }
    public void Earn(int amount)
    {
        currencyAmount += amount;

        OnCurrencyChange?.Invoke(currencyAmount);
    }
}
