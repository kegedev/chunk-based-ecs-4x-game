using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierComponentFactory : BaseFactory<SoldierComponent>
{
    protected override SoldierComponent Create(params object[] arguments)
    {
        return new SoldierComponent() { SoldierType = (SoldierType)arguments[0] };
    }

    protected override SoldierComponent Update(ref SoldierComponent component, params object[] arguments)
    {
        component.SoldierType = (SoldierType)arguments[0];
        return component;
    }
}
