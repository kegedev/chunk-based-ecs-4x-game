using Unity.Mathematics;
using UnityEngine;

public class TileCreationSystem : IInitSystem
{

    private FactoryManager _factoryManager;

    public TileCreationSystem(FactoryManager factoryManager)
    {
        _factoryManager = factoryManager;
    }
    public void Init(SystemManager systemManager)
    {
        ECSWorld eCSWorld = systemManager.GetWorld();

        TerrainType[] terrainMap = ((ArrayContainer<TerrainType>)systemManager.GetSharedData<ArrayContainer<TerrainType>>()).Data;
        CreateTileChunks(eCSWorld,
                         terrainMap,
                         ComponentMask.CoordinateComponent | ComponentMask.TileComponent);

       
    }

    public void CreateTileChunks(ECSWorld eCSWorld,TerrainType[] terrainMap, ComponentMask componentMask)
    {
        int chunkXSize = MapSettings.TileChunkEdgeSize;
        int chunkYSize = MapSettings.TileChunkEdgeSize;

        int xChunkNumber = (MapSettings.MapWidth / chunkXSize);
        int yChunkNumber = (MapSettings.MapHeight / chunkYSize);

        for (int yCN = 0; yCN < yChunkNumber; yCN++)
        {
            for (int xCN = 0; xCN < xChunkNumber; xCN++)
            {
                for (int y = 0; y < chunkYSize; y++)
                {
                    for (int x = 0; x < chunkXSize; x++)
                    {
                        int absoluteX = x + xCN * chunkXSize;
                        int absoluteY = y + yCN * chunkYSize;

           

                        int2 coordinate = new int2(absoluteX, absoluteY);
                        TerrainType terrainType = GetTerrainType(in terrainMap, absoluteX, absoluteY);

                        if (absoluteX >= MapSettings.MapWidth || absoluteY >= MapSettings.MapHeight)
                            continue;
                        
                        eCSWorld.AddEntity(componentMask, 64, new IComponent[2]
                        {
                            _factoryManager.GetInstance<CoordinateComponent>((int2)coordinate),
                            _factoryManager.GetInstance<TileComponent>(new object[2]{ terrainType,-1 })
                        });


                    }
                }
            }
        }
    }

    public TerrainType GetTerrainType(in TerrainType[] terrainMap, int x, int y)
    {
        if (x < 0 || x >= MapSettings.MapWidth || y < 0 || y >= MapSettings.MapHeight)
        {
            Debug.LogError($"Coordinates ({x}, {y}) are out of bounds!");
            return TerrainType.None;
        }

        int index = x + y * MapSettings.MapWidth;

        return terrainMap[index];
    }
}
