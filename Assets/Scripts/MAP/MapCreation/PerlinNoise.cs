using UnityEngine;
using static UnityEngine.Experimental.Rendering.RayTracingAccelerationStructure;

public class PerlinNoise : IInitSystem
{


    private TerrainType[] terrainMap;


    public void Init(SystemManager systemManager)
    {

        TerrainType[] terrainMap=GenerateMap((PerlinNoiseSettings)systemManager.GetSharedData<PerlinNoiseSettings>());

        systemManager.AddSharedData(new ArrayContainer<TerrainType>
        {
            Data = terrainMap,
        });
    }

    private TerrainType[] GenerateMap(in PerlinNoiseSettings perlinNoiseSettings)
    {
        terrainMap = new TerrainType[MapSettings.MapWidth * MapSettings.MapHeight];
        for (int x = 0; x < MapSettings.MapWidth; x++)
        {
            for (int y = 0; y < MapSettings.MapHeight; y++)
            {
                float perlinValue = Mathf.PerlinNoise((x + perlinNoiseSettings.OffsetX + 0.001f) / perlinNoiseSettings.Scale, (y + perlinNoiseSettings.OffsetY + 0.001f) / perlinNoiseSettings.Scale);
                perlinValue = Mathf.Clamp01(perlinValue); 
                TerrainType terrainType = DetermineTerrainType(in perlinNoiseSettings,perlinValue);
                
                int index = x + y * MapSettings.MapWidth;
                terrainMap[index] = terrainType;
            }
        }

        return terrainMap;
    }



    private TerrainType DetermineTerrainType(in PerlinNoiseSettings perlinNoiseSettings,float height)
    {
        if (height < perlinNoiseSettings.SeaLevel)
            return TerrainType.Sea;
        else if (height < perlinNoiseSettings.SandLevel)
            return TerrainType.Sand;
        else if (height < perlinNoiseSettings.GrassLevel)
            return TerrainType.Grass;
        else if (height < perlinNoiseSettings.ForestLevel)
            return TerrainType.Forest;
        else
            return TerrainType.Mountain;
    }

   
}


