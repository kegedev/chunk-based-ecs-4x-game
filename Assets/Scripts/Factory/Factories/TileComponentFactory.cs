using UnityEngine;
using UnityEngine.Tilemaps;

public class TileComponentFactory : BaseFactory<TileComponent>
{
    protected override TileComponent Create(params object[] arguments)
    {
        return new TileComponent() { TerrainType = (TerrainType)arguments[0] ,
                                     MoverIndex = (int)arguments[1]
                                    };
    }

    protected override TileComponent Update(ref TileComponent tileComponent, params object[] arguments)
    {
        tileComponent.TerrainType = (TerrainType)arguments[0];
        tileComponent.MoverIndex = (int)arguments[1];
        return tileComponent;
    }
}
