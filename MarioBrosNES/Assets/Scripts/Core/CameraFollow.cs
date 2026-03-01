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

    /// <summary>
    /// Maneja la asignación del objetivo de seguimiento para la cámara, actualizando la posición inicial de la cámara en función de la posición del jugador y estableciendo el límite máximo de desplazamiento horizontal para evitar que la cámara retroceda.
    /// </summary>
    /// <param name="newPlayer"></param>
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

    /// <summary>
    /// Mueve la camara para seguir al jugador solo en la dirección horizontal (eje X), asegurando que la cámara no retroceda por detrás del jugador al actualizar su posición cada frame. Si el jugador se mueve hacia adelante, la cámara se desplazará para mantenerlo dentro de la vista, pero si el jugador retrocede, la cámara permanecerá en su posición actual sin retroceder.
    /// </summary>
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
