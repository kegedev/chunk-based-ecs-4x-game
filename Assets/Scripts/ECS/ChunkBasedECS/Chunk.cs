using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Analytics;

internal unsafe struct Chunk
{
    internal int ChunkId;
    internal int ChunkSize;
    internal int EntityCount;
    internal ComponentMask ComponentMask;
    internal int* ComponentIdArray;
    internal void** ComponentArrays;
}

internal static class ChunkUtility
{

    internal static unsafe Chunk CreateChunk(  int chunkId,
                                             int chunkSize,
                                             int entityCount,
                                             ComponentMask componentMask)
    {


        Chunk newChunk= new Chunk() 
        { 
            ChunkId = chunkId,
            ChunkSize = chunkSize,
            EntityCount = entityCount,
            ComponentMask = componentMask
        };

        AllocateChunkArrays(ref newChunk, chunkSize,componentMask);

        return newChunk;
    }

    #region ComponentArraysAllocation
    internal static unsafe void AllocateChunkArrays(ref Chunk chunk,int chunkSize,ComponentMask componentMask)
    {
        int componentCount = 0;
        componentCount = GetComponentCount(componentMask);
       
        chunk.ComponentIdArray=(int*)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<int>()*componentCount,UnsafeUtility.AlignOf<int>(),Allocator.Persistent);
        UnsafeUtility.MemClear(chunk.ComponentIdArray, UnsafeUtility.SizeOf<int>() * componentCount);

        chunk.ComponentArrays = (void**)UnsafeUtility.Malloc(UnsafeUtility.SizeOf<IntPtr>()*chunkSize,UnsafeUtility.AlignOf<IntPtr>(),Allocator.Persistent);
        UnsafeUtility.MemClear(chunk.ComponentArrays, UnsafeUtility.SizeOf<IntPtr>() * chunkSize);

      
  
        int currentComponentIndex = 0;

        List<ComponentMask> compMasks = GetComponentMasks(chunk.ComponentMask);

