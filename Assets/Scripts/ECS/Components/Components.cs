using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public struct PositionComponent : IComponent
{
    public float3 Position;
}

public struct RotationComponent : IComponent
{
    public quaternion Quaternion;
}

public struct ScaleComponent : IComponent
{
    public float3 Scale;
}

public struct TileComponent : IComponent
{
    public TerrainType TerrainType;
    public int MoverIndex;
}

public struct RenderComponent : IComponent
{
    public int MeshType;
    public int MaterialType;
    public float2 TextureOffset;
    public NativeArray<Matrix4x4> Matrices;
}

public struct DynamicRenderComponent : IComponent
{
    public int MeshType;
    public int MaterialType;
    public float2 TextureOffset;
    public NativeArray<Matrix4x4> Matrices;
}

public struct QuadTreeLeafComponent : IComponent
{
    public int LeafID;
    public Rect Rect;
}

public struct MoverComponent : IComponent
{
    public bool HasPath;
    public int PathStepNumber;
    public NativeArray<int2> Path;
}

public struct SoldierComponent : IComponent
{

    public SoldierType SoldierType;
}

public struct CoordinateComponent : IComponent
{
    public int2 Coordinate;
}


