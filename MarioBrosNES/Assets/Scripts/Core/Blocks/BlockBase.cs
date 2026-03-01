using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Comportamiento genérico de bloque: monedas, power-ups y animación de golpe.
/// </summary>
public class BlockBase : MonoBehaviour
{
    
    [SerializeField] protected bool isCoinBlock;
    [SerializeField] private bool isPowerUpBlock;
    [SerializeField] private bool is1UpBlock;
    [SerializeField] private bool isStarBlock;
    [SerializeField] protected bool isMultiCoinBlock;
    [SerializeField] protected float multiCoinDuration = 5f;

    [SerializeField] private float velocity;
    [SerializeField] private float offsetY;
    [SerializeField] private GameObject CoinPrefab;
    [SerializeField] private GameObject PowerPrefabMush;
    [SerializeField] private GameObject PowerPrefabFlower;
    [SerializeField] private GameObject OneUpPrefab;
    [SerializeField] private GameObject StarPrefab;

    
    protected bool multiCoinActive = false;
    protected bool multiCoinExpired = false;

    private Vector3 positionTarget;
    private Vector3 powerUpTargetPosition;
    private Vector3 coinTargetPosition;
    private Vector3 originalPosition;
    
    private bool isActive = false;

    protected PlayerManager player;

    /// <summary>
    /// Inicializa la referencia al jugador y las posiciones objetivo/originales del bloque.
    /// </summary>
    private void Start()
    {
        this.originalPosition = this.transform.position;
        this.powerUpTargetPosition = transform.position + Vector3.up * 1f;
        ResetTargetBlockPosition();
        ResetTargetCoinPosition();
    }
    /// <summary>
    /// Calcula y resetea las posiciones objetivo para la animación del bloque y la moneda.
    /// </summary>
    private void ResetTargetBlockPosition()
    {
        this.positionTarget = transform.position + Vector3.up * offsetY;

    }

    private void ResetTargetCoinPosition()
    {
        this.coinTargetPosition = transform.position + Vector3.up * 3f;
    }
    /// <summary>
    /// Lógica cuando el bloque es golpeado: otorga monedas o power-ups según configuración.
    /// </summary>
    protected virtual void OnHit()
    {
        if (!isActive)
        {
            if (isCoinBlock && isMultiCoinBlock)
            {
                if (!multiCoinActive && !multiCoinExpired)
                {
                    StartCoroutine(MultiCoinRoutine());
                }

                if (multiCoinActive)
                {
                    StartCoroutine(HitAnimation());
                    GiveCoins();
                    return;
                }

                if (multiCoinExpired)
                {
                    Debug.Log("Block hit detected.");
                    StartCoroutine(HitAnimation());
                    GiveCoins();
                    return;
                }
                
            }
            Debug.Log("Block hit detected.");
            StartCoroutine(HitAnimation());
            if (isCoinBlock && !isMultiCoinBlock)
            {

                GiveCoins();
            }
            else if (isStarBlock)
            {
                GivePowerUp(StarPrefab);

            }
            else if (is1UpBlock)
            {
                GivePowerUp(OneUpPrefab);
            }
            else if (isPowerUpBlock)
            {
                switch (player.state)
                {
                    case PlayerManager.PlayerState.Small:
                        GivePowerUp(PowerPrefabMush);
                        break;
                    case PlayerManager.PlayerState.Super:
                        GivePowerUp(PowerPrefabFlower);
                        break;
                    case PlayerManager.PlayerState.Fire:
                        GivePowerUp(PowerPrefabFlower);
                        break;
                }
            }
            
        }
    }

    /// <summary>
    /// Bloque de monedas múltiples: activa un estado temporal que permite otorgar monedas adicionales al ser golpeado repetidamente durante su duración.
    /// </summary>
    /// <returns></returns>
    protected IEnumerator MultiCoinRoutine()
    {
        multiCoinActive = true;
        Debug.Log("Multi-coin mode activated.");
        float timer = multiCoinDuration;
        while (timer > 0f)
        {
            timer -= Time.deltaTime;
            yield return null;
        }

        multiCoinActive = false;
        multiCoinExpired = true;
        Debug.Log("Multi-coin mode expired.");
    }


