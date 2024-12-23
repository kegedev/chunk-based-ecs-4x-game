
public enum ComponentMask : ushort
{
    PositionComponent=1<<0,
    RotationComponent=1<<1,
    ScaleComponent=1<<2,
    TileComponent=1<<3,
    StaticRenderComponent=1<<4,
    QuadTreeLeafComponent = 1<<5,
    SoldierComponent=1<<6,
    MoverComponent=1<<7,
    CoordinateComponent=1<<8,
    DynamicRenderComponent = 1<<9,
}


public enum TerrainType
{
    None,
    Grass,
    Sand,
    Forest,
    Mountain,
    Sea,


}

public enum SoldierType
{
    Swordman=6,
    Archer=7,
}