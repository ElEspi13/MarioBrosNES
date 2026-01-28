using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flower : PowerUpBase
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            base.Collect(collision.gameObject, PowerUpType.FireFlower);
        }
    }
}