    /// <summary>
    /// Detecta la colisión con el jugador y ejecuta la lógica de golpe del bloque.
    /// </summary>
    /// <param name="collision">Collider que interactuó con el bloque</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<PlayerManager>();
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f) 
                {
                    OnHit(); 
                }
            }
        }
    }

    /// <summary>
    /// Corrutina: anima el bloque al ser golpeado (sube y baja) mientras evita reentradas.
    /// </summary>
    /// <returns>IEnumerator — invocarse mediante <c>StartCoroutine</c></returns>
    protected IEnumerator HitAnimation()
    {
        isActive = true;
        do { MoveTowards(); yield return null; } while (transform.position != positionTarget);
        positionTarget = originalPosition;
        do { MoveTowards(); yield return null; } while (transform.position != originalPosition);

        
        ResetTargetBlockPosition();
        isActive = false;
    }

    /// <summary>
    /// Mueve el bloque hacia la posición objetivo principal usando la velocidad configurada.
    /// </summary>
    private void MoveTowards()
    {
        this.transform.position = Vector3.MoveTowards(transform.position,positionTarget,this.velocity * Time.deltaTime);
    }

    /// <summary>
    /// Mueve un GameObject hacia su posición objetivo con la velocidad indicada.
    /// </summary>
    private void MoveTowards(GameObject gameObject, Vector3 targetPosition , float velocity)
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,targetPosition, velocity * Time.deltaTime);
    }

    /// <summary>
    /// Instancia una moneda y comienza su animación de ascenso/emergencia.
    /// </summary>
    private void GiveCoins()
    {

        GameObject Coin = Instantiate(CoinPrefab, this.transform.position + Vector3.up, Quaternion.identity);
        StartCoroutine(EmergeAnimationCoin(Coin));
        GameManager.Instance.AddCoin();
        GameManager.Instance.AddScore(200);

    }


    /// <summary>
    /// Corrutina: anima la moneda emergiendo del bloque y la elimina al terminar.
    /// </summary>
    /// <returns>IEnumerator — invocarse mediante <c>StartCoroutine</c></returns>
    private IEnumerator EmergeAnimationCoin(GameObject Coin)
    {
        do
        {
            this.MoveTowards(Coin,coinTargetPosition, 7f);
            yield return null;
        } while (Coin.transform.position != this.coinTargetPosition);

        do
        {
            this.MoveTowards(Coin, originalPosition, 7f);
            yield return null;
        } while (Coin.transform.position != this.originalPosition);
        Destroy(Coin);

    }

    /// <summary>
    /// Instancia un power-up y lanza su animación de emergencia desde el bloque.
    /// </summary>
    private void GivePowerUp(GameObject powerUpPrefab)
    {
        GameObject PowerUp = Instantiate(powerUpPrefab, this.transform.position+ Vector3.up * 0.3f , Quaternion.identity);
        PowerUpBase powerUpScript = PowerUp.GetComponent<PowerUpBase>();
        powerUpScript.EmergeFromBlock();
        StartCoroutine(EmergeAnimationPowerUp(PowerUp,powerUpScript));
        

    }

    /// <summary>
    /// Corrutina: anima el power-up emergiendo hasta su posición objetivo y finaliza su activación.
    /// </summary>
    /// <returns>IEnumerator — invocarse mediante <c>StartCoroutine</c></returns>
    private IEnumerator EmergeAnimationPowerUp(GameObject PowerUp, PowerUpBase powerUpScript)
    {
        do
        {
            this.MoveTowards(PowerUp, powerUpTargetPosition,1f);

            yield return null;
        } while (PowerUp.transform.position != powerUpTargetPosition);
        
        powerUpScript.FinishEmerging();
    }

}
