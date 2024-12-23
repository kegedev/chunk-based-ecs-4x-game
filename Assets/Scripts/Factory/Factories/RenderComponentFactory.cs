using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using static UnityEngine.Terrain;

public class RenderComponentFactory : BaseFactory<RenderComponent>
{
    protected override RenderComponent Create(params object[] arguments)
    {
        return new RenderComponent() { 
            MeshType = (int)arguments[0] ,
            MaterialType = (int)arguments[1] ,
            Matrices=(NativeArray<Matrix4x4>)arguments[2] 
        };
    }

    protected override RenderComponent Update(ref RenderComponent renderComponent, params object[] arguments)
    {
        renderComponent.MeshType = (int)arguments[0];
            renderComponent.MaterialType = (int)arguments[1];
            renderComponent.Matrices = (NativeArray<Matrix4x4>)arguments[2];
        return renderComponent;
    }
}
