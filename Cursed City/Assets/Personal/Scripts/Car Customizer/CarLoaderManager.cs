using UnityEngine;
using UnityEngine.SceneManagement;

public class CarLoaderManager : MonoBehaviour
{
    [SerializeField] private CarCustomizer.CarCustomization[] availableCars;
    
    void Awake()
    {
        // Ensure this object persists across scene loads
        DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        // Subscribe to scene loaded event
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        // Unsubscribe from scene loaded event to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if we're in the main game scene
        if (scene.name == "Main Game") 
        {
            SpawnSelectedCar();
        }
    }

    public void SpawnSelectedCar()
    {
        int selectedCarIndex = PlayerPrefs.GetInt("SelectedCarIndex", -1);
        int selectedMaterialIndex = PlayerPrefs.GetInt("SelectedMaterialIndex", 0);

        if (selectedCarIndex >= 0 && selectedCarIndex < availableCars.Length)
        {
            GameObject carPrefab = availableCars[selectedCarIndex].carPrefab;

            if (carPrefab != null)
            {
                // Find a spawn point in the scene (optional)
                GameObject spawnPoint = GameObject.FindGameObjectWithTag("CarSpawnPoint");
                Vector3 spawnPosition = spawnPoint ? spawnPoint.transform.position : Vector3.zero;
                Quaternion spawnRotation = spawnPoint ? spawnPoint.transform.rotation : Quaternion.identity;

                print("Spawning " + carPrefab.name);
                // Instantiate the car at the spawn point
                GameObject spawnedCar = Instantiate(carPrefab, spawnPosition, spawnRotation);

                // Apply the selected material
                Renderer carRenderer = spawnedCar.GetComponent<Renderer>();
                if (carRenderer != null && selectedMaterialIndex < availableCars[selectedCarIndex].materials.Count)
                {
                    carRenderer.material = availableCars[selectedCarIndex].materials[selectedMaterialIndex];
                }
            }
            else
            {
                Debug.LogError("Selected car prefab is null!");
            }
        }
        else
        {
            Debug.LogWarning("No car selected or invalid car index!");
        }
    }
}