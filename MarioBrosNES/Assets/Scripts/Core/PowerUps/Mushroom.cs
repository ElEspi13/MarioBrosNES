using UnityEngine;

/// <summary>
/// Power-up Mushroom: se desplaza horizontalmente, cambia de dirección al chocar.
/// </summary>
public class Mushroom : PowerUpBase
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayDistance = 0.1f;

    private Vector2 direction = Vector2.right;

    /// <summary>
    /// Mueve el hongo horizontalmente aplicando su velocidad y comprobando paredes.
    /// </summary>
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
        CheckWall();
    }

    /// <summary>
    /// Realiza un raycast hacia la dirección de movimiento y cambia de dirección si hay un obstáculo.
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
    /// Detecta colisión con el jugador y aplica el power-up de tipo Mushroom.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect(collision.gameObject, PowerUpType.Mushroom);
        }
    }
}


