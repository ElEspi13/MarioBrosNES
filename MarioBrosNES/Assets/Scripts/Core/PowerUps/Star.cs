using UnityEngine;

/// <summary>
/// Power-up Star: se mueve, rebota y al colisionar otorga invencibilidad (simbolizado por Star).
/// </summary>
public class Star : PowerUpBase
{
    [SerializeField] private float speed = 2.5f;
    [SerializeField] private float jumpForce = 6f;

    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float groundRayDistance = 0.2f;


    /// <summary>
    /// Empuje inicial del Star en la dirección establecida.
    /// </summary>
    private void Start()
    {
        rb.velocity = new Vector2(Vector2.right.x * speed, rb.velocity.y);
    }
    /// <summary>
    /// Movimiento y comportamiento del Star: se desplaza y ajusta salto sobre terreno.
    /// </summary>
    private void FixedUpdate()
    {
        CheckGroundAndJump();
    }

    

    /// <summary>
    /// Detecta si el Star está sobre suelo y aplica un salto automático si procede.
    /// </summary>
    private void CheckGroundAndJump()
    {
        Bounds b = col.bounds;

        Vector2 origin = new Vector2(b.center.x, b.min.y);

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundRayDistance, groundLayer);

        if (hit.collider != null && rb.velocity.y <= 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    /// <summary>
    /// Detecta colisión con el jugador y aplica el power-up de tipo Star.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect(collision.gameObject, PowerUpType.Star);
        }
    }

}
