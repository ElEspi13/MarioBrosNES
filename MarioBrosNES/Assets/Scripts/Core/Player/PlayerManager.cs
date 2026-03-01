using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

/// <summary>
/// Maneja el estado del jugador (Small, Super, Fire), sprites y lógica de power-ups.
/// </summary>
public class PlayerManager : MonoBehaviour
{
    /// <summary>
    /// Estados posibles del jugador.
    /// </summary>
    public enum PlayerState { Small, Super, Fire }
    public PlayerState state = PlayerState.Small;

    [SerializeField] private Animator animator;

    private SpriteRenderer _sr;
    private CapsuleCollider2D _cc;
    private bool isInvulnerable = false;
    private bool isTransforming = false;
    private bool isStarActive = false;

    /// <summary>
    /// Obtiene referencias a SpriteRenderer y BoxCollider2D usados para cambiar apariencia y colisión.
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(this);
        _sr = GetComponent<SpriteRenderer>();
        _cc = GetComponent<CapsuleCollider2D>();
    }

    /// <summary>
    /// Aplica el efecto de un power-up sobre el jugador.
    /// </summary>
    /// <param name="type">Tipo de power-up a aplicar</param>
    public void ApplyPowerUp(PowerUpType type)
    {
        switch (type)
        {
            case PowerUpType.Mushroom:
                if (state == PlayerState.Small)
                StartCoroutine(IsChangingState(PlayerState.Super));
                break;
            case PowerUpType.FireFlower:
                if (state != PlayerState.Fire) 
                StartCoroutine(IsChangingState(PlayerState.Fire));
                break;
            case PowerUpType.Star:
                ActivateStar(10f);
                break;
            case PowerUpType.Up_1:
                GameManager.Instance.AddLife(1);
                break;
        }
    }

    /// <summary>
    /// Cambia el estado del jugador y actualiza sprite y collider.
    /// </summary>
    private void ChangeState(PlayerState newState)
    {
        state = newState;

        switch (state)
        {
            case PlayerState.Small:
                _cc.size = new Vector2(0.7f, 1f);
                transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
                animator.SetInteger("MarioState", 0);


                break;

            case PlayerState.Super:
                _cc.size = new Vector2(0.7f, 2f);
                animator.SetInteger("MarioState", 1);

                break;

            case PlayerState.Fire:
                _cc.size = new Vector2(0.7f, 2f);
                animator.SetInteger("MarioState", 2);

                break;

        }
    }

    /// <summary>
    /// Procesa daño recibido y cambia el estado del jugador en consecuencia.
    /// </summary>
    public void TakeDamage()
    {
        if (isInvulnerable) return;

        if (state == PlayerState.Fire) StartCoroutine(DamageSequence(PlayerState.Super));
        else if (state == PlayerState.Super) StartCoroutine(DamageSequence(PlayerState.Small));
        else if (state == PlayerState.Small)
        {
            Die();
            return; 
        }
        
    }

    private IEnumerator DamageSequence(PlayerState newState)
    {
            yield return StartCoroutine(IsChangingState(newState));
            yield return StartCoroutine(TemporaryInvulnerability(3f));
    }

    /// <summary>
    /// Metodo para invulnerabilidad temporal: activa un estado de invulnerabilidad durante una duración determinada, alternando la visibilidad del sprite para indicar el estado y desactivando las colisiones con enemigos durante ese tiempo.
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator TemporaryInvulnerability(float duration)
    {
        isInvulnerable = true;

        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemigos");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        float blinkInterval = 0.1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            _sr.enabled = !_sr.enabled;
            yield return new WaitForSeconds(blinkInterval);
            elapsed += blinkInterval;
        }

        _sr.enabled = true;

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        isInvulnerable = false;
    }

    /// <summary>
    /// Metodo para manejar la transición entre estados del jugador con animación y pausa temporal para efecto visual.
    /// </summary>
    /// <param name="newState"></param>
    /// <returns></returns>
    private IEnumerator IsChangingState(PlayerState newState)
    {
        isTransforming = true;

        Time.timeScale = 0f;

        animator.updateMode = AnimatorUpdateMode.UnscaledTime;

        animator.SetBool("IsChanging", true);
        animator.SetInteger("PreviousState", (int)state);

        state = newState;
        animator.SetInteger("MarioState", (int)state);

        animator.SetTrigger("MarioChanged");

        yield return new WaitForSecondsRealtime(0.6f);

        animator.SetBool("IsChanging", false);
        ChangeState(newState);
        Time.timeScale = 1f;
        animator.updateMode = AnimatorUpdateMode.Normal;

        isTransforming = false;
    }
    /// <summary>
    /// Lógica de muerte del jugador (puede ampliarse para efectos, reinicios o vidas).
    /// </summary>
    private void Die()
    {
        GetComponent<PlayerController>().Die();
    }

    /// <summary>
    /// Activa el estado de estrella por una duración determinada, otorgando invulnerabilidad.
    /// </summary>
    /// <param name="duration"></param>
    public void ActivateStar(float duration)
    {
        if (!isStarActive)
            StartCoroutine(StarRoutine(duration));
    }

    private IEnumerator StarRoutine(float duration)
    {
        isStarActive = true;
        animator.SetBool("IsStarActive", true);
        yield return new WaitForSeconds(duration);

        isStarActive = false;
        animator.SetBool("IsStarActive", false);
    }

    /// <summary>
    /// Metod para verificar si el estado de estrella está activo, utilizado por enemigos para determinar si deben morir al contacto o no.
    /// </summary>
    /// <returns></returns>
    public bool IsStarActive()
    {
        return isStarActive;
    }

}

