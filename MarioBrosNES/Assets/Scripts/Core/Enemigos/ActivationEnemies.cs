using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationEnemies : MonoBehaviour
{
    /// <summary>
    /// Metodos de activacion de enemigos, se activa cuando el enemigo entra en el trigger.
    /// </summary>
    /// <param name="other">Collider del enemigo</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemigo = other.GetComponent<EnemigoBase>();
            if (enemigo != null)
            {
                enemigo.isActive = true;
            }
        }
    }

    /// <summary>
    /// Metodo de desactivacion de enemigos, se activa cuando el enemigo sale del trigger.
    /// </summary>
    /// <param name="other">Collider del enemigo</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            var enemigo = other.GetComponent<EnemigoBase>();
            if (enemigo != null)
            {
                enemigo.isActive = false;
            }
        }
    }
}
