using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

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
        isMovingShell = false;
        rb.velocity = Vector2.zero;
        wakeUpCounter = wakeUpTime;
    }



    private void LaunchShell()
    {
        isMovingShell = true;

        GameObject mario = GameObject.FindGameObjectWithTag("Player");

        float direction = transform.position.x < mario.transform.position.x ? -1f : 1f;

        rb.velocity = new Vector2(direction * shellSpeed, rb.velocity.y);
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

    

}
