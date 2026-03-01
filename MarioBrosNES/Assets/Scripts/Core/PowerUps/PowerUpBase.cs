using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tipos de power-ups disponibles.
/// </summary>
public enum PowerUpType
{
    Mushroom,
    FireFlower,
    Star,
    Up_1
}
/// <summary>
/// Comportamiento base para power-ups: control de colisión y emergido.
/// </summary>
public class PowerUpBase : MonoBehaviour
{
    protected Collider2D col;
    protected Rigidbody2D rb;


    /// <summary>
    /// Obtiene referencias a Collider2D y Rigidbody2D usadas por el power-up.
    /// </summary>
    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Desactiva colisiones y simulación mientras el power-up emerge del bloque.
    /// </summary>
    public void EmergeFromBlock()
    {
        col.enabled = false;      
        rb.simulated = false;    
    }

    /// <summary>
    /// Reactiva colisiones y simulación cuando el power-up termina de emerger.
    /// </summary>
    public virtual void FinishEmerging()
    {
        col.enabled = true;     
        rb.simulated = true;    
    }

    /// <summary>
    /// Lógica de recolección: aplica el power-up al jugador y destruye el objeto.
    /// </summary>
    /// <param name="player">GameObject del jugador que recoge el power-up</param>
    /// <param name="type">Tipo de power-up a aplicar</param>
    protected void Collect(GameObject player, PowerUpType type)
    {

        player.GetComponent<PlayerManager>().ApplyPowerUp(type);
        GameManager.Instance.AddScore(1000);  

        rb.simulated = false;
        col.isTrigger= false;

        
        Destroy(gameObject);
    }


}
