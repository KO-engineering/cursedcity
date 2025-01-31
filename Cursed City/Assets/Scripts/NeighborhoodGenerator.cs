using UnityEngine;

public class NeighborhoodGenerator : MonoBehaviour
{
    // Public fields for prefabs
    public GameObject housePrefab;     // House object
    public GameObject sidewalkPrefab; // Sidewalk object
    public GameObject streetPrefab;   // Street object
    public Transform ground;          // Reference to the ground/terrain

    // Neighborhood settings
    public int rows = 6;             // Number of house rows
    public int columns = 6;          // Number of house columns
    public float houseSpacing = 10f; // Distance between house centers
    public float streetWidth = 5f;   // Width of the street

    private float groundHeight;      // Ground level (y-axis)

    void Start()
    {
        // Set the groundHeight based on the ground object
        if (ground != null)
        {
            groundHeight = ground.position.y;
        }
        else
        {
            Debug.LogWarning("Ground reference not set. Defaulting to y = 0.");
            groundHeight = 0f; // Default to y = 0 if no ground is set
        }

        GenerateNeighborhood();
    }

    void GenerateNeighborhood()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int z = 0; z < rows; z++)
            {
                // Calculate the position for each house
                Vector3 housePosition = new Vector3(x * houseSpacing, groundHeight, z * houseSpacing);

                // Instantiate the house
                GenerateHouse(housePosition);

                // Add sidewalks
                GenerateSidewalk(housePosition);

                // Add streets
                if (x < columns - 1) GenerateStreet(housePosition, Vector3.right);
                if (z < rows - 1) GenerateStreet(housePosition, Vector3.forward);
            }
        }
    }

    void GenerateHouse(Vector3 position)
    {
        Instantiate(housePrefab, position, Quaternion.identity);
    }

    void GenerateSidewalk(Vector3 housePosition)
    {
        // Generate a sidewalk in front of the house
        Vector3 sidewalkPosition = housePosition + new Vector3(0, 0, -houseSpacing / 2);
        Instantiate(sidewalkPrefab, sidewalkPosition, Quaternion.identity);
    }

    void GenerateStreet(Vector3 startPosition, Vector3 direction)
    {
        // Generate a street in the specified direction
        Vector3 streetPosition = startPosition + (direction * (houseSpacing / 2 + streetWidth / 2));
        streetPosition.y = groundHeight; // Ensure street aligns with ground height
        Instantiate(streetPrefab, streetPosition, Quaternion.LookRotation(direction));
    }
}