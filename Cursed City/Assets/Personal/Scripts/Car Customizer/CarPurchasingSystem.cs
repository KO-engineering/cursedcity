using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
        for(int i = 0; i < carUI.Count; i++){
            SetOpacity(85f / 255f, carUI[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PurchaseCar(){
        int carPrice;
        if (!int.TryParse(carCustomizer.cars[index].price, out carPrice))
        {
            UnityEngine.Debug.Log(" " + carPrice);
            return;
        }
        if(currency.currencyAmount > 0) {
            currency.TakeAway(carPrice);
            SetOpacity(1f, carUI[index]);
        }
        carCustomizer.cars[index].isOwned = true;
    }
    public void SetCarIndex(int indexX){
        index = indexX;
    }
    public void SetOpacity(float opacity, RawImage image){

        Color color = image.color;
        color.a = Mathf.Clamp01(opacity); 
        image.color = color;
    }
}
