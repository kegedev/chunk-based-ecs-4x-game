using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    ECSWorld world;
    SystemManager systemManager;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 500;
        world =new ECSWorld();
        systemManager=new SystemManager(world); 
    

        PoolManager poolManager = new PoolManager();

        FactoryManager factoryManager = new FactoryManager(poolManager);

        MapSettings.Initialize(128,128,1,8);


        systemManager.AddSharedData(new PerlinNoiseSettings
        {
            Scale=10f,
            HeightMultiplier= 1500f,
            OffsetX= 100,
            OffsetY=0,
            SeaLevel= 0.58f,
            SandLevel= 0.65f,
            GrassLevel= 0.8f,
            ForestLevel= 0.9f,
        });

 

        InputController inputController = new InputController(_camera);
        AStarSystem aStarSystem = new AStarSystem();
        SelectionSystem selectionSystem = new SelectionSystem();
        MovementSystem movementSystem = new MovementSystem();
  
        selectionSystem.GetMoverPath += aStarSystem.GetMoverPath;
       QuerySystem.ProcessSelection += selectionSystem.ProcessSelection;
        

        MoverCreationSystem moverCreationSystem = new MoverCreationSystem(factoryManager);
        TileOccupancySystem tileOccupancySystem= new TileOccupancySystem();

        moverCreationSystem.SetOccupant += tileOccupancySystem.SetTileOccupant;
        moverCreationSystem.GetOccupant += tileOccupancySystem.GetTileOccupant;
        movementSystem.SetTileOccupant += tileOccupancySystem.SetTileOccupant;
        RenderSystem renderSystem = new RenderSystem();
        movementSystem.UpdateRenderMatrix += renderSystem.UpdateMoverRenderComponentMatrix;
        systemManager.AddSystem(new AssetLoadingSystem());

        systemManager.AddSystem(new PerlinNoise());
        systemManager.AddSystem(new TileCreationSystem(factoryManager));
        systemManager.AddSystem(new QuadtreeCreationSystem(factoryManager));


        systemManager.AddSystem(inputController);
        systemManager.AddSystem(tileOccupancySystem);
        systemManager.AddSystem(moverCreationSystem);
        systemManager.AddSystem(aStarSystem);
        systemManager.AddSystem(selectionSystem);


        systemManager.AddSystem(new RenderChunkCreationSystem());

        systemManager.AddSystem(renderSystem);
        systemManager.AddSystem(movementSystem);



        systemManager.Init();

  


    }

    private void Update()
    {
        systemManager.UpdateSystems();
    }


    private void OnDrawGizmos()
    {
       
        //    DrawNode(world.QuadtreeRoots[ComponentMask.TileComponentMask]);
        //foreach (var chunk in world.ChunkContainers[ComponentMask.TileComponentMask].Chunks)
        //{
        //     foreach (var item in chunk.GetAllComponents<CoordinateComponent>())
        //{
        //    Gizmos.DrawWireCube(
        //      new Vector3((float)item.Coordinate.x+0.5f , 0, (float)item.Coordinate.y + 0.5f),
        //      new Vector3(1, 0, 1)
        //      );
        //}
        //}
       
    }

    private void DrawNode(QuadTreeNodeData node)
    {
        //// Draw the boundary of the node
        //Gizmos.color = Color.green;
        //Gizmos.DrawWireCube(
        //    new Vector3(node.Rect.x + node.Rect.width / 2, 0, node.Rect.y + node.Rect.height / 2),
        //    new Vector3(node.Rect.width, 0, node.Rect.height)
        //);

        //// Draw leaves if any
        //Gizmos.color = Color.red;
        //foreach (var leafId in node.Leaves)
        //{
        //    // For visualization, assume leaf data includes position info
        //    // Replace with actual position retrieval logic if necessary
        //    Gizmos.DrawSphere(new Vector3(node.Rect.x + node.Rect.width / 2, 0, node.Rect.y + node.Rect.height / 2), 0.5f);
        //}

        //// Recursively draw child nodes
        //if (node.IsDivided)
        //{
        //    foreach (var childNode in node.Nodes)
        //    {
                
        //            DrawNode(childNode);
                
        //    }
        //}
        //else
        //{
        //    foreach (var leaf in node.Leaves)
        //    {

        //        Gizmos.color = Color.red;
        //        ChunkUtility.GetAllComponents<CoordinateComponent>(world.ChunkContainers[(ushort)ComponentMask.TileComponentMask][leaf]);
        //        //world.ChunkContainers[ComponentMask.TileComponentMask].Chunks[leaf].GetAllComponents<CoordinateComponent>();
                
             

        //    }
        //}
    }

}

/*
 TODOs:
*0.5lik kayma
*tile not found çöz
*
*move quadtreedatas from world
*query system parametre kalabalıgını azalt yukarıdaki maddeden sonra
*add terraincheck to astar and mover creation
*memoryfreed kodları
*nativearray dispose kodları
*memory leak check
*frustum culling
*camera zoom clamp
*readonly yapılacanilecek alanalra bak
*private olabilecek alanlara bak
*public yerine internal kullan
*sealed class yapabileceklerine bak
*
*profiling messageları kaldır
*todoları yaz
*remove unused libraries
*setpass call ve batch sayısını fpsin oraya yazdır
*namespaces
*
*
*hariatayı buyut
*mobilde dene
*job içinde static method çağırmayı araştır
 
 */
