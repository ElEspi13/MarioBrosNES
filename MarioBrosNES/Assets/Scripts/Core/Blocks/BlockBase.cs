using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockBase : MonoBehaviour
{
    
    [SerializeField] private bool isCoinBlock;
    [SerializeField] private bool isPowerUpBlock;
    [SerializeField] private bool is1UpBlock;
    [SerializeField] private bool isStarBlock;

    [SerializeField] private float velocity;
    [SerializeField] private float offsetY;
    [SerializeField] private GameObject CoinPrefab;
    [SerializeField] private GameObject PowerPrefabMush;
    [SerializeField] private GameObject PowerPrefabFlower;
    [SerializeField] private GameObject OneUpPrefab;
    [SerializeField] private GameObject StarPrefab;

    private Vector3 positionTarget;
    private Vector3 powerUpTargetPosition;
    private Vector3 coinTargetPosition;
    private Vector3 originalPosition;
    
    private bool isActive = false;

    private PlayerManager player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerManager>();
        this.originalPosition = this.transform.position;
        this.powerUpTargetPosition = transform.position + Vector3.up * 1f;
        ResetTargetPosition();
    }
    private void ResetTargetPosition()
    {
        this.positionTarget = transform.position + Vector3.up * offsetY;
        this.coinTargetPosition = transform.position + Vector3.up * 3f;
    }
    protected virtual void OnHit()
    {
        if (!isActive)
        {
            StartCoroutine(HitAnimation());
            if (isCoinBlock)
            {
                GiveCoins();
            }else if(isStarBlock){
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnHit();
            Debug.Log("Block Hit!");
        }
    }

    private IEnumerator HitAnimation() 
    {
        isActive = true;
        do 
        { 
            this.MoveTowards();
            yield return null;
        } while (this.transform.position != this.positionTarget);

        this.positionTarget = this.originalPosition;
        do 
        {
            this.MoveTowards();
            yield return null;
        } while (this.transform.position != this.originalPosition);
        isActive = false;
        ResetTargetPosition();

    }
    private void MoveTowards()
    {
        this.transform.position = Vector3.MoveTowards(transform.position,positionTarget,this.velocity * Time.deltaTime);
    }

    private void MoveTowards(GameObject gameObject, Vector3 targetPosition , float velocity)
    {
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position,targetPosition, velocity * Time.deltaTime);
    }

    private void GiveCoins()
    {
        GameObject Coin = Instantiate(CoinPrefab, this.transform.position + Vector3.up, Quaternion.identity);
        StartCoroutine(EmergeAnimationCoin(Coin));
    }

    private IEnumerator EmergeAnimationCoin(GameObject Coin)
    {
        do
        {
            this.MoveTowards(Coin,coinTargetPosition, 7f);
            yield return null;
        } while (Coin.transform.position != this.coinTargetPosition);

        this.coinTargetPosition = this.originalPosition;
        do
        {
            this.MoveTowards(Coin, coinTargetPosition, 7f);
            yield return null;
        } while (Coin.transform.position != this.originalPosition);
        ResetTargetPosition();
        Destroy(Coin);

    }

    private void GivePowerUp(GameObject powerUpPrefab)
    {
        GameObject PowerUp = Instantiate(powerUpPrefab, this.transform.position+ Vector3.up * 0.3f , Quaternion.identity);
        PowerUpBase powerUpScript = PowerUp.GetComponent<PowerUpBase>();
        powerUpScript.EmergeFromBlock();
        StartCoroutine(EmergeAnimationPowerUp(PowerUp,powerUpScript));
        

    }

    private IEnumerator EmergeAnimationPowerUp(GameObject PowerUp, PowerUpBase powerUpScript)
    {
        do
        {
            Debug.Log("Emerging PowerUp");
            this.MoveTowards(PowerUp, powerUpTargetPosition,1f);

            yield return null;
        } while (PowerUp.transform.position != powerUpTargetPosition);
        
        powerUpScript.FinishEmerging();
    }

}
