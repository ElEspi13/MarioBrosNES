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
    }

        private IEnumerator DeathRoutine()
        {
            yield return new WaitForSeconds(0.5f);
            Destroy(gameObject);
    }

}
