using UnityEngine;

public class TileOccupancySystem : IInitSystem
{

    private ECSWorld _world;

    public void Init(SystemManager systemManager)
    {
        _world = systemManager.GetWorld();
    }

    public void SetTileOccupant(CoordinateComponent coordinateComponent, int moverindex)
    {
        Vector2 pos = new Vector2(coordinateComponent.Coordinate.x, coordinateComponent.Coordinate.y);
        Chunk chunk = _world.ChunkContainers[(ushort)(ComponentMask.CoordinateComponent | ComponentMask.TileComponent)][QuerySystem.GetChunkId(
                                                                                                             _world.ChunkContainers[(ushort)(ComponentMask.CoordinateComponent | ComponentMask.TileComponent)],
                                                                                                             _world.ChunkContainers[(ushort)(ComponentMask.QuadTreeLeafComponent)][0],
                                                                                                             _world.quadTreeNodeDatas,
                                                                                                             _world.QuadtreeNodeIndexes,
                                                                                                             _world.QuadtreeLeafIndexes,
                                                                                                             _world.TileQuadtreeRoot,
                                                                                                             pos)];
        int tileIndex = QuerySystem.SearchTileInChunk(chunk, coordinateComponent.Coordinate);
        ChunkUtility.GetEntityComponentValueAtIndex<TileComponent>(chunk, tileIndex).MoverIndex = moverindex;
        //chunk.GetEntityComponentValueAtIndex<TileComponent>(tileIndex).MoverIndex = moverindex;



    }

    public int GetTileOccupant(CoordinateComponent coordinateComponent)
    {
        Vector2 pos = new Vector2(coordinateComponent.Coordinate.x, coordinateComponent.Coordinate.y);
        Chunk chunk = _world.ChunkContainers[(ushort)(ComponentMask.CoordinateComponent | ComponentMask.TileComponent)][QuerySystem.GetChunkId(
                                                                                                             _world.ChunkContainers[(ushort)(ComponentMask.CoordinateComponent | ComponentMask.TileComponent)],
                                                                                                             _world.ChunkContainers[(ushort)(ComponentMask.QuadTreeLeafComponent)][0],
                                                                                                             _world.quadTreeNodeDatas,
                                                                                                             _world.QuadtreeNodeIndexes,
                                                                                                             _world.QuadtreeLeafIndexes,
                                                                                                             _world.TileQuadtreeRoot,
                                                                                                             pos)];
        int tileIndex = QuerySystem.SearchTileInChunk(chunk, coordinateComponent.Coordinate);
        //TileComponent tileComponent = chunk.GetEntityComponentValueAtIndex<TileComponent>(tileIndex);
        TileComponent tileComponent = ChunkUtility.GetEntityComponentValueAtIndex<TileComponent>(chunk, tileIndex);
        return tileComponent.MoverIndex;
    }


}
