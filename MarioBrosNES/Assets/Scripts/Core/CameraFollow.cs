using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;   
    public float offsetX = 0f;  

    private float maxCameraX;

    void Start()
    {
        maxCameraX = transform.position.x;
    }

    public void SetTarget(Transform newPlayer)
    {
        player = newPlayer;
        maxCameraX = player.position.x + 7.5f;
        transform.position = new Vector3(
            maxCameraX,
            transform.position.y,
            transform.position.z
        );
    }


    void LateUpdate()
    {
        if(player== null) return;
        float targetX = player.position.x + offsetX;

        if (targetX > maxCameraX)
        {
            maxCameraX = targetX;
        }

        transform.position = new Vector3(
            maxCameraX,
            transform.position.y,
            transform.position.z
        );
    }
}
