using System.Collections.Generic;
using UnityEngine;

public class CarCustomizer : MonoBehaviour
{
    [System.Serializable]
    public class CarCustomization
    {
        public GameObject carPrefab;  // The car model
        public List<Material> materials; // List of materials available for this car
        public List<GameObject> carUI;  // List of UI elements for this car
    }

    [SerializeField] private List<CarCustomization> cars; // List of cars with their materials and UI

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
    }
    public void SetCarColorFromButton(string parameters)
    {
        string[] values = parameters.Split(',');
        if (values.Length == 2 && int.TryParse(values[0], out int carNum) && int.TryParse(values[1], out int matNum))
        {
            SetColor(carNum, matNum);
        }
    }
}