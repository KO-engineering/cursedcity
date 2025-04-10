using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerCustomizer : Singleton<PlayerCustomizer>
{
    [System.Serializable]
    public class PlayerCustomization{
        public enum Gender{
            Male, 
            Female,
        }
        public GameObject selectedCharSkin;
        public Gender selectedGender;
        public List<SkinColorMaterials> materials;

        [System.Serializable]
        public class SkinColorMaterials
        {
            public List<Material> skinColors; 
        }
    }
    [System.Serializable]
    public class PlayerHats{
        public GameObject hatGameobj;
        public Texture hatUI;
        public int hatIndex;
    }
    [SerializeField] public List<PlayerCustomization> skins; 
    public List<PlayerHats> hats;
    public RawImage hatImage;
    public int currentHatIndex = 0;
    public int currentSkinIndex = 0;
    public int currentSkinColorIndex = 0;

    void Start()
    {
        // Initially set a random skin
        SelectRandomSkin();
        UpdateHatUI();
    }

    void Update()
    {
        // Additional logic for continuous updates can go here if needed
    }
     public void OnSelectCharacter()
    {
        // Save selected skin and hat indexes to PlayerPrefs (or use another save method)
        PlayerPrefs.SetInt("SelectedSkinIndex", currentSkinIndex);
        PlayerPrefs.SetInt("SelectedHatIndex", currentHatIndex);
        PlayerPrefs.SetInt("SelectedSkinColorIndex", currentSkinColorIndex);
        PlayerPrefs.Save();

        // Switch to Main Game scene
        SceneManager.LoadScene("Main Game");
    }


    public void SelectGenderMale()
    {
        SelectSkinByGender(PlayerCustomization.Gender.Male);
    }

    public void SelectGenderFemale()
    {
        SelectSkinByGender(PlayerCustomization.Gender.Female);
    }

    // Select a random skin based on gender (no spawning, just activate the correct skin)
    public void SelectRandomSkin()
    {
        // Randomly select a gender to get a skin for
        PlayerCustomization.Gender gender = (Random.Range(0, 2) == 0) ? PlayerCustomization.Gender.Male : PlayerCustomization.Gender.Female;
        SelectSkinByGender(gender);
    }

    // Select a skin based on gender (no spawning, just activate the correct skin)
    public void SelectSkinByGender(PlayerCustomization.Gender gender)
    {
        // Disable all skins first
        foreach (var skin in skins)
        {
            skin.selectedCharSkin.SetActive(false);
        }

        // Find and activate the correct skin based on gender
        var filteredSkins = skins.FindAll(skin => skin.selectedGender == gender);
        if (filteredSkins.Count > 0)
        {
            PlayerCustomization selectedSkin = filteredSkins[Random.Range(0, filteredSkins.Count)];
            selectedSkin.selectedCharSkin.SetActive(true);
            currentSkinIndex = skins.IndexOf(selectedSkin);
            currentSkinColorIndex = 0; // Default to the first skin color
        }
    }

    // Change the hat when pressing left or right
    public void SelectHatRight()
    {
        currentHatIndex++;
        if (currentHatIndex >= hats.Count)
        {
            currentHatIndex = 0; // Loop back to the first hat
        }

        UpdateHatUI();
    }

    public void SelectHatLeft()
    {
        currentHatIndex--;
        if (currentHatIndex < 0)
        {
            currentHatIndex = hats.Count - 1; // Loop back to the last hat
        }

        UpdateHatUI();
    }

    void UpdateHatUI()
    {
        // Update the hat texture on the UI
        hatImage.texture = hats[currentHatIndex].hatUI;

        // Disable all hats first
        foreach (var hat in hats)
        {
            hat.hatGameobj.SetActive(false);
        }

        // Activate the current selected hat
        hats[currentHatIndex].hatGameobj.SetActive(true);
    }

    // Cycle through the skin colors
    public void CycleSkinColor(int skinColorIndex)
    {
        PlayerCustomization selectedSkin = skins[currentSkinIndex];

        if (skinColorIndex >= 0 && skinColorIndex < selectedSkin.materials[0].skinColors.Count)
        {
            // Apply the selected skin color to the player's character model
            Material selectedColorMaterial = selectedSkin.materials[0].skinColors[skinColorIndex];
            selectedSkin.selectedCharSkin.GetComponent<Renderer>().material = selectedColorMaterial;

            // Update the current skin color index
            currentSkinColorIndex = skinColorIndex;
        }
    }

    // Cycle through the skins list (using SetActive)
    public void CycleSkinRight()
    {
        currentSkinIndex++;
        if (currentSkinIndex >= skins.Count)
        {
            currentSkinIndex = 0; // Loop back to the first skin
        }

        UpdateSelectedSkin();
    }

    public void CycleSkinLeft()
    {
        currentSkinIndex--;
        if (currentSkinIndex < 0)
        {
            currentSkinIndex = skins.Count - 1; // Loop back to the last skin
        }

        UpdateSelectedSkin();
    }

    void UpdateSelectedSkin()
    {
        // Disable all skins first
        foreach (var skin in skins)
        {
            skin.selectedCharSkin.SetActive(false);
        }

        // Activate the selected skin
        skins[currentSkinIndex].selectedCharSkin.SetActive(true);
    }
}