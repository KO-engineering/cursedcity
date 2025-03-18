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

    // Update is called once per frame
    void Update()
    {
        currencyText.text = currencyAmount.ToString();
    }
    public void TakeAway(int amount)
    {
        currencyAmount -= amount;
    }
    public void Earn(int amount)
    {
        currencyAmount += amount;
    }
}
