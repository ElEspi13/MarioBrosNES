using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bloque interrogante que se usa una sola vez y cambia su sprite al ser golpeado.
/// </summary>
public class QuestionBlock : BlockBase
{
    private bool isUsed = false;
    [SerializeField] private Sprite usedSprite;

    private PlatformEffector2D platformEffector;
    private Animator anim;

    private void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
        anim =GetComponent<Animator>();
    }

    /// <summary>
    /// Evita que un bloque ya usado sea procesado nuevamente.
    /// </summary>
    protected override void OnHit()
    {
        DisablePlatformEffector();
        if (isUsed) return;

        if (isCoinBlock && isMultiCoinBlock)
        {
            
            base.OnHit();
            
            if (base.multiCoinExpired)
            {
                isUsed = true;
                anim.enabled = false;
                GetComponent<SpriteRenderer>().sprite = usedSprite;
                
            }

            return;
        }
        isUsed = true;
        GetComponent<SpriteRenderer>().sprite = usedSprite;
        anim.enabled = false;
        base.OnHit();
    }

    private void DisablePlatformEffector()
    {
        if (platformEffector != null)
        {
            platformEffector.enabled = false;
        }
    }



}
