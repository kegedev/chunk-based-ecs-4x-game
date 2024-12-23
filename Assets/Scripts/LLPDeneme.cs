using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Collections.LowLevel.Unsafe;
using System.Runtime.InteropServices;
using System;
using Unity.Mathematics;
using System.Drawing;
using System.Reflection;
using System.ComponentModel;
public class LLPDeneme : MonoBehaviour
{
    void Start()
    {

        CreateMemoryLocation();
    }

    private unsafe void CreateMemoryLocation()
    {
    //    Chunk chunk = new Chunk(ComponentMask.PositionComponent, 1);
    //    IComponent[] components = new IComponent[1];
    //    components[0] = new PositionComponent() { Position = new float3(3,3,3) };
    //    chunk.AddEntity(components);
    //    components[0] = new PositionComponent() { Position = new float3(1, 2, 3) };
    //    chunk.AddEntity(components);

    //   Debug.Log(chunk.GetEntityComponentValueAtIndex<PositionComponent>(0).Position);
    //   Debug.Log(chunk.GetEntityComponentValueAtIndex<PositionComponent>(1).Position);

    //    chunk.SetEntityComponentValueAtIndex(new PositionComponent() { Position = new float3(10, 3, 3) }, 0);
    //    Debug.Log("---------------");
    //Debug.Log(chunk.GetEntityComponentValueAtIndex<PositionComponent>(0).Position);
    //    Debug.Log(chunk.GetEntityComponentValueAtIndex<PositionComponent>(1).Position);

    //    Debug.Log(chunk.HasComponent(ComponentMask.PositionComponent));
    //    Debug.Log(chunk.HasComponent(ComponentMask.RotationComponent));

    }




}
