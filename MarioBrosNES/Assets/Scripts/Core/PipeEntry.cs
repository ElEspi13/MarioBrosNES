using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PipeEntry : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private Checkpoints checkpoint;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        GameManager.Instance.SetCheckpoint(checkpoint);
        SceneManager.LoadScene(sceneName);
    }

}
