using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCustomizerSaveSystem : MonoBehaviour
{
    // Reference to the player character and hat game objects in the main game scene
    public GameObject[] characterSkins;  // Drag all skin game objects here
    public GameObject[] hats;            // Drag all hat game objects here

    void Start()
    {
        // Load the saved customization data from PlayerPrefs
        int selectedSkinIndex = PlayerPrefs.GetInt("SelectedSkinIndex", 0);
        int selectedHatIndex = PlayerPrefs.GetInt("SelectedHatIndex", 0);
        int selectedSkinColorIndex = PlayerPrefs.GetInt("SelectedSkinColorIndex", 0);

        // Apply the saved skin and hat selection
        ApplyCustomization(selectedSkinIndex, selectedHatIndex, selectedSkinColorIndex);
    }

    // Apply the customization settings
    void ApplyCustomization(int skinIndex, int hatIndex, int skinColorIndex)
    {
        // Disable all character skins
        foreach (var skin in characterSkins)
        {
            skin.SetActive(false);
        }

        // Enable the selected skin
        if (skinIndex >= 0 && skinIndex < characterSkins.Length)
        {
            characterSkins[skinIndex].SetActive(true);

            // Optionally, apply the skin color here if applicable
            // For simplicity, we are assuming the skins are pre-colored or handled through materials
        }

        // Disable all hats
        foreach (var hat in hats)
        {
            hat.SetActive(false);
        }

        // Enable the selected hat
        if (hatIndex >= 0 && hatIndex < hats.Length)
        {
            hats[hatIndex].SetActive(true);
        }
    }
}