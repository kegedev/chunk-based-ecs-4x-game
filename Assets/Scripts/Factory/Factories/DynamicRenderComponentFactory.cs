using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class DynamicRenderComponentFactory : BaseFactory<DynamicRenderComponent>
{
    protected override DynamicRenderComponent Create(params object[] arguments)
    {
        return new DynamicRenderComponent()
        {
            MeshType = (int)arguments[0],
            MaterialType = (int)arguments[1],
            Matrices = (NativeArray<Matrix4x4>)arguments[2]
        };
    }

    protected override DynamicRenderComponent Update(ref DynamicRenderComponent renderComponent, params object[] arguments)
    {
        renderComponent.MeshType = (int)arguments[0];
        renderComponent.MaterialType = (int)arguments[1];
        renderComponent.Matrices = (NativeArray<Matrix4x4>)arguments[2];
        return renderComponent;
    }
}
