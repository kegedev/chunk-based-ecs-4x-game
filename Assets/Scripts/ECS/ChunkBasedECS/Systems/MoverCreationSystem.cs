using System;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class MoverCreationSystem : IInitSystem
{
    private FactoryManager _factoryManager;

    public Action<CoordinateComponent,int> SetOccupant;
    public Func<CoordinateComponent,int> GetOccupant;
    public MoverCreationSystem(FactoryManager factoryManager)
    {
        _factoryManager = factoryManager;
    }
    public void Init(SystemManager systemManager)
    {
        CreateSoldierChunks(systemManager.GetWorld(), (ComponentMask.CoordinateComponent | ComponentMask.SoldierComponent | ComponentMask.MoverComponent));
    }

    public void CreateSoldierChunks(ECSWorld eCSWorld, ComponentMask componentMask)
    {

        for (int i = 0; i < 1024; i++)
        {
            int xPos = UnityEngine.Random.Range(0, 128);
            int yPos = UnityEngine.Random.Range(0, 128);

         CoordinateComponent coordinateComponent = _factoryManager.GetInstance<CoordinateComponent>(new int2(xPos, yPos));

            //_mapController.SetTileOccupancy((Vector3)cubePos, OccupancyType.Soldier);

            eCSWorld.AddEntity(componentMask, 1024, new IComponent[3]
                            {
                            coordinateComponent,
                            _factoryManager.GetInstance<SoldierComponent>(SoldierType.Swordman),
                            _factoryManager.GetInstance<MoverComponent>(new object[] {false,0,new NativeArray<int2>()})
                            });
          //OPEN
            SetOccupant.Invoke(coordinateComponent, eCSWorld.ChunkContainers[(ushort)componentMask][0].EntityCount-1);
        }

    }
}
