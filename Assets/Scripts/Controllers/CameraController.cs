using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] private float MovementSpeed;

    public void MoveCamera(Vector2 input)
    {
        Vector3 moveDirection = new Vector3(input.x, 0, input.y);
        transform.position += moveDirection * MovementSpeed * Time.deltaTime;
    }

    public void Zoom(float scrollDelta)
    {
        Vector3 zoomDirection = transform.forward * scrollDelta;
        transform.position += zoomDirection * Time.deltaTime * 50f;
    }

    
}
