using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NaughtyAttributes;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class NPCManager : Singleton<NPCManager>
{
    public List<NPCSpawnData> npcSpawnDatas = new List<NPCSpawnData>();
    public int npcSpawnCount;

    [HorizontalLine]
    public Transform defaultSpawn;
    public List<NavMeshSurface> navMeshSurfaces= new List<NavMeshSurface>();
    public Transform npcPathParent;
    [ReadOnly] public List<Transform> npcPaths = new List<Transform>();
    //public Transform npcDrunkPathParent;
    //[ReadOnly] public List<Transform> npcDrunkPaths = new List<Transform>();

    [HorizontalLine]

    [MinMaxSlider(0.5f, 4)] public Vector2 npcSpeedRange = new Vector2(1, 2);

    [HorizontalLine]

    [ReadOnly] public List<string> textLines = new List<string>();
   // string femaleFileName = "Female Names";
    //string maleFilename = "Male Names";
    //string fileName;

    void Start()
    {
        for (int i = 0; i < npcSpawnCount; i++)
        {
            NPC newNPC = Instantiate(npcSpawnDatas[Random.Range(0, npcSpawnDatas.Count)].npcPrefab).GetComponent<NPC>();
            TeleportToRandomNavMeshPoint(newNPC.GetComponent<NavMeshAgent>());

            Material[] newMaterials = new Material[newNPC.GetComponentInChildren<SkinnedMeshRenderer>().sharedMaterials.Length];
            NPCSpawnData ourNPCData = npcSpawnDatas[Random.Range(0, npcSpawnDatas.Count)];
            newMaterials[0] = ourNPCData.materials[Random.Range(0, ourNPCData.materials.Count)];
            
            newNPC.GetComponentsInChildren<SkinnedMeshRenderer>(true).ToList().ForEach(x => x.sharedMaterials = newMaterials);

            newNPC.RandomizeCharacter();
        }
    }

    [Button("Update NPC Paths")]
    public void UpdateNPCPaths()
    {
        npcPaths = npcPathParent.Cast<Transform>().ToList();
    }

    // public string GenerateNewName(NPC.Gender gender)
    // {
    //     fileName = gender == NPC.Gender.Male ? maleFilename : femaleFileName;
    //     string filePath = Path.Combine(Application.dataPath, "Game Assets/Data", fileName + ".txt");

    //     if (File.Exists(filePath))
    //     {
    //         textLines.Clear();
    //         textLines.AddRange(File.ReadAllLines(filePath));

    //         if (textLines.Count > 0)
    //         {
    //             return textLines[Random.Range(0, textLines.Count)];
    //         }
    //     }
    //     else Debug.LogError("File not found: " + filePath);
    //     return "";
    // }

    public void TeleportToRandomNavMeshPoint(NavMeshAgent agent)
    {
        agent.Warp(GetRandomPositionOnNavMesh());
    }

    public Vector3 GetRandomPositionOnNavMesh()
    {
        NavMeshSurface selectedSurface = navMeshSurfaces[Random.Range(0, navMeshSurfaces.Count)];
        Vector3 size = selectedSurface.size;

        Vector3 randomOffset = new Vector3(
            Random.Range(-size.x / 2f, size.x / 2f),
            Random.Range(-size.y / 2f, size.y / 2f),
            Random.Range(-size.z / 2f, size.z / 2f)
        );

        Vector3 randomPoint = selectedSurface.center + randomOffset + selectedSurface.transform.position;
        NavMeshHit hit;
        print($"Selected surface {selectedSurface.name} | {selectedSurface.center}");
        if (NavMesh.SamplePosition(randomPoint, out hit, 30000, NavMesh.AllAreas))
        {
            return hit.position;
        }
        
        return defaultSpawn.position;
    }

    [System.Serializable]
    public class NPCSpawnData
    {
        public Transform npcPrefab;
        public List<Material> materials = new List<Material>();
    }
}
