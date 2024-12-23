using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class RenderSystem : IInitSystem, IUpdateSystem
{
    Mesh[] meshes;
    Material[] materials;

    ECSWorld _world;
    public void Init(SystemManager systemManager)
    {
        _world=systemManager.GetWorld();
        meshes = ((MeshContainer)systemManager.GetSharedData<MeshContainer>()).Meshes;
        materials = ((MaterialContainer)systemManager.GetSharedData<MaterialContainer>()).Materials;

    }
    public void Update(SystemManager systemManager)
    {

        RenderStaticEntities(
                       _world.ChunkContainers[(ushort)ComponentMask.StaticRenderComponent],
                       meshes,
                       materials);
        RenderDynamicEntities(
                       _world.ChunkContainers[(ushort)ComponentMask.DynamicRenderComponent],
                       meshes,
                       materials);
    }

    internal void RenderStaticEntities(NativeList<Chunk> chunks, Mesh[] meshes, Material[] materials)
    {
        foreach (var chunk in chunks)
        {
            for (int i = 0; i < chunk.EntityCount; i++)
            {
                var compNatArray = ChunkUtility.GetEntityComponentValueAtIndex<RenderComponent>(chunk,i);
                //var compNatArray = chunk.GetEntityComponentValueAtIndex<RenderComponent>(i);

                Graphics.DrawMeshInstanced(
                    meshes[compNatArray.MeshType],
                    0,
                    materials[0],
                    compNatArray.Matrices.ToArray()
                );
            }
        }
    }


    internal void RenderDynamicEntities(NativeList<Chunk> chunks, Mesh[] meshes, Material[] materials)
    {
        foreach (var chunk in chunks)
        {
            for (int i = 0; i < chunk.EntityCount; i++)
            {
                var compNatArray = ChunkUtility.GetEntityComponentValueAtIndex<DynamicRenderComponent>(chunk, i);
                //var compNatArray = chunk.GetEntityComponentValueAtIndex<RenderComponent>(i);

                Graphics.DrawMeshInstanced(
                    meshes[compNatArray.MeshType],
                    0,
                    materials[0],
                    compNatArray.Matrices.ToArray()
                );
            }
        }
    }
    public void UpdateMoverRenderComponentMatrix(ComponentMask componentMask,int index,int2 coord)
    {
        ref DynamicRenderComponent renderComp = ref ChunkUtility.GetEntityComponentValueAtIndex<DynamicRenderComponent>(_world.ChunkContainers[(ushort)componentMask][0],0);
        renderComp.Matrices[index] = Matrix4x4.TRS(new Vector3(coord.x,0,coord.y),//* _mapSettings.TileEdgeSize
                                                   Quaternion.identity,
                                                   Vector3.one);
    }
}
