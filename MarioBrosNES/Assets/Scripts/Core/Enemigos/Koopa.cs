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

    private void StopShell()
    {
        canMove = false;
        isMovingShell = false;
        rb.velocity = Vector2.zero;
        wakeUpCounter = wakeUpTime;
    }



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

    private IEnumerator EnableHurtAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        canKill = true;
    }



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

    private void ExitShell()
    {
        isShell = false;
        isMovingShell = false;

        animator.enabled = true;
        col.offset = new Vector2(0, -0.5f);


    }

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

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision); 


        if (isShell && isMovingShell && collision.collider.CompareTag("Enemy"))
        {
            EnemigoBase enemy = collision.collider.GetComponent<EnemigoBase>();
            if (enemy != null && enemy != this)
            {
                enemy.Die();
            }
        }
    }



}
