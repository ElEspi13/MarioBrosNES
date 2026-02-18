using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private Sprite smallSprite;
    [SerializeField] private Sprite superSprite;
    [SerializeField] private Sprite fireSprite;
    [SerializeField] private Animator animator;

    private SpriteRenderer _sr;
    private CapsuleCollider2D _cc;
    private bool isInvulnerable = false;

    /// <summary>
    /// Obtiene referencias a SpriteRenderer y BoxCollider2D usados para cambiar apariencia y colisión.
    /// </summary>
    private void Awake()
    {
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
                if (state == PlayerState.Small) state = PlayerState.Super;
                ChangeState(PlayerState.Super);
                break;
            case PowerUpType.FireFlower:
                if (state != PlayerState.Fire) state = PlayerState.Fire;
                ChangeState(PlayerState.Fire);
                break;
            case PowerUpType.Star:
                // Activate invincibility logic here
                break;
            case PowerUpType.Up_1:
                // Metodo para vidas extra
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
                _sr.sprite = smallSprite;
                _cc.size = new Vector2(0.7f, 1f);
                transform.position = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);
                animator.SetInteger("MarioState", 0);
                animator.SetTrigger("MarioChanged");
                break;

            case PlayerState.Super:
                _sr.sprite = superSprite;
                _cc.size = new Vector2(0.7f, 2f);
                animator.SetInteger("MarioState", 1);
                animator.SetTrigger("MarioChanged");
                break;

            case PlayerState.Fire:
                _sr.sprite = fireSprite;
                _cc.size = new Vector2(0.7f, 2f);
                animator.SetInteger("MarioState", 2);
                animator.SetTrigger("MarioChanged");
                break;

        }
    }

    /// <summary>
    /// Procesa daño recibido y cambia el estado del jugador en consecuencia.
    /// </summary>
    public void TakeDamage()
    {
        if (isInvulnerable) return;

        if (state == PlayerState.Fire) ChangeState(PlayerState.Super);
        else if (state == PlayerState.Super) ChangeState(PlayerState.Small);
        else if (state == PlayerState.Small)
        {
            Die();
            return; 
        }


        StartCoroutine(TemporaryInvulnerability(5f));
    }

    private IEnumerator TemporaryInvulnerability(float duration)
    {
        isInvulnerable = true;


        int playerLayer = LayerMask.NameToLayer("Player");
        int enemyLayer = LayerMask.NameToLayer("Enemigos");
        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        

        yield return new WaitForSeconds(duration);

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);
        isInvulnerable = false;
    }
    /// <summary>
    /// Lógica de muerte del jugador (puede ampliarse para efectos, reinicios o vidas).
    /// </summary>
    private void Die()
    {

        Debug.Log("Player has died.");
    }
}

