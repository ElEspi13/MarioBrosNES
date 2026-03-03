using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PipeEntry : MonoBehaviour
{
    public enum PipeDirection
    {
        Down,
        Left,
        Right
    }

    [Header("Pipe Settings")]
    [SerializeField] private PipeDirection direction;
    [SerializeField] private string sceneName;
    [SerializeField] private Checkpoints checkpoint;

    private bool playerInside = false;
    private bool isEntering = false;
    private PlayerController player;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        Debug.Log("Player entered pipe trigger");

        playerInside = true;
        player = other.GetComponent<PlayerController>();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;
        player = null;
    }

    private void Update()
    {
        if (!playerInside || player == null || isEntering)
            return;

        Vector2 input = player.MoveInput;

        bool correctDirection = false;

        switch (direction)
        {
            case PipeDirection.Down:
                correctDirection = input.y < -0.5f;
                break;

            case PipeDirection.Left:
                correctDirection = input.x < -0.5f;
                break;

            case PipeDirection.Right:
                correctDirection = input.x > 0.5f;
                break;
        }

        if (correctDirection)
        {
            EnterPipe();
        }
    }

    private void EnterPipe()
    {
        isEntering = true;


        GameManager.Instance.SetCheckpoint(checkpoint);
        SceneManager.LoadScene(sceneName);
    }

}
