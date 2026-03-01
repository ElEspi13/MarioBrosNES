using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : EnemigoBase
{
    [SerializeField]private Sprite StompedSprite;
    protected override void onStomp()
    {
        GetComponent<Rigidbody2D>().simulated = false;
        GetComponent<Animator>().enabled = false;
        GetComponent<SpriteRenderer>().sprite = StompedSprite;
        StartCoroutine(DeathRoutine());
        GameManager.Instance.AddStompComboPoints();
    }
    /// <summary>
    /// Muerte del Goomba: espera un breve tiempo para mostrar la animación de aplastado antes de destruir el objeto del juego.
    /// </summary>
    /// <returns></returns>
    private IEnumerator DeathRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
    }

}
