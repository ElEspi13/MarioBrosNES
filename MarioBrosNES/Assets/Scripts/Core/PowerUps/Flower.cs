using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Power-up Flower: al colisionar con el jugador aplica la FireFlower.
/// </summary>
public class Flower : PowerUpBase
{
    /// <summary>
    /// Detecta colisión con el jugador y aplica el power-up de tipo FireFlower.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            base.Collect(collision.gameObject, PowerUpType.FireFlower);
        }
    }
}
