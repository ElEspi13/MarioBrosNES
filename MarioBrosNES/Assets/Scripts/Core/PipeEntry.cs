using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PipeEntry : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Checkpoints checkpoint;

    /// <summary>
    /// Cambio de escena al entrar en la tubería, estableciendo el checkpoint correspondiente en el GameManager.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        GameManager.Instance.SetCheckpoint(checkpoint);
        SceneManager.LoadScene(sceneName);
    }

}
