using UnityEngine;
using UnityEngine.SceneManagement;

public class CarLoaderManager : Singleton<CarLoaderManager>
{
    public GameObject carPrefab;
    public Material carMaterial;

    public void SetCar(GameObject prefab, Material carMaterial)
    {
        this.carPrefab = prefab;
        this.carMaterial = carMaterial;
    }

    void Start()
    {
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
            if (carRenderer != null && carMaterial != null)
            {
                carRenderer.material = carMaterial;
            }
        }
    }
}