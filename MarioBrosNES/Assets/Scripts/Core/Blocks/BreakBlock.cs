using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bloque que puede romperse si el jugador tiene tamaño adecuado o actuar como bloque normal usado.
/// </summary>
public class BreakBlock : BlockBase
{
    [Header("Break Block Settings")]
    [SerializeField] private bool isBreakable;
    [SerializeField] private Sprite blockedSprite;
    [SerializeField] private bool isUsed;
    private PlatformEffector2D platformEffector;


    private void Awake()
    {
        platformEffector = GetComponent<PlatformEffector2D>();
    }
    /// <summary>
    /// Maneja el golpe específico para este tipo de bloque (rompible o usado).
    /// Soporta bloques con contador de monedas: si quedan monedas, delega al comportamiento base.
    /// </summary>
    protected override void OnHit()
    {
        DisablePlatformEffector();

        if (isBreakable)
        {
            if (base.player.state != PlayerManager.PlayerState.Small)
            {
                GetComponent<SpriteRenderer>().sprite = null;
                StartCoroutine(WaitDestroy());
            }
            else
            {
                StartCoroutine(base.HitAnimation());
            }

            return; 
        }

        
        if (isUsed) return;

        if (isCoinBlock && isMultiCoinBlock)
        {
            base.OnHit();

            if (base.multiCoinExpired)
            {
                isUsed = true;
                GetComponent<SpriteRenderer>().sprite = blockedSprite;
            }

            return;
        }
        isUsed = true;
        GetComponent<SpriteRenderer>().sprite = blockedSprite;
        base.OnHit();
    }

    private void DisablePlatformEffector()
    {
        if (platformEffector != null)
        {
            platformEffector.enabled = false;
        }
    }


    /// <summary>
    /// Corrutina: espera un breve intervalo antes de destruir el bloque (efecto de rompimiento).
    /// </summary>
    /// <returns>IEnumerator — invocarse mediante <c>StartCoroutine</c></returns>
    private IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(this.gameObject);
    }

}
