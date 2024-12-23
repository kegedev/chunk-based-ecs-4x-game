using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

internal static class QuerySystem
{

    internal static Action<int2, int> ProcessSelection;


    internal static void ClickedOnMap(in NativeList<Chunk> tileChunks,
                                    in Chunk quadtreeChunk,
                                    in NativeList<QuadTreeNodeData> quadTreeNodeDatas,
                                    in NativeList<int> QuadtreeNodeIndexes,
                                    in NativeList<int> QuadtreeLeavesIndexes,
                                    in QuadTreeNodeData tileQuadTree, 
                                    float3 pos)
    {
        Vector2 clickPosOnXZPlane = new Vector2(pos.x,pos.z);
        EntityIndexReference entityIndexReference = GetEntityChunkIndexReference(tileChunks,
                                                                                 quadtreeChunk,
                                                                                 quadTreeNodeDatas,
                                                                                 QuadtreeNodeIndexes,
                                                                                 QuadtreeLeavesIndexes,
                                                                                 tileQuadTree, 
                                                                                 clickPosOnXZPlane);

        Chunk chunk= tileChunks[entityIndexReference.ChunkId];
        ref var tileComponent = ref ChunkUtility.GetEntityComponentValueAtIndex<TileComponent>( chunk, entityIndexReference.Index);
        ref var coord = ref ChunkUtility.GetEntityComponentValueAtIndex<CoordinateComponent>( chunk, entityIndexReference.Index);

        ProcessSelection.Invoke(coord.Coordinate, tileComponent.MoverIndex);

       Debug.Log(pos+" "+coord.Coordinate + " "+tileComponent.MoverIndex + " " + tileComponent.TerrainType);
     
    }


    internal static EntityIndexReference GetEntityChunkIndexReference(in NativeList<Chunk> tileChunks,
                                                                    in Chunk quadtreeChunk,
                                                                    in NativeList<QuadTreeNodeData> quadTreeNodeDatas,
                                                                    in NativeList<int> QuadtreeNodeIndexes,
                                                                    in NativeList<int> QuadtreeLeavesIndexes,
                                                                    in QuadTreeNodeData rootNode, 
                                                                    Vector2 point)
    {
        int chunkId = GetChunkId(tileChunks,
                                 quadtreeChunk,
                                 quadTreeNodeDatas,
                                 QuadtreeNodeIndexes,
                                 QuadtreeLeavesIndexes,
                                 rootNode, 
                                 point);

        Chunk chunk = tileChunks[chunkId];
        
        return new EntityIndexReference() { ChunkId = chunkId, Index = SearchTileInChunk(chunk, new int2((int)point.x, (int)point.y)) };
    }


    internal static int GetChunkId(in NativeList<Chunk> tileChunks,
                                 in Chunk quadtreeChunk,
                                 in NativeList<QuadTreeNodeData> quadTreeNodeDatas,
                                 in NativeList<int> QuadtreeNodeIndexes,
                                 in NativeList<int> QuadtreeLeavesIndexes,
                                 in QuadTreeNodeData rootNode, 
                                 Vector2 point)
    {
        if (!rootNode.Rect.Contains(point))
        {
            Debug.LogError("Point is out of bounds of the root node.");
            return -1;
        }

        QuadTreeNodeData currentNode = rootNode;
        while (currentNode.IsDivided)
        {
            bool foundInChild = false;
            for (int i = 0; i < 4; i++)
            {
                if (quadTreeNodeDatas[QuadtreeNodeIndexes[currentNode.NodesStart+i]].Rect.Contains(point))
                {
                    currentNode = quadTreeNodeDatas[QuadtreeNodeIndexes[currentNode.NodesStart + i]];
                    foundInChild = true;
                    break;
                }
            }
            if (!foundInChild)
            {
                Debug.LogError("Point not found in any child nodes.");
                return -1;
            }
        }

       
        

        for (int i = 0; i < currentNode.Capacity; i++)
        {
            QuadTreeLeafComponent quadTreeLeafComponent = ChunkUtility.GetEntityComponentValueAtIndex<QuadTreeLeafComponent>(quadtreeChunk, QuadtreeLeavesIndexes[currentNode.LeavesStart+i]);
            if (quadTreeLeafComponent.Rect.Contains(point))
            {
                return QuadtreeLeavesIndexes[currentNode.LeavesStart + i];
            }
        }

        Debug.LogError("Leaf containing the point not found.");
        return -1;
    }

    internal static Chunk GetChunkByIndexReference(in NativeList<Chunk> tileChunks, in EntityIndexReference entityIndexReference)
    {
        return tileChunks[entityIndexReference.ChunkId];
    }

    internal static int SearchTileInChunk(in Chunk chunk, int2 coordinate)
    {
        CoordinateComponent[] coordinateComponents = ChunkUtility.GetAllComponents<CoordinateComponent>( chunk);
       
        float tileEdgeSize = MapSettings.TileEdgeSize;
        float2 targetPos = new float2((float)coordinate.x, (float)coordinate.y);

        for (int i = 0; i < coordinateComponents.Length; i++)
        {

            float2 tilePos = new float2(
                (coordinateComponents[i].Coordinate.x * tileEdgeSize),
                (coordinateComponents[i].Coordinate.y * tileEdgeSize)
            );


            if (math.distance(tilePos, targetPos) <= tileEdgeSize / 2)
            {
               
                return i;
            }
        }

        throw new Exception("Tile cannot be found");
    }
    //public Chunk GetChunk(QuadTreeNodeData rootNode, Vector2 point)
    //{
    //    int chunkId = GetChunkId(rootNode, point);
    //    return _world.ChunkContainers[ComponentMask.TileComponentMask][chunkId];
    //}
    //public int SearchTileInChunk(Chunk chunk, float3 pos)
    //{
       
    //    CoordinateComponent[] coordinateComponents = ChunkUtility.GetAllComponents<CoordinateComponent>( chunk);
    //    //CoordinateComponent[] coordinateComponents = chunk.GetAllComponents<CoordinateComponent>();

    //    float tileEdgeSize = MapSettings.TileEdgeSize;
    //    float2 targetPos = new float2(pos.x, pos.z);

    //    for (int i = 0; i < coordinateComponents.Length; i++)
    //    {
    //        float2 tileCenter = new float2(
    //            coordinateComponents[i].Coordinate.x * tileEdgeSize,
    //            coordinateComponents[i].Coordinate.y * tileEdgeSize
    //        );

       
    //        float2 minBounds = tileCenter - (tileEdgeSize / 2) * new float2(1, 1);
    //        float2 maxBounds = tileCenter + (tileEdgeSize / 2) * new float2(1, 1);

    //        if (targetPos.x >= minBounds.x && targetPos.x <= maxBounds.x &&
    //            targetPos.y >= minBounds.y && targetPos.y <= maxBounds.y)
    //        {
    //            return i;
    //        }
    //    }

    //    throw new Exception("Tile cannot be found");
    //}
}

