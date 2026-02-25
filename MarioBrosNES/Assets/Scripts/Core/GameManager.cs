using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField]private GameObject _playerPrefab;
    [SerializeField] private CameraFollow _cameraFollow;
    [SerializeField] private GameObject _player;

    public Checkpoints currentCheckpoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_player == null)
        {
            var player = Instantiate(_playerPrefab, _playerPrefab.transform.position, Quaternion.identity);
            _player = player;
            _cameraFollow.SetTarget(player.transform);
        }

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Guarda el checkpoint actual usando su ID
    /// </summary>
    public void SetCheckpoint(Checkpoints id)
    {
        currentCheckpoint = id;
        Debug.Log("Checkpoint guardado: " + id);
    }

    internal void Respawn()
    {
        Checkpoint[] allCheckpoints = FindObjectsOfType<Checkpoint>();

        foreach (var cp in allCheckpoints)
        {
            if (cp.checkpointID== currentCheckpoint)
            {
                
                _player = Instantiate(_playerPrefab, cp.transform.position, Quaternion.identity);
                
                return;
            }
        }

    }
    

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_cameraFollow == null)
            _cameraFollow = FindObjectOfType<CameraFollow>();
            _cameraFollow.SetTarget(_player.transform);
    }
   

    public void ResetPlayerPosition()
    {
        Checkpoint[] allCheckpoints = FindObjectsOfType<Checkpoint>();

        foreach (var cp in allCheckpoints)
        {
            if (cp.checkpointID == currentCheckpoint)
            {

                _player.transform.position = cp.transform.position;
                _cameraFollow.SetTarget(_player.transform);
            }
        }
    }

}