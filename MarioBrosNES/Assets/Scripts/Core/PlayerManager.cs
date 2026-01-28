using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public enum PlayerState { Small, Super, Fire }
    public PlayerState state = PlayerState.Small;

    public Sprite smallSprite;
    public Sprite superSprite;
    public Sprite fireSprite;

    private SpriteRenderer sr;
    private BoxCollider2D bc;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        bc = GetComponent<BoxCollider2D>();
    }

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
        }
    }

    private void ChangeState(PlayerState newState)
    {
        state = newState;

        switch (state)
        {
            case PlayerState.Small:
                sr.sprite = smallSprite;
                bc.size = new Vector2(0.6f, 1f);
                break;

            case PlayerState.Super:
                sr.sprite = superSprite;
                bc.size = new Vector2(0.6f, 2f);
                break;

            case PlayerState.Fire:
                sr.sprite = fireSprite;
                bc.size = new Vector2(0.6f, 2f);
                break;
        }
    }

    public void TakeDamage()
    {
        if (state == PlayerState.Fire) state = PlayerState.Super;
        else if (state == PlayerState.Super) state = PlayerState.Small;
        else if (state == PlayerState.Small) Die();
    }
    private void Die()
    {
        // Handle player death logic here
        Debug.Log("Player has died.");
    }
}

