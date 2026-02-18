using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform player;   
    public float offsetX = 0f;  

    private float maxCameraX;

    void Start()
    {
        maxCameraX = transform.position.x;
    }

    void LateUpdate()
    {
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
