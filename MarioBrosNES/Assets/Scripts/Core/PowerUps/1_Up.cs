using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Power-up 1-Up: otorga vida extra al jugador al colisionar y se desplaza horizontalmente.
/// </summary>
public class Up_1 : PowerUpBase
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayDistance = 0.1f;

    private Vector2 direction = Vector2.right;

    /// <summary>
    /// Mueve el 1-Up horizontalmente y comprueba colisiones con muros para cambiar dirección.
    /// </summary>
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
        CheckWall();
    }

    /// <summary>
    /// Comprueba si hay un obstáculo lateral y cambia la dirección si es necesario.
    /// </summary>
    private void CheckWall()
    {
        Bounds b = col.bounds;

        Vector2 origin = new Vector2(
            direction.x > 0 ? b.max.x : b.min.x,
            b.center.y
        );

        RaycastHit2D hit = Physics2D.Raycast(origin, direction, rayDistance, groundLayer);


        if (hit.collider != null)
        {
            direction *= -1;
        }
    }

    /// <summary>
    /// Detecta colisión con el jugador y aplica el power-up de tipo Up_1 (vida extra).
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect(collision.gameObject, PowerUpType.Up_1);
        }
    }
}



