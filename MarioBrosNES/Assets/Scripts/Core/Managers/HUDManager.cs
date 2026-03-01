using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI coinsText;
    public TextMeshProUGUI timeText;

    [Header("Loading Screen")]
    [SerializeField] private GameObject loadingPanel;
    [SerializeField] private TextMeshProUGUI loadingLivesText;

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
        }
    }

    public void UpdateHUD(GameStats stats)
    {
        // Formato de 6 dígitos para puntos
        scoreText.text = stats.score.ToString("D6");

        // Formato de 2 dígitos para monedas
        coinsText.text = stats.coins.ToString("D2");

    }
    public IEnumerator TimerRoutine(GameStats stats)
    {
        while (stats.time > 0)
        {
            yield return new WaitForSeconds(0.4f);

            stats.time--;
            timeText.text = stats.time.ToString();
        }
    }


    public IEnumerator ShowLevelStartScreen()
    {
        // Activamos panel negro
        loadingPanel.SetActive(true);
        loadingLivesText.enabled = true;

        loadingLivesText.text = "x " + GameManager.Instance.stats.lives;

        Time.timeScale = 0f;

        // Esperamos en tiempo REAL
        yield return new WaitForSecondsRealtime(5f);

        // Quitamos pantalla
        loadingPanel.SetActive(false);
        loadingLivesText.enabled = false;

        Time.timeScale = 1f;
    }
}