using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleComponentFactory : BaseFactory<ScaleComponent>
{
    protected override ScaleComponent Create(params object[] arguments)
    {
        return new ScaleComponent() { Scale = (Vector3)arguments[0] };
    }

    protected override ScaleComponent Update(ref ScaleComponent scaleComponent, params object[] arguments)
    {
        scaleComponent.Scale = (Vector3)arguments[0];
        return scaleComponent;
    }
}
