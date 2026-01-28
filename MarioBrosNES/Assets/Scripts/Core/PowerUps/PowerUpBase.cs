using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    Mushroom,
    FireFlower,
    Star
}
public class PowerUpBase : MonoBehaviour
{
    protected Collider2D col;
    protected Rigidbody2D rb;


    private void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }

    public void EmergeFromBlock()
    {
        col.enabled = false;      
        rb.simulated = false;    
    }

    public virtual void FinishEmerging()
    {
        col.enabled = true;     
        rb.simulated = true;    
    }

    protected void Collect(GameObject player, PowerUpType type)
    {

        player.GetComponent<PlayerManager>().ApplyPowerUp(type);

        
        rb.simulated = false;
        col.isTrigger= false;

        
        Destroy(gameObject);
    }
    
}
