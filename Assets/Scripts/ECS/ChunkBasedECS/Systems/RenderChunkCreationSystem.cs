using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class RenderChunkCreationSystem : IInitSystem
{

    public void Init(SystemManager systemManager)
    {

        Dictionary<TerrainType, List<Matrix4x4>> keyValuePairs = CreateTerrainTypeMatrices(systemManager.GetWorld());
        CreateTileRenderChunks(systemManager.GetWorld(),keyValuePairs);

        //OPEN

        Dictionary<SoldierType, List<Matrix4x4>> keyValuePairsSoldier = CreateSoldierTypeMatrices(systemManager.GetWorld());
        CreateSoldierRenderChunks(systemManager.GetWorld(), keyValuePairsSoldier);


    }

    public Dictionary<TerrainType, List<Matrix4x4>> CreateTerrainTypeMatrices(ECSWorld eCSWorld)
    {
        Dictionary<TerrainType, List<Matrix4x4>> terrainMatrices=new Dictionary<TerrainType, List<Matrix4x4>>();
        foreach (var chunk in eCSWorld.ChunkContainers[(ushort)(ComponentMask.CoordinateComponent | ComponentMask.TileComponent)])
        {
           TileComponent[] tileComponents= ChunkUtility.GetAllComponents<TileComponent>(chunk);
           CoordinateComponent[] coordinateComponents= ChunkUtility.GetAllComponents<CoordinateComponent>(chunk);
            //TileComponent[] tileComponents= chunk.GetAllComponents<TileComponent>();
           //CoordinateComponent[] coordinateComponents= chunk.GetAllComponents<CoordinateComponent>();
      
            for (int i = 0; i < chunk.EntityCount; i++)
            {
                
                 if (terrainMatrices.ContainsKey(tileComponents[i].TerrainType))//trygetvalueya cevir
                 {
                    terrainMatrices[tileComponents[i].TerrainType].Add(
                            Matrix4x4.TRS(new Vector3((coordinateComponents[i].Coordinate.x* MapSettings.TileEdgeSize) ,
                                                      -1f,
                                                      (coordinateComponents[i].Coordinate.y * MapSettings.TileEdgeSize)),
                                          Quaternion.identity,
                                          Vector3.one* MapSettings.TileEdgeSize*0.99f));
                }
                else
                {
                    terrainMatrices.Add(tileComponents[i].TerrainType,new List<Matrix4x4> { Matrix4x4.TRS(new Vector3((coordinateComponents[i].Coordinate.x*MapSettings.TileEdgeSize),
                                                                                                                      -1f,
                                                                                                                      (coordinateComponents[i].Coordinate.y * MapSettings.TileEdgeSize)),
                                                                                                          Quaternion.identity,
                                                                                                          Vector3.one*MapSettings.TileEdgeSize*0.99f)});
                }
                
            }

        }
        return terrainMatrices; 
    }

    public void CreateTileRenderChunks(ECSWorld eCSWorld,Dictionary<TerrainType, List<Matrix4x4>> tileterrainMatrixPair)
    {
        foreach (var keyValuePair in tileterrainMatrixPair)
        {
            int index = 0;
            List<Matrix4x4> chunkMatrices = new List<Matrix4x4>();

            foreach (var matrix in keyValuePair.Value)
            {
                chunkMatrices.Add(matrix);
                index++;

                if (index == 1024)
                {
                    NativeArray<Matrix4x4> nativeArray = new NativeArray<Matrix4x4>(chunkMatrices.Count, Allocator.Persistent);
                    for (int i = 0; i < chunkMatrices.Count; i++)
                    {
                        nativeArray[i] = chunkMatrices[i];
                    }

                    RenderComponent newRenderComponent = new RenderComponent()
                    {
                        MeshType = (int)keyValuePair.Key,
                        MaterialType = 1,
                        TextureOffset = new float2(0, 0),
                        Matrices = new NativeArray<Matrix4x4>(chunkMatrices.Count, Allocator.Persistent)
                    };

                    newRenderComponent.Matrices.CopyFrom(nativeArray);

                    eCSWorld.AddEntity(ComponentMask.StaticRenderComponent, 1024, new IComponent[1]
                    {
                        newRenderComponent


                    });

                    chunkMatrices.Clear();
                    index = 0;
                }
            }

            if (chunkMatrices.Count > 0)
            {
                eCSWorld.AddEntity(ComponentMask.StaticRenderComponent, chunkMatrices.Count, new IComponent[1]
                {
                    new RenderComponent()
                    {
                        MeshType = (int)keyValuePair.Key,
                        MaterialType = 1,
                        TextureOffset = new float2(0, 0),
                        Matrices = chunkMatrices.ToNativeArray<Matrix4x4>( Allocator.Persistent)
                    }
                });
            }
        }
    }

    public Dictionary<SoldierType, List<Matrix4x4>> CreateSoldierTypeMatrices(ECSWorld eCSWorld)
    {
        Dictionary<SoldierType, List<Matrix4x4>> terrainMatrices = new Dictionary<SoldierType, List<Matrix4x4>>();
        foreach (var chunk in eCSWorld.ChunkContainers[(ushort)(ComponentMask.CoordinateComponent | ComponentMask.SoldierComponent | ComponentMask.MoverComponent)])
        {
            CoordinateComponent[] coordinateComponents = ChunkUtility.GetAllComponents<CoordinateComponent>(chunk);
            SoldierComponent[] soldierComponents = ChunkUtility.GetAllComponents<SoldierComponent>(chunk);
            //CoordinateComponent[] coordinateComponents = chunk.GetAllComponents<CoordinateComponent>();
            //SoldierComponent[] soldierComponents = chunk.GetAllComponents<SoldierComponent>();
           
           

            for (int i = 0; i < chunk.EntityCount; i++)
            {
                if (terrainMatrices.ContainsKey(soldierComponents[i].SoldierType))//trygetvalueya cevir
                {
                    terrainMatrices[soldierComponents[i].SoldierType].Add(
                            Matrix4x4.TRS(new Vector3(coordinateComponents[i].Coordinate.x * MapSettings.TileEdgeSize,
                                                       0f,
                                                       coordinateComponents[i].Coordinate.y * MapSettings.TileEdgeSize),
                                          Quaternion.identity,
                                          Vector3.one ));
                }
                else
                {
                    terrainMatrices.Add(soldierComponents[i].SoldierType, new List<Matrix4x4> { Matrix4x4.TRS(new Vector3(coordinateComponents[i].Coordinate.x * MapSettings.TileEdgeSize,
                                                                                                             0f,
                                                                                                             coordinateComponents[i].Coordinate.y * MapSettings.TileEdgeSize),
                                                                                                Quaternion.identity,
                                                                                                Vector3.one )});
                }

            }

        }
        return terrainMatrices;
    }

    public void CreateSoldierRenderChunks(ECSWorld eCSWorld, Dictionary<SoldierType, List<Matrix4x4>> tileterrainMatrixPair)
    {
        foreach (var keyValuePair in tileterrainMatrixPair)
        {
            int index = 0;
            List<Matrix4x4> chunkMatrices = new List<Matrix4x4>();

            foreach (var matrix in keyValuePair.Value)
            {
                chunkMatrices.Add(matrix);
                index++;

                if (index == 1024)
                {
                    NativeArray<Matrix4x4> nativeArray = new NativeArray<Matrix4x4>(chunkMatrices.Count, Allocator.Persistent);
                    for (int i = 0; i < chunkMatrices.Count; i++)
                    {
                        nativeArray[i] = chunkMatrices[i];
                    }

                    DynamicRenderComponent newRenderComponent = new DynamicRenderComponent()
                    {
                        MeshType = (int)keyValuePair.Key,
                        MaterialType = 1,
                        TextureOffset = new float2(0, 0),
                        Matrices = new NativeArray<Matrix4x4>(chunkMatrices.Count, Allocator.Persistent)
                    };

                    newRenderComponent.Matrices.CopyFrom(nativeArray);

                    eCSWorld.AddEntity(ComponentMask.DynamicRenderComponent, 1024, new IComponent[1]
                    {
                        newRenderComponent


                    });

                    chunkMatrices.Clear();
                    index = 0;
                }
            }

            if (chunkMatrices.Count > 0)
            {
                eCSWorld.AddEntity(ComponentMask.DynamicRenderComponent, chunkMatrices.Count, new IComponent[1]
                {
                    new DynamicRenderComponent()
                    {
                        MeshType = (int)keyValuePair.Key,
                        MaterialType = 1,
                        TextureOffset = new float2(0, 0),
                        Matrices = chunkMatrices.ToNativeArray<Matrix4x4>( Allocator.Persistent)
                    }
                });
            }
        }
    }
}
