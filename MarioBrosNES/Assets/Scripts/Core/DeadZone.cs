using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }
        else
        {
            StartCoroutine(DeathRoutine(collision.gameObject));
            
        }
    }
    private IEnumerator DeathRoutine(GameObject Player)
    {
        yield return new WaitForSeconds(2f);

        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
        Destroy(Player.gameObject);
        GameManager.Instance.LoseLife();
    }
}
