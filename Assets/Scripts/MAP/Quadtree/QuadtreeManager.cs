using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class QuadtreeManager
{


    //public void AddQuadtree(ref List<Chunk> quadtreeChunks,QuadtreeType quadtreeType, List<Chunk> chunks, int mapWidth, int mapHeight, float tileWidth)
    //{
    //    Quadtrees.Add(quadtreeType, CreateQuadTree(chunks, mapWidth, mapHeight, tileWidth));
    //}

    //public QuadTreeNodeData GetQuadtree(QuadtreeType quadtreeType)
    //{
    //    if(Quadtrees.TryGetValue(quadtreeType, out QuadTreeNodeData quadtreeRoot))
    //    {
    //        return quadtreeRoot;
    //    }
    //    else
    //    {
    //        Debug.LogError(quadtreeType + " Not exist!");
    //        return default;
    //    }
    //}
    //public QuadTreeNodeData CreateQuadTree(QuadTreeLeafComponent[] quadTreeLeafComponents, int mapWidth, int mapHeight, float tileWidth)
    //{
    //    QuadTreeNodeData TileQuadTreeRoot = new QuadTreeNodeData()
    //    {
    //        Rect = new Rect(-tileWidth / 2, -tileWidth / 2, mapWidth, mapHeight),
    //        Leaves = new NativeList<int>(Allocator.Persistent),
    //        Nodes = new NativeArray<QuadTreeNodeData>(4, Allocator.Persistent),
    //        Capacity = 4,
    //        IsDivided = false
    //    };

    //    for (int i = 0; i < quadTreeLeafComponents.Length; i++)
    //    {
           
    //        InsertLeaf(ref TileQuadTreeRoot, quadTreeLeafComponents, quadTreeLeafComponents[i].LeafID);
    //    }
    //    return TileQuadTreeRoot;
    //}
    //private void InsertLeaf(ref QuadTreeNodeData rootNode, QuadTreeLeafComponent[] quadTreeLeafComponents, int leafId)
    //{
    //    //Debug.Log("leafId "+leafId+"q");
       
    //    if (rootNode.IsDivided)
    //    {
           
    //        for (int i = 0; i < 4; i++)
    //        {
    //            if (IsLeafInNode(rootNode.Nodes[i], quadTreeLeafComponents[leafId]))
    //            {
    //                InsertLeaf(ref rootNode.Nodes[i], quadTreeLeafComponents, leafId);
    //                return;
    //            }
    //        }
    //    }
    //    else
    //    {
          
    //        if (!rootNode.Leaves.Contains(leafId))
    //            rootNode.Leaves.Add(leafId);

           
    //        if (rootNode.Leaves.Length > rootNode.Capacity)
    //        {
    //            Subdivide(ref rootNode);

               
    //            foreach (var item in rootNode.Leaves)
    //            {
    //                for (int i = 0; i < 4; i++)
    //                {
    //                    if (IsLeafInNode(rootNode.Nodes[i], quadTreeLeafComponents[item]))
    //                    {
    //                        InsertLeaf(ref rootNode.Nodes[i], quadTreeLeafComponents, item);
    //                        break;
    //                    }
    //                }
    //            }
              
    //            rootNode.Leaves.Clear();
    //        }
    //    }
    //}

    //private bool IsLeafInNode(QuadTreeNodeData node, QuadTreeLeafComponent leaf)
    //{
       
    //    return node.Rect.Contains(leaf.Rect.center);
    //}


    //private void Subdivide(ref QuadTreeNodeData node)
    //{
    //    float halfWidth = node.Rect.width / 2f;
    //    float halfHeight = node.Rect.height / 2f;

    //    node.Nodes = new NativeArray<QuadTreeNodeData>(4,Allocator.Persistent);
        
    //    node.Nodes[0] = new QuadTreeNodeData()
    //    {
    //        Rect = new Rect(node.Rect.x, node.Rect.y, halfWidth, halfHeight),
    //        Leaves = new NativeList<int>(Allocator.Persistent),
    //        Nodes = new NativeArray<QuadTreeNodeData>(4, Allocator.Persistent),
    //        Capacity = 4,
    //        IsDivided = false
    //    };
    //    node.Nodes[1] = new QuadTreeNodeData()
    //    {
    //        Rect = new Rect(node.Rect.x + halfWidth, node.Rect.y, halfWidth, halfHeight),
    //        Leaves = new NativeList<int>(Allocator.Persistent),
    //        Nodes = new NativeArray<QuadTreeNodeData>(4, Allocator.Persistent),
    //        Capacity = 4,
    //        IsDivided = false
    //    };
    //    node.Nodes[2] = new QuadTreeNodeData()
    //    {
    //        Rect = new Rect(node.Rect.x, node.Rect.y + halfHeight, halfWidth, halfHeight),
    //        Leaves = new NativeList<int>(Allocator.Persistent),
    //        Nodes = new NativeArray<QuadTreeNodeData>(4, Allocator.Persistent),
    //        Capacity = 4,
    //        IsDivided = false
    //    };
    //    node.Nodes[3] = new QuadTreeNodeData()
    //    {
    //        Rect = new Rect(node.Rect.x + halfWidth, node.Rect.y + halfHeight, halfWidth, halfHeight),
    //        Leaves = new NativeList<int>(Allocator.Persistent),
    //        Nodes = new NativeArray<QuadTreeNodeData>(4, Allocator.Persistent),
    //        Capacity = 4,
    //        IsDivided = false
    //    };
    //    node.IsDivided = true;
    //}

    //public int GetDepth(QuadTreeNodeData rootNode, int currentDepth=0)
    //{
    //    if(rootNode.IsDivided)
    //    {
    //        currentDepth++;
    //        return GetDepth(rootNode.Nodes[0], currentDepth);
    //    }
    //    else
    //    {
    //        return currentDepth;
    //    }
    //}

    //public static List<QuadTreeNodeData> GetNodesAtDepth(QuadTreeNodeData rootNode, int targetDepth, int currentDepth = 0)
    //{
    //    List<QuadTreeNodeData> result = new List<QuadTreeNodeData>();

    //    if (currentDepth == targetDepth)
    //    {
    //        result.Add(rootNode);
    //        return result;
    //    }

    //    if (rootNode.IsDivided && rootNode.Nodes != null)
    //    {
    //        foreach (var child in rootNode.Nodes)
    //        {
    //            result.AddRange(GetNodesAtDepth(child, targetDepth, currentDepth + 1));
    //        }
    //    }

    //    return result;
    //}






  

    //private QuadTreeLeafComponent GetQuadtreeLeafComponent(QuadTreeLeafComponent[] quadTreeLeafComponents, int chunkID)
    //{
    //    foreach (var quadTreeLeafComponent in quadTreeLeafComponents)
    //    {
    //        if (quadTreeLeafComponent.LeafID == chunkID) return quadTreeLeafComponent;
    //    }
    //    Debug.LogError("quadTreeLeafComponent not found");
    //    return default;
    //}
}
