
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ECSWorld
{

    internal NativeHashMap<ushort, NativeList<Chunk>> ChunkContainers;

    public int chunkIDCounter = 0;

    public QuadTreeNodeData TileQuadtreeRoot;


    public NativeList<QuadTreeNodeData> quadTreeNodeDatas = new NativeList<QuadTreeNodeData>(Allocator.Persistent);
    public NativeList<int> QuadtreeNodeIndexes= new NativeList<int>(Allocator.Persistent);
    public NativeList<int> QuadtreeLeafIndexes= new NativeList<int>(Allocator.Persistent);
    public int quadtreeNodeIndex = 0;
    public ECSWorld()
    {
        ChunkContainers= new NativeHashMap<ushort, NativeList<Chunk>>(0, Allocator.Persistent);
    }

    public void AddEntity(ComponentMask componentMask, int chunkSize, params IComponent[] components)
    {
       
        var maskKey = (ushort)componentMask;

        if (!ChunkContainers.ContainsKey(maskKey)) ChunkContainers.Add(maskKey, new NativeList<Chunk>(Allocator.Persistent));
        int index = FindOrCreateChunk(ChunkContainers[maskKey], componentMask, chunkSize);


        Chunk chunk = ChunkContainers[maskKey][index];
        ChunkUtility.AddEntity(ref chunk, components);

        NativeList<Chunk> tempNativeList = ChunkContainers[maskKey];
        tempNativeList[index] = chunk;
        ChunkContainers[maskKey] = tempNativeList;

    }

    internal int FindOrCreateChunk(NativeList<Chunk> chunkContainer, ComponentMask componentMask, int chunkSize)
    {

       // foreach (var chunk in chunkContainer.Chunks)
        for (int i = 0; i < chunkContainer.Length; i++)
        {
           
            if (ChunkUtility.IsChunkSuitable(chunkContainer[i], componentMask)) return i;
        }

        Chunk newChunk = ChunkUtility.CreateChunk(chunkIDCounter++, chunkSize,0, componentMask);
       


        //if (!chunkContainer.Chunks.Contains(newChunk))
        //{
        chunkContainer.Add(newChunk);
        

        return chunkContainer.Length - 1;
    }

    internal NativeList<Chunk> GetChunksByMask(ComponentMask ComponentMask)
    {
     
        return ChunkContainers[(ushort)ComponentMask];
    }



    //public Chunk GetChunkByID(NativeList<Chunk> chunks,int chunkId)
    //{
    //    foreach (var chunk in chunks)
    //    {
    //        if (chunk.ChunkId == chunkId) return chunk;
    //    }
    //    Debug.LogError("Chunk can not found");
    //    throw new Exception("Chunk can not found");
    //}

}

public struct EntityIndexReference
{
    public int ChunkId;
    public int Index;
}
