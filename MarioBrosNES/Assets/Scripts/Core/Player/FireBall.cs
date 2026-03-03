using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class FireBall : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 8f;
    [SerializeField] private float bounceForce = 6f;
    [SerializeField] private float gravityMultiplier = 2f;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 4f;

    [Header("Collision")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float rayDistance = 0.1f;

    private Rigidbody2D _rb;
    private bool _movingRight;

    public void Initialize(bool facingRight)
    {
        _movingRight = facingRight;
    }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void FixedUpdate()
    {
        float direction = _movingRight ? 1f : -1f;
        _rb.velocity = new Vector2(direction * speed, _rb.velocity.y);

        // Gravedad aumentada al caer
        if (_rb.velocity.y < 0)
        {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (gravityMultiplier - 1) * Time.fixedDeltaTime;
        }

        // Raycast lateral para detectar paredes y suelo
        Vector2 origin = transform.position;
        Vector2 rayDir = _movingRight ? Vector2.right : Vector2.left;

        RaycastHit2D hit = Physics2D.Raycast(origin, rayDir, rayDistance, groundLayer);
        if (hit.collider != null)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            Debug.Log(collision.contacts);
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f)
                {
                    _rb.velocity = new Vector2(_rb.velocity.x, bounceForce);
                    return;
                }
            }

            // Si no es suelo (pared), destruir
            Destroy(gameObject);
        }

        // Si golpea enemigo
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemigoBase>()?.Die();
            GameManager.Instance.AddScore(200);
            Destroy(gameObject);
        }
    }
    
}