        foreach (var item in compMasks)
        {
         
             
                int componentId = ComponentMaskIdConversion(item);
           
                UnsafeUtility.WriteArrayElement<int>(chunk.ComponentIdArray,currentComponentIndex,componentId);

                void* currentComponentArray = UnsafeUtility.Malloc(GetComponentSizebyId(componentId)*chunk.ChunkSize, GetComponentAlignbyId(componentId),Allocator.Persistent);
                UnsafeUtility.MemClear(currentComponentArray, GetComponentSizebyId(componentId) * chunk.ChunkSize);

                UnsafeUtility.WriteArrayElement(chunk.ComponentArrays, currentComponentIndex++, (IntPtr)currentComponentArray);
        }



    }
    #endregion
    #region EntityAddRemoveRegion
    internal static unsafe void AddEntity(ref Chunk chunk,params IComponent[] components)
    {
       
        int entityIndex = chunk.EntityCount;

        foreach (IComponent component in components)
        {
            int typeInt = ComponentTypeIntConversion(component);
            int index = -1;

            for (int i = 0; i < GetComponentCount(chunk.ComponentMask); i++)
            {
                if (UnsafeUtility.ReadArrayElement<int>(chunk.ComponentIdArray, i) == typeInt)
                {
                    index = i; break;
                }
            }



            IntPtr currentCompArray = UnsafeUtility.ReadArrayElement<IntPtr>(chunk.ComponentArrays, index);


            SetComponentArrayValueAtIndex(currentCompArray, chunk.EntityCount, component);
        }

       chunk.EntityCount++;
    }
    internal static void RemoveEntity()
    {

    }
    #endregion

    #region ComponentArrayCreateDestroyRegion
    private static unsafe IntPtr CreateComponentArrayUnsafe(ref Chunk chunk, IComponent component)
    {
        IntPtr componentArrayPtr = (IntPtr)UnsafeUtility.Malloc(
                                                                  Marshal.SizeOf(component.GetType()) * chunk.ChunkSize,
                                                                  GetComponentAlignment(component),
                                                                  Allocator.Persistent);
        return componentArrayPtr;

    }

    private static unsafe void DestroyComponentArray(ref QuadTreeNodeData component)
    {

    }
    #endregion

    #region ReadWriteComponentValueRegion


    private static unsafe void SetComponentArrayValueAtIndex(IntPtr intPtr, int index, IComponent component)
    {
   
        switch (component)
        {
            case PositionComponent positionComponent:
                {
                    UnsafeUtility.WriteArrayElement(intPtr.ToPointer(), index, positionComponent);
                    break;
                }
            case RotationComponent rotationComponent:
                {
                    UnsafeUtility.WriteArrayElement(intPtr.ToPointer(), index, rotationComponent);
                    break;
                }
            case ScaleComponent scaleComponent:
                {
                    UnsafeUtility.WriteArrayElement(intPtr.ToPointer(), index, scaleComponent);
                    break;
                }
            case TileComponent tileComponent:
                {
                    UnsafeUtility.WriteArrayElement(intPtr.ToPointer(), index, tileComponent);
                    break;
                }
            case RenderComponent renderComponent:
                {
                    UnsafeUtility.WriteArrayElement(intPtr.ToPointer(), index, renderComponent);
                    break;
                }
            case QuadTreeLeafComponent quadTreeLeafComponent:
                {
                    UnsafeUtility.WriteArrayElement(intPtr.ToPointer(), index, quadTreeLeafComponent);
                    break;
                }
            case SoldierComponent soldierComponent:
                {
                    UnsafeUtility.WriteArrayElement(intPtr.ToPointer(), index, soldierComponent);
                    break;
                }
            case MoverComponent moverComponent:
                {
              
                    UnsafeUtility.WriteArrayElement(intPtr.ToPointer(), index, moverComponent);
                    break;
                }
            case CoordinateComponent coordinateComponent:
                {
                    UnsafeUtility.WriteArrayElement(intPtr.ToPointer(), index, coordinateComponent);
                    break;
                }
            case DynamicRenderComponent dynamicRenderComponent:
                {
                    UnsafeUtility.WriteArrayElement(intPtr.ToPointer(), index, dynamicRenderComponent);
                    break;
                }
            default:
                Debug.LogError("No description for component");
                break;
        }
    }

    public static unsafe ref T GetEntityComponentValueAtIndex<T>(in Chunk chunk,int index) where T : struct
    {
       
        int CompArrayIndex = -1;

        for (int i = 0; i < GetComponentCount(chunk.ComponentMask); i++)
        {
         
            if (UnsafeUtility.ReadArrayElement<int>(chunk.ComponentIdArray, i) == ComponentTypeIntConversion(typeof(T)))
            {
                CompArrayIndex = i; break;
            }
        }



        IntPtr currentCompArray = UnsafeUtility.ReadArrayElement<IntPtr>(chunk.ComponentArrays, CompArrayIndex);


        if (currentCompArray!=null)
        {

            return ref GetComponentArrayValueAtIndex<T>(currentCompArray, index);

        }
        else
        {
          
            throw new KeyNotFoundException($"Component array for type {typeof(T)} does not exist.");
        }
    }

    private static unsafe ref T GetComponentArrayValueAtIndex<T>(IntPtr intPtr, int index) where T : struct
    {
        void* ptr = (byte*)intPtr.ToPointer() + index * UnsafeUtility.SizeOf<T>();

        return ref UnsafeUtility.AsRef<T>(ptr);
    }

    public static unsafe T[] GetAllComponents<T>(in Chunk chunk) where T : struct
    {
        T[] values = new T[chunk.EntityCount];

        int CompArrayIndex = -1;

        for (int i = 0; i < GetComponentCount(chunk.ComponentMask); i++)
        {
            if (UnsafeUtility.ReadArrayElement<int>(chunk.ComponentIdArray, i) == ComponentTypeIntConversion(typeof(T)))
            {
                CompArrayIndex = i; break;
            }
        }



        IntPtr currentCompArray = UnsafeUtility.ReadArrayElement<IntPtr>(chunk.ComponentArrays, CompArrayIndex);


        if (currentCompArray!=null)
        {
            for (int i = 0; i < chunk.EntityCount; i++)
            {
                values[i] = GetComponentArrayValueAtIndex<T>(currentCompArray, i);
            }
            return values;

        }
        else
        {
            Debug.LogError($"The component array could not be found for type {typeof(T)}.");
            return Array.Empty<T>();
        }
    }

    #endregion

    #region UtilityMethods
    public static int ComponentTypeIntConversion(IComponent component)
    {
        return component switch
        {
            PositionComponent => 1,
            RotationComponent => 2,
            ScaleComponent => 3,
            TileComponent => 4,
            RenderComponent => 5,
            QuadTreeLeafComponent => 6,
            SoldierComponent => 7,
            MoverComponent => 8,
            CoordinateComponent => 9,
            DynamicRenderComponent => 10,
           
            _ => throw new Exception("Conversion cannot be done!")
        };

    }
    public static int ComponentTypeIntConversion(Type type)
    {


        if (type == typeof(PositionComponent))
        {
            return 1;
        }
        else
        if (type == typeof(RotationComponent))
        {
            return 2;
        }
        else
        if (type == typeof(ScaleComponent))
        {
            return 3;
        }
        else
        if (type == typeof(TileComponent))
        {
            return 4;
        }
        else
        if (type == typeof(RenderComponent))
        {
            return 5;
        }
        else
        if (type == typeof(QuadTreeLeafComponent))
        {
            return 6;
        }
        else
        if (type == typeof(SoldierComponent))
        {
            return 7;
        }
        else
        if (type == typeof(MoverComponent))
        {
            return 8;
        }
        else
        if (type == typeof(CoordinateComponent))
        {
            return 9;
        }
        else
        if (type == typeof(DynamicRenderComponent))
        {
            return 10;
        }
        else  throw new Exception("Conversion cannot be done!");

    }


    public static int ComponentMaskIdConversion(ComponentMask componentUshort)
    {
        
        return componentUshort switch
        {
            ComponentMask.PositionComponent => 1,
            ComponentMask.RotationComponent => 2,
            ComponentMask.ScaleComponent => 3,
            ComponentMask.TileComponent => 4,
            ComponentMask.StaticRenderComponent => 5,
            ComponentMask.QuadTreeLeafComponent => 6,
            ComponentMask.SoldierComponent => 7,
            ComponentMask.MoverComponent => 8,
            ComponentMask.CoordinateComponent => 9,
            ComponentMask.DynamicRenderComponent => 10,
            _ => throw new Exception("Conversion cannot be done!")
        };

    }
    public static int GetComponentAlignment(IComponent component)
    {
        return component switch
        {
            PositionComponent => UnsafeUtility.AlignOf<PositionComponent>(),
            ScaleComponent => UnsafeUtility.AlignOf<ScaleComponent>(),
            RotationComponent => UnsafeUtility.AlignOf<RotationComponent>(),
            TileComponent => UnsafeUtility.AlignOf<TileComponent>(),
            RenderComponent => UnsafeUtility.AlignOf<RenderComponent>(),
            QuadTreeLeafComponent => UnsafeUtility.AlignOf<QuadTreeLeafComponent>(),
            SoldierComponent => UnsafeUtility.AlignOf<SoldierComponent>(),
            MoverComponent => UnsafeUtility.AlignOf<MoverComponent>(),
            CoordinateComponent => UnsafeUtility.AlignOf<CoordinateComponent>(),
            DynamicRenderComponent => UnsafeUtility.AlignOf<DynamicRenderComponent>(),
            _ => throw new Exception("Conversion cannot be done!")
        };

    }
    public static int GetComponentAlignbyId(int componentId)
    {
        return componentId switch
        {
            1 => UnsafeUtility.AlignOf<PositionComponent>(),
            3 => UnsafeUtility.AlignOf<RotationComponent>(),
            2 => UnsafeUtility.AlignOf<ScaleComponent>(),
            4 => UnsafeUtility.AlignOf<TileComponent>(),
            5 => UnsafeUtility.AlignOf<RenderComponent>(),
            6 => UnsafeUtility.AlignOf<QuadTreeLeafComponent>(),
            7 => UnsafeUtility.AlignOf<SoldierComponent>(),
            8 => UnsafeUtility.AlignOf<MoverComponent>(),
            9 => UnsafeUtility.AlignOf<CoordinateComponent>(),
            10 => UnsafeUtility.AlignOf<DynamicRenderComponent>(),
            _ => throw new Exception("Conversion cannot be done!")
        };


    }
    public static int GetComponentSizebyId(int componentId)
    {
        return componentId switch
        {
            1 => UnsafeUtility.SizeOf<PositionComponent>(),
            3 => UnsafeUtility.SizeOf<RotationComponent>(),
            2 => UnsafeUtility.SizeOf<ScaleComponent>(),
            4 => UnsafeUtility.SizeOf<TileComponent>(),
            5 => UnsafeUtility.SizeOf<RenderComponent>(),
            6 => UnsafeUtility.SizeOf<QuadTreeLeafComponent>(),
            7 => UnsafeUtility.SizeOf<SoldierComponent>(),
            8 => UnsafeUtility.SizeOf<MoverComponent>(),
            9 => UnsafeUtility.SizeOf<CoordinateComponent>(),
            10 => UnsafeUtility.SizeOf<DynamicRenderComponent>(),
            _ => throw new Exception("Conversion cannot be done!")
        };


    }
    public static int GetComponentCount(ComponentMask componentMask)
    {
        ushort chunkMask= (ushort)componentMask;
        ushort checkMask = 0b1000000000000000; ;
        ushort componentCount = 0;

        while (checkMask != 0)
        {

            if ((chunkMask & checkMask) == checkMask) componentCount++;

            checkMask >>= 1;
        }



        return componentCount;
    }
    public static List<ComponentMask> GetComponentMasks(ComponentMask componentMask)
    {
        List<ComponentMask> ids = new List<ComponentMask>();    
        ushort chunkMask = (ushort)componentMask;
        ushort checkMask = 0b1000000000000000;
       
        while (checkMask != 0)
        {
            if ((chunkMask & checkMask) == checkMask)
            { 
            
            ids.Add((ComponentMask)checkMask);
            }
            

            checkMask >>= 1;
        }



        return ids;
    }
    public static bool HasComponent(ref Chunk chunk, ComponentMask componentMask)
    {
        return (chunk.ComponentMask & componentMask) == componentMask;
    }

    public static bool IsChunkSuitable(in Chunk chunk, ComponentMask componentMask)
    {
        return chunk.ComponentMask == componentMask && chunk.EntityCount < chunk.ChunkSize;
    }
    #endregion

}




