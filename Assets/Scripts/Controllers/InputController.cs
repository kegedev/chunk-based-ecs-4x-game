using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

public class InputController : IInitSystem,IUpdateSystem
{
    [SerializeField] private Camera _camera;
    [SerializeField] private CameraController _cameraController;
    private ECSWorld _world;


    public InputController(Camera camera)
    {
        _camera = camera;
        _cameraController=camera.GetComponent<CameraController>();
    }
    public void Init(SystemManager systemManager)
    {
        _world=systemManager.GetWorld();
    }
    public void Update(SystemManager systemManager)
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        _cameraController.MoveCamera(new Vector2(horizontal, vertical));

        if (Input.mouseScrollDelta.y != 0)
        {
            _cameraController.Zoom(Input.mouseScrollDelta.y);
        }
        if (Input.GetMouseButtonUp(0))
        {

            GetClickPositionOnXZPlane(Input.mousePosition);
        }
    }


    public void GetClickPositionOnXZPlane(Vector3 screenPosition)
    {
        Vector3 worldPositionNear = _camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, _camera.nearClipPlane));
        Vector3 worldPositionFar = _camera.ScreenToWorldPoint(new Vector3(screenPosition.x, screenPosition.y, _camera.farClipPlane));


        Vector3 direction = worldPositionFar - worldPositionNear;


        float distance = -worldPositionNear.y / direction.y;
        Vector3 intersection = (worldPositionNear + direction * distance)+new Vector3(0.5f,0,0.5f);

        QuerySystem.ClickedOnMap(
            _world.ChunkContainers[(ushort)(ComponentMask.CoordinateComponent | ComponentMask.TileComponent)],
            _world.ChunkContainers[(ushort)(ComponentMask.QuadTreeLeafComponent)][0], 
            _world.quadTreeNodeDatas,
            _world.QuadtreeNodeIndexes,
            _world.QuadtreeLeafIndexes, 
            _world.TileQuadtreeRoot,
            intersection);
        //trigger click action by intersection
    }


}
