using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private CameraFollow _cameraFollow;
    [SerializeField] private GameObject _player;
    [SerializeField] private PauseMenu _pauseMenu;
    private Coroutine _timerCoroutine;

    public Checkpoints currentCheckpoint;

    [Header("Game Stats")]
    public GameStats stats = new GameStats();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (_player == null)
        {
            SpawnPlayer(_playerPrefab.transform.position);
        }
    }

    void Update()
    {
        if (stats.time <= 0)
        {
            LoseLife();
        }
    }

    public void ChangeLevel(int sceneIndex)
    {
        StartCoroutine(ChangeLevels(sceneIndex));

    }

    private IEnumerator ChangeLevels(int sceneIndex)
    {

        SceneManager.LoadSceneAsync(sceneIndex);
        yield return HUDManager.Instance.ShowLevelStartScreen();
        _timerCoroutine = StartCoroutine(HUDManager.Instance.TimerRoutine(stats));
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Metodo que se llama cada vez que se carga una nueva escena, se asegura de que la camara siga al jugador en cada nivel.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_cameraFollow == null)
            _cameraFollow = FindObjectOfType<CameraFollow>();

        if (_player != null)
            _cameraFollow.SetTarget(_player.transform);
    }

    /// <summary>
    /// Metodo para instanciar al jugador en una posición dada, utilizado para respawns y cambios de nivel. Asegura que la cámara siga al jugador después de instanciarlo.
    /// </summary>
    /// <param name="position"></param>
    private void SpawnPlayer(Vector3 position)
    {
        _player = Instantiate(_playerPrefab, position, Quaternion.identity);
        if (_cameraFollow != null)
            _cameraFollow.SetTarget(_player.transform);
    }

    /// <summary>
    /// Metodo para actualizar el checkpoint actual del jugador, se llama desde los objetos de checkpoint en la escena. Esto permite que el jugador respawnee en el último checkpoint alcanzado después de morir o cambiar de nivel.
    /// </summary>
    /// <param name="id"></param>
    public void SetCheckpoint(Checkpoints id)
    {
        currentCheckpoint = id;
        Debug.Log("Checkpoint guardado: " + id);
    }

    /// <summary>
    /// Busca el checkpoint actual en la escena y mueve al jugador a esa posición, utilizado para respawns después de morir. Asegura que la cámara siga al jugador después de moverlo.
    /// </summary>
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

    /// <summary>
    /// Metodo para generar un nuevo jugador en el último checkpoint alcanzado.
    /// </summary>
    internal void Respawn()
    {
        Checkpoint[] allCheckpoints = FindObjectsOfType<Checkpoint>();
        foreach (var cp in allCheckpoints)
        {
            if (cp.checkpointID == currentCheckpoint)
            {
                SpawnPlayer(cp.transform.position);
                return;
            }
            
        }
    }

    internal void TogglePause()
    {
        _pauseMenu.TogglePause();
    }

    /// <summary>
    /// Metodo para salir del juego: resetea las estadísticas, detiene el timer y actualiza el HUD para reflejar el estado de juego reiniciado. Se llama desde el menú de pausa.
    /// </summary>
    internal void ExitGame()
    {
        stats.ResetGameStats();
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }
        HUDManager.Instance.UpdateHUD(stats);
        HUDManager.Instance.timeText.text = "";
    }

    /// <summary>
    /// Metodo para manejar el game over: inicia la rutina de game over que recarga la escena, muestra la pantalla de inicio del nivel y resetea las estadísticas. También detiene el timer si está corriendo para evitar que siga decrementando después de perder todas las vidas.
    /// </summary>
    private void GameOver()
    {

        StartCoroutine(GameOverRoutine());
        if (_timerCoroutine != null)
        {
            StopCoroutine(_timerCoroutine);
            _timerCoroutine = null;
        }

    }
    private IEnumerator GameOverRoutine()
    {

        yield return SceneManager.LoadSceneAsync(0);
        InputManager.DisableMap(InputManager.Actions.UIcontrolls);
        yield return null;

        yield return HUDManager.Instance.ShowLevelStartScreen();

        stats.ResetGameStats();
        SpawnPlayer(_playerPrefab.transform.position);
        InputManager.SwitchMap(InputManager.Actions.UIcontrolls);

    }

    #region Stats Methods

    public void AddScore(int amount)
    {
        stats.score += amount;
        HUDManager.Instance.UpdateHUD(stats); 
    }

    public void AddCoin(int amount = 1)
    {
        stats.coins += amount;
        if (stats.coins >= 100)
        {
            stats.coins -= 100;
            AddLife(1);
        }
        HUDManager.Instance.UpdateHUD(stats); 
    }

    public void AddLife(int amount = 1)
    {
        stats.lives += amount;
        Debug.Log("Lives: " + stats.lives);
    }

    public void LoseLife()
    {
        stats.lives--;
        
        if (stats.lives <= 0)
        {

            GameOver();
            HUDManager.Instance.timeText.text="";
        }
        else
        {
            StartCoroutine(HUDManager.Instance.ShowLevelStartScreen());
            HUDManager.Instance.timeText.text = "";
            Respawn();
            stats.time = 400;
        }
    }

    public void UpdateTime(float delta)
    {
        stats.time -= delta;
        if (stats.time <= 0)
        {
            stats.time = 0;
            GameOver();
        }
    }

    

    #endregion
    #region Shell Combo System

    private int shellComboCount = 0;

    private readonly int[] shellComboTable =
    {
        500,   // 1 enemigo
        1000,  // 2
        2000,  // 3
        4000,  // 4
        5000,  // 5
        8000   // 6
        // Después de esto 1UP
    };

    public void AddShellComboPoints()
    {
        shellComboCount++;

        if (shellComboCount > shellComboTable.Length)
        {
            AddLife(1);
            return;
        }

        int points = shellComboTable[shellComboCount - 1];
        AddScore(points);
    }

    public void ResetShellCombo()
    {
        shellComboCount = 0;
    }

    #endregion
    #region Stomp Combo System

    private int stompComboCount = 0;

    private readonly int[] stompComboTable =
    {
        100,   // 1 enemigo
        200,   // 2
        400,   // 3
        800,   // 4
        1000,  // 5
        2000,  // 6
        4000,  // 7
        5000,  // 8
        8000   // 9
        // Después 1UP
    };

    public void AddStompComboPoints()
    {
        stompComboCount++;

        if (stompComboCount > stompComboTable.Length)
        {
            AddLife(1);
            return;
        }

        int points = stompComboTable[stompComboCount - 1];
        AddScore(points);
    }

    /// <summary>
    /// Resetea el contador de stomp combo a cero, se llama cuando el jugador toca el suelo.
    /// </summary>
    public void ResetStompCombo()
    {
        stompComboCount = 0;
    }

    



    #endregion
}