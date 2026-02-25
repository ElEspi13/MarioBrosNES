using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Checkpoints checkpointID;

    private void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.SetCheckpoint(checkpointID);
        }
    }
}