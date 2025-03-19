using System.Collections.Generic;
using UnityEngine;

public class CarCustomizer : Singleton<CarCustomizer>
{
    [System.Serializable]
    public class CarCustomization
    {
        public string title;
        public string description;
        public string price;
        public GameObject carPrefab;  
        public List<Material> materials; 
        public List<GameObject> carUI;  
        public bool isOwned = false;
    }

    [SerializeField] public List<CarCustomization> cars; 
    [SerializeField] public GameObject tooltipPanel;
    [SerializeField] private TMPro.TextMeshProUGUI tooltipTitle;
    [SerializeField] private TMPro.TextMeshProUGUI tooltipDescription;
    [SerializeField] private TMPro.TextMeshProUGUI tooltipPrice;    
    public CarPurchasingSystem purchasingSystem;
    private GameObject activeCar;
    public void Start(){
        purchasingSystem = CarPurchasingSystem.Instance;
        HideTooltip();
    }

    public void SetColor(int carNum, int matNum)
    {
        if (carNum < 0 || carNum >= cars.Count)
        {
            Debug.LogError("Invalid car index!");
            return;
        }

        CarCustomization selectedCar = cars[carNum];

        if (matNum < 0 || matNum >= selectedCar.materials.Count)
        {
            Debug.LogError("Invalid material index!");
            return;
        }

        // Disable all cars and their UI elements
        foreach (var car in cars)
        {
            car.carPrefab.SetActive(false);
            foreach (var uiElement in car.carUI)
            {
                if (uiElement != null) uiElement.SetActive(false);
            }
        }

        // Enable only the selected car and its UI elements
        selectedCar.carPrefab.SetActive(true);
        foreach (var uiElement in selectedCar.carUI)
        {
            if (uiElement != null) uiElement.SetActive(true);
        }

        // Apply material to the selected car
        Renderer carRenderer = selectedCar.carPrefab.GetComponent<Renderer>();
        if (carRenderer != null)
        {
            carRenderer.material = selectedCar.materials[matNum];
        }
        else
        {
            Debug.LogError("Selected car does not have a Renderer component!");
        }
        activeCar = selectedCar.carPrefab;
        EnableRotationForSelectedCar();
    }
    public void SetCarColorFromButton(string parameters)
    {
        string[] values = parameters.Split(',');
        if (values.Length == 2 && int.TryParse(values[0], out int carNum) && int.TryParse(values[1], out int matNum))
        {
            SetColor(carNum, matNum);
        }
    }
    private void EnableRotationForSelectedCar()
    {
        foreach (var car in cars)
        {
            CarRotationController rotationController = car.carPrefab.GetComponent<CarRotationController>();
            if (rotationController != null)
            {
                rotationController.enabled = (car.carPrefab == activeCar);
            }
        }
    }
public void ShowTooltip(int carNum)
{
    purchasingSystem.SetCarIndex(carNum);
    // Toggle the tooltip on and off when the button is clicked
    if (tooltipPanel.activeSelf)
    {
        HideTooltip();
    }
    else
    {
        if (carNum >= 0 && carNum < cars.Count)
        {
            tooltipTitle.text = cars[carNum].title;
            tooltipDescription.text = cars[carNum].description;
            tooltipPrice.text = cars[carNum].price;
            tooltipPanel.SetActive(true);
            
        }
        else
        {
            Debug.LogError("Invalid car index for tooltip!");
        }
    }
}

    public void HideTooltip()
    {
        tooltipPanel.SetActive(false);
    }
}