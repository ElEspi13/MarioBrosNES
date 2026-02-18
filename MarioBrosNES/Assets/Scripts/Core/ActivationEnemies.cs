using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivationEnemies : MonoBehaviour
{
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
