using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class MovementSystem :  IUpdateSystem
{
    private float movementTimer = 0f;
    private const float movementInterval = 0.05f;
    public Action<CoordinateComponent, int> SetTileOccupant;
    public Action<ComponentMask, int,int2> UpdateRenderMatrix;


    public void Update(SystemManager systemManager)
    {
        movementTimer += Time.deltaTime;
        if (movementTimer >= movementInterval)
        {
            movementTimer -= movementInterval;
            NativeList<Chunk> tempChunks = systemManager.GetWorld().ChunkContainers[(ushort)(ComponentMask.CoordinateComponent | ComponentMask.SoldierComponent| ComponentMask.MoverComponent)];
            MoveEntities(ref tempChunks);
            systemManager.GetWorld().ChunkContainers[(ushort)(ComponentMask.CoordinateComponent | ComponentMask.SoldierComponent | ComponentMask.MoverComponent)] =tempChunks;
        }
       
    }

    internal void MoveEntities(ref NativeList<Chunk> moverChunks)
    {
        foreach (var chunk in moverChunks)
        {
            int entityCount = chunk.EntityCount;

            

            for (int i = 0; i < entityCount; i++)
            {
                ref var coordinateComp = ref ChunkUtility.GetEntityComponentValueAtIndex<CoordinateComponent>(chunk,i);
                ref var moverComp = ref ChunkUtility.GetEntityComponentValueAtIndex<MoverComponent>(chunk, i);
                //ref var coordinateComp = ref chunk.GetEntityComponentValueAtIndex<CoordinateComponent>(i);
                //ref var moverComp = ref chunk.GetEntityComponentValueAtIndex<MoverComponent>(i);

                if (!moverComp.HasPath) continue;
                if (moverComp.PathStepNumber != moverComp.Path.Length)
                {
                    SetTileOccupant.Invoke(coordinateComp, -1);
                    coordinateComp.Coordinate = moverComp.Path[moverComp.PathStepNumber];
                    SetTileOccupant.Invoke(coordinateComp, i);
            
                    UpdateRenderMatrix.Invoke(ComponentMask.DynamicRenderComponent, i, moverComp.Path[moverComp.PathStepNumber]);
            
                    moverComp.PathStepNumber++;
                    
                }
                else
                {
                    moverComp.HasPath = false;
                    moverComp.Path.Dispose();
                    moverComp.PathStepNumber = 0;
                    SetTileOccupant.Invoke(coordinateComp, i);

                }
            }
        }
    }
}
