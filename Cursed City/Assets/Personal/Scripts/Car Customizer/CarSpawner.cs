using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> carPrefabs;
    [SerializeField] private List<List<Material>> carMaterials;
    [SerializeField] private Transform spawnPoint;
    
    private void Start()
    {
        // Load saved data
        CarDataManager.LoadFromPlayerPrefs();
        
        // Spawn the selected car
        SpawnSelectedCar();
    }
    
    private void SpawnSelectedCar()
    {
        int carIndex = CarDataManager.SelectedCarIndex;
        int materialIndex = CarDataManager.SelectedMaterialIndex;
        
        // Check if we have a valid car selection
        if (carIndex >= 0 && carIndex < carPrefabs.Count)
        {
            // Instantiate the car at the spawn point
            GameObject car = Instantiate(carPrefabs[carIndex], spawnPoint.position, spawnPoint.rotation);
            
            // Apply the selected material if valid
            if (materialIndex >= 0 && carIndex < carMaterials.Count && materialIndex < carMaterials[carIndex].Count)
            {
                Renderer carRenderer = car.GetComponent<Renderer>();
                if (carRenderer != null)
                {
                    carRenderer.material = carMaterials[carIndex][materialIndex];
                }
            }
            
            // You can add additional car setup here
            // For example, car controller components, etc.
        }
        else
        {
            Debug.LogWarning("No car selected or invalid car index! Using default car if available.");
            
            // Optional: Spawn a default car if no selection is made
            if (carPrefabs.Count > 0)
            {
                Instantiate(carPrefabs[0], spawnPoint.position, spawnPoint.rotation);
            }
        }
    }
}