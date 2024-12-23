using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationComponentFactory : BaseFactory<RotationComponent>
{
    protected override RotationComponent Create(params object[] arguments)
    {
        return new RotationComponent() { Quaternion = (Quaternion)arguments[0] };
    }

    protected override RotationComponent Update(ref RotationComponent rotationComponent, params object[] arguments)
    {
        rotationComponent.Quaternion = (Quaternion)arguments[0];
        return rotationComponent;
    }
}
