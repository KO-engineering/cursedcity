using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;


public class CarPurchasingSystem : Singleton<CarPurchasingSystem>
{
    private CarCustomizer carCustomizer;
    private Currency currency;
    public List<RawImage> carUI;
    int index = 0;

    void Start()
    {
        carCustomizer = CarCustomizer.Instance;
        currency = Currency.Instance;
        for (int i = 0; i < carUI.Count; i++)
        {
            SetOpacity(85f / 255f, carUI[i]);
        }

        // Load owned cars from player prefs
        string ownedCars = PlayerPrefs.GetString("OwnedCars", "");
        if (ownedCars != "")
        {
            string[] ownedCarIndexes = ownedCars.Split(',');
            foreach (string ownedCarId in ownedCarIndexes)
            {
                int index;
                if (int.TryParse(ownedCarId, out index))
                {
                    print("Owned true: " + carCustomizer.cars[index].title);
                    carCustomizer.cars[index].isOwned = true;
                    SetOpacity(1f, carUI[index]);
                }
            }
        }
    }

    [Button]
    public void ResetOwnedCars()
    {
        for (int i = 0; i < carUI.Count; i++)
        {
            carCustomizer.cars[i].isOwned = false;
            SetOpacity(85f / 255f, carUI[i]);
        }
        PlayerPrefs.DeleteKey("OwnedCars");
    }


    public void PurchaseCar()
    {
        int carPrice;
        if (!int.TryParse(carCustomizer.cars[index].price, out carPrice))
        {
            UnityEngine.Debug.Log(" " + carPrice);
            return;
        }
        if (currency.currencyAmount - carPrice >= 0)
        {
            currency.TakeAway(carPrice);
            SetOpacity(1f, carUI[index]);
        }

        carCustomizer.cars[index].isOwned = true;

        // Save owned cars to player prefs
        string ownedCars = PlayerPrefs.GetString("OwnedCars", "");

        if(ownedCars != "")
            PlayerPrefs.SetString("OwnedCars", ownedCars + ", " + index);
        else
            PlayerPrefs.SetString("OwnedCars", index.ToString());
    }


    public void SetCarIndex(int indexX)
    {
        index = indexX;
    }
    public void SetOpacity(float opacity, RawImage image)
    {

        Color color = image.color;
        color.a = Mathf.Clamp01(opacity);
        image.color = color;
    }

}
