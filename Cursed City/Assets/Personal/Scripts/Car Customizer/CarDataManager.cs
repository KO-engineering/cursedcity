using System.Collections.Generic;
using UnityEngine;

public static class CarDataManager
{
    public static int SelectedCarIndex { get; set; } = -1;
    public static int SelectedMaterialIndex { get; set; } = 0;
    public static List<int> OwnedCars { get; private set; } = new List<int>();

    public static void SaveCarOwnership(int carIndex)
    {
        if (!OwnedCars.Contains(carIndex))
        {
            OwnedCars.Add(carIndex);
        }
        
        SaveToPlayerPrefs();
    }

    public static void SelectCar(int carIndex, int materialIndex)
    {
        SelectedCarIndex = carIndex;
        SelectedMaterialIndex = materialIndex;
        
        SaveToPlayerPrefs();
    }

    public static bool IsCarOwned(int carIndex)
    {
        return OwnedCars.Contains(carIndex);
    }

    private static void SaveToPlayerPrefs()
    {
        PlayerPrefs.SetInt("SelectedCarIndex", SelectedCarIndex);
        PlayerPrefs.SetInt("SelectedMaterialIndex", SelectedMaterialIndex);
        
        string ownedCarsData = string.Join(",", OwnedCars);
        PlayerPrefs.SetString("OwnedCars", ownedCarsData);
        
        PlayerPrefs.Save();
    }

    public static void LoadFromPlayerPrefs()
    {
        SelectedCarIndex = PlayerPrefs.GetInt("SelectedCarIndex", -1);
        SelectedMaterialIndex = PlayerPrefs.GetInt("SelectedMaterialIndex", 0);
        
        string ownedCarsData = PlayerPrefs.GetString("OwnedCars", "");
        OwnedCars.Clear();
        
        if (!string.IsNullOrEmpty(ownedCarsData))
        {
            string[] carIndices = ownedCarsData.Split(',');
            foreach (string indexStr in carIndices)
            {
                if (int.TryParse(indexStr, out int index))
                {
                    OwnedCars.Add(index);
                }
            }
        }
    }
}