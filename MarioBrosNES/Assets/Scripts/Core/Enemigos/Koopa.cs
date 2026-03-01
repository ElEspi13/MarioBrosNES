using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Koopa : EnemigoBase
{
    [Header("Sprites")]
    [SerializeField] private Sprite shellSprite;

    [Header("Configuración")]
    [SerializeField] private float shellSpeed = 8f;
    [SerializeField] private float wakeUpTime = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sr;
    private BoxCollider2D col;

    private bool isShell = false;
    private bool isMovingShell = false;
    private float wakeUpCounter;
    private bool canKill = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Metodo que maneja el comportamiento específico al ser pisado por el jugador: entrar en caparazón, detenerse o lanzarse dependiendo del estado actual.
    /// </summary>
    protected override void onStomp()
    {
        if (!isShell)
        {
            EnterShell();
        }
        else
        {
            if (isMovingShell)
            {
                StopShell();
            }
            else
            {
                LaunchShell();
            }
        }
    }


    /// <summary>
    /// Metodo para cambiar el estado del Koopa a caparazón: detiene el movimiento, cambia la apariencia y prepara el contador para despertar después de un tiempo.
    /// </summary>
    private void EnterShell()
    {
        isShell = true;
        isMovingShell = false;
        col.offset = new Vector2(0,0);

        canMove = false;

        rb.velocity = Vector2.zero;
        animator.enabled = false;
        sr.sprite = shellSprite;

        wakeUpCounter = wakeUpTime;
    }

    /// <summary>
    /// Metodo para detener el caparazón en movimiento: detiene la velocidad, resetea el estado de movimiento y reinicia el contador para despertar.
    /// </summary>
    private void StopShell()
    {
        canMove = false;
        isMovingShell = false;
        rb.velocity = Vector2.zero;
        wakeUpCounter = wakeUpTime;
    }


    /// <summary>
    /// Metodo para lanzar el caparazón: determina la dirección opuesta al jugador, establece la velocidad y activa el estado de movimiento. También inicia una rutina para permitir que el caparazón pueda dañar después de un retraso.
    /// </summary>
    private void LaunchShell()
    {
        canMove = true;
        isMovingShell = true;
        canKill = false;

        GameObject mario = GameObject.FindGameObjectWithTag("Player");

        Vector2 direction = new Vector2(
            transform.position.x < mario.transform.position.x ? -1f : 1f,
            0f
        );

        ChangeSpeed(shellSpeed);
        ChangeDirection(direction);

        StartCoroutine(EnableHurtAfterDelay());
    }

    /// <summary>
    /// Metodo para habilitar la capacidad de dañar del caparazón después de un retraso, evitando que el jugador sea dañado inmediatamente al lanzar el caparazón.
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnableHurtAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        canKill = true;
    }


    /// <summary>
    /// Metodo de actualización que maneja el conteo para despertar del caparazón: si el Koopa está en estado de caparazón pero no se está moviendo, decrementa el contador y llama a ExitShell() cuando llegue a cero para volver al estado normal.
    /// </summary>
    private void Update()
    {
        if (isShell && !isMovingShell)
        {
            wakeUpCounter -= Time.deltaTime;

            if (wakeUpCounter <= 0)
            {
                ExitShell();
            }
        }
    }

    /// <summary>
    /// Metodo para salir del estado de caparazón: resetea el estado a normal, reactiva el movimiento, habilita el animator y ajusta el collider para volver a la forma original del Koopa.
    /// </summary>
    private void ExitShell()
    {
        isShell = false;
        isMovingShell = false;
        canMove = true;
        animator.enabled = true;
        col.offset = new Vector2(0, -0.5f);


    }

    /// <summary>
    /// Metodo que maneja la colisión con el jugador: si el jugador tiene una estrella activa, el Koopa muere. Si el Koopa es un caparazón en movimiento, solo daña al jugador si la colisión ocurre desde los lados. Si el Koopa es un caparazón detenido, se lanza en la dirección opuesta al jugador. En cualquier otro caso, el jugador recibe daño.
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnPlayerCollision(Collision2D collision)
    {
        PlayerManager player = collision.collider.GetComponent<PlayerManager>();
        if (player == null) return;

        if (player.IsStarActive())
        {
            Die();
            return;
        }

        if (isShell && isMovingShell)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.x != 0) 
                {
                    player.TakeDamage();
                    break;
                }
            }
            return; 
        }

        if (isShell && !isMovingShell)
        {
            Rigidbody2D playerRb = collision.collider.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                LaunchShell(); 
            }
            return; 
        }

        player.TakeDamage();
    }

    /// <summary>
    /// Metodo que maneja la colisión con otros enemigos: si el Koopa es un caparazón en movimiento y colisiona con un enemigo, ese enemigo muere y se otorgan puntos de combo por caparazón. Esto permite que el caparazón en movimiento pueda eliminar a otros enemigos, incentivando al jugador a usar esta mecánica para obtener más puntos.
    /// </summary>
    /// <param name="collision"></param>
    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision); 


        if (isShell && isMovingShell && collision.collider.CompareTag("Enemy"))
        {
            EnemigoBase enemy = collision.collider.GetComponent<EnemigoBase>();
            if (enemy != null && enemy != this)
            {
                enemy.Die();
                GameManager.Instance.AddShellComboPoints();
            }
        }
    }



}
