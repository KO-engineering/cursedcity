using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityGeneration : MonoBehaviour
{
    [SerializeField] int size = 20;
    [SerializeField] int spacing = 3;
    [SerializeField] Vector2Int dimensions;
    [SerializeField] Transform normalRoad;
    [SerializeField] Transform tSectionRoad;
    [SerializeField] Transform fourSectionRoad;
    [SerializeField] Transform cornerRoad;
    [SerializeField] Transform pavement;
    [SerializeField] Transform building;

    void Start()
    {
        GenerateCity();
    }

    void GenerateCity()
    {
        GenerateRoad();
    }

    void GenerateRoad()
    {
        for (int z = 0; z < dimensions.y; z++)
        {
            for (int x = 0; x < dimensions.x; x++)
            {
                Vector3 position = new Vector3(x * size, 0, z * size);
                Quaternion rotation = Quaternion.Euler(Vector3.zero);

                bool isBottomRow = z == 0;
                bool isTopRow = z == dimensions.y - 1;
                bool isLeftEdge = x == 0;
                bool isRightEdge = x == dimensions.x - 1;
                bool isBottomLeft = z == 0 && x == 0;
                bool isBottomRight = z == 0 && x == dimensions.x - 1;
                bool isTopLeft = z == dimensions.y - 1 && x == 0;
                bool isTopRight = z == dimensions.y - 1 && x == dimensions.x - 1;
                bool isMiddleTop = z == dimensions.y - spacing - 2;
                bool isMiddleBottom = z == 0 + spacing + 1;

                bool areEdges = z == 0 || z == dimensions.y - 1 || x == 0 || x == dimensions.x - 1;
                bool allowedRoads = z % (spacing + 1) == 0 || x % (spacing + 1) == 0;
                bool allowedRoadsBetween = z % (spacing + 1) != 0 || x % (spacing + 1) != 0;

                bool areCorners = (z == 0 && x == 0) || (z == dimensions.y - 1 && x == dimensions.x - 1) || (z == dimensions.y - 1 && x == 0) || (z == 0 && x == dimensions.x - 1);
                bool areCornerTSections = areEdges && allowedRoads && !areCorners && !allowedRoadsBetween;
                bool areFourIntersections = !areEdges && allowedRoads && !allowedRoadsBetween;

                if (areEdges || allowedRoads)
                {   if(isTopRow || isBottomRow && !areCorners || isMiddleTop || isMiddleBottom && !areCornerTSections){
                        rotation = Quaternion.Euler(0, 90, 0);
                    }
                    Transform roadToSpawn = normalRoad;

                    if(areCorners)
                    {
                        roadToSpawn = cornerRoad;
                        if(isBottomLeft){
                            rotation = Quaternion.Euler(0, -90, 0);
                        }
                        else if(isTopRight){
                            rotation = Quaternion.Euler(0, 90, 0);
                        }
                        else if(isTopLeft){
                            rotation = Quaternion.Euler(0, 0, 0);
                        }
                        else if(isBottomRight){
                            rotation = Quaternion.Euler(0, 180, 0);
                        }
                    }
        
                    if(areCornerTSections)
                        roadToSpawn = tSectionRoad;
                        if(isRightEdge && !areCorners){
                            rotation = Quaternion.Euler(0, 180, 0);
                        }
                        if(isBottomRow && !areCorners){
                            rotation = Quaternion.Euler(0, -90, 0);
                        }
                        if(isLeftEdge && !areCorners){
                            rotation = Quaternion.Euler(0, 0, 0);
                        }

                    if(areFourIntersections)
                        roadToSpawn = fourSectionRoad;

                    Instantiate(roadToSpawn, position, rotation);
                }
                else
                {
                    Instantiate(pavement, position, Quaternion.identity);
                    //buildings
                    Instantiate(building, position, Quaternion.identity);
                }


            }
        }
    }
}
