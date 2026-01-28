using UnityEngine;

public class Mushroom : PowerUpBase
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayDistance = 0.5f;

    private Vector2 direction = Vector2.right;

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(direction.x * speed, rb.velocity.y);
        CheckWall();
    }

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect(collision.gameObject, PowerUpType.Mushroom);
        }
    }
}


