using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public enum Gender { Male, Female };
    public Gender gender;

    [HorizontalLine]
    public bool randomizeCharacter = true;
    public GameObject[] femaleModels;
    public GameObject[] maleModels;

    [HorizontalLine]
    //public string npcName;
    //public TMP_Text nameText;

    [HideInInspector] public bool finishGenerating;
    Camera cam;

    void Start()
    {   
        cam = Camera.main;

        if(randomizeCharacter) RandomizeCharacter();
    }

    void Update()
    {
        //UpdateNameText();
    }

    [Button("Randomize Character")]
    public void RandomizeCharacter()
    {
        gender = Random.Range(0,2) == 0? Gender.Male : Gender.Female;
        RandomizeModel();
        //GenerateNewName();
    }
    
    [Button("Randomize Model")]
    public void RandomizeModel()
    {
        GameObject newModel = 
            gender == Gender.Male? 
                maleModels[Random.Range(0, maleModels.Length)] :
                femaleModels[Random.Range(0, femaleModels.Length)];

        foreach (var models in maleModels.Concat(femaleModels))
            models.SetActive(false);
        newModel.SetActive(true);
    }

    //[Button("Generate New Name")]
    //public void GenerateNewName()
    //{
        //npcName = NPCManager.Instance.GenerateNewName(gender);

        //finishGenerating = true;
    //}
    
    // void UpdateNameText()
    // {
    //     nameText.text = npcName;

    //     Vector3 dirToCam = cam.transform.position - nameText.transform.position;

    //     Quaternion lookRotation = Quaternion.LookRotation(-dirToCam);

    //     nameText.transform.rotation = Quaternion.Slerp(nameText.transform.rotation, lookRotation, Time.deltaTime * 5f);
    // }
}
