using UnityEngine;

public class PositionComponentFactory : BaseFactory<PositionComponent>
{
    protected override PositionComponent Create(params object[] arguments)
    {
        return new PositionComponent() { Position = (Vector3)arguments[0] };
    }

    protected override PositionComponent Update(ref PositionComponent positionComponent,params object[] arguments)
    {
        positionComponent.Position = (Vector3)arguments[0];
        return positionComponent;
    }
}
