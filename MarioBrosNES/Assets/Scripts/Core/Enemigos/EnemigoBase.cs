using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoBase : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayDistance = 0.1f;
    [SerializeField] private float bounceForce = 3f;

    protected Vector2 direction = Vector2.left;
    private Rigidbody2D _rb;
    private Collider2D _col;
    
    protected bool canMove = true;
    public bool isActive = false;
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _col = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FixedUpdate()
    {
        if (canMove && isActive)
        {
            _rb.velocity = new Vector2(direction.x * speed, _rb.velocity.y);
            CheckWall();
        }
        

    }

    public void ChangeSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void ChangeDirection(Vector2 newdirection)
    {
        direction = newdirection;
    }


    /// <summary>
    /// Comprueba si hay un obstáculo lateral y cambia la dirección si es necesario.
    /// </summary>
    private void CheckWall()
    {
        Bounds b = _col.bounds;

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
    /// Detecta colisión con el jugador.
    /// </summary>
    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.collider.CompareTag("Player"))
            return;

        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f)
            {
                Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
                if (playerRb != null)
                    playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);

                onStomp();
                return;
            }
        }

        // Si no fue stomp, delegamos al hijo
        OnPlayerCollision(collision);
    }

    protected virtual void OnPlayerCollision(Collision2D collision)
    {
        PlayerManager player = collision.collider.GetComponent<PlayerManager>();
        if (player == null) return;

        if (player.IsStarActive()) // verificamos si la estrella está activa
        {
            Die();
        }
        else
        {
            player.TakeDamage(); // daño normal
        }
    }

    protected virtual void onStomp()
    {
        Destroy(gameObject);
    }

    public void Die()
    {
        Destroy(gameObject);
    }


    private void OnDrawGizmos()
    {
        if (_col == null) return;
        Bounds b = _col.bounds;
        Vector2 origin = new Vector2(
            direction.x > 0 ? b.max.x : b.min.x,
            b.center.y
        );
        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin, origin + direction * rayDistance);
    }

}
    
    

