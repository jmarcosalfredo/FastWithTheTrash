using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float gameTime = 60f;
    private int score = 0;
    private bool isGameOver = false;

    // Referências para os elementos da UI
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI inventoryText;

    [Header("UI Inventário por Tipo")]
    public TextMeshProUGUI papelCountText;
    public TextMeshProUGUI plasticoCountText;
    public TextMeshProUGUI metalCountText;
    public TextMeshProUGUI vidroCountText;

    [Header("Pause")]
    public GameObject pausePanel;
    [SerializeField] private GameObject tutorialTooltip;
    private bool isPaused = false;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI reasontxt;       // Exibe o motivo / título
    public TextMeshProUGUI gameOverScoreText;  // Exibe a pontuação final

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        scoreText.text = "" + score;
        timerText.text = "Tempo: " + Mathf.Round(gameTime);
        inventoryText.text = "0/0";
        UpdateInventoryDetailUI(new List<TrashType>());
        gameOverPanel.SetActive(false);
        pausePanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (isPaused) return;

        gameTime -= Time.deltaTime;
        timerText.text = "Tempo: " + Mathf.Max(0, Mathf.RoundToInt(gameTime));

        if (gameTime <= 0)
        {
            WinGame();
        }

        HandleTimeScale();
    }

    private void HandleTimeScale()
    {
        if (tutorialTooltip.activeSelf)
        {
            Time.timeScale = 0;
        }

        if (pausePanel.activeSelf == false && tutorialTooltip.activeSelf == false)
        {
            Time.timeScale = 1;
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePanel.SetActive(isPaused);
        Time.timeScale = isPaused ? 0 : 1;
    }

    public bool IsPaused => isPaused;

    public void OpenTutorial()
    {
        tutorialTooltip.SetActive(true);
    }

    public void CloseTutorial()
    {
        tutorialTooltip.SetActive(false);
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;
        AudioManager.instance?.PlayDiscardTrash();
        score += amount;
        scoreText.text = "" + score;
    }

    public void GameOver(string reason)
    {
        if (isGameOver) return;
        isGameOver = true;
        AudioManager.instance?.StopBackgroundMusic();
        AudioManager.instance?.PlayGameOver();
        reasontxt.text = reason;
        gameOverScoreText.text = "Pontuação Final: " + score;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void WinGame()
    {
        isGameOver = true;
        reasontxt.text = "Tempo esgotado!";
        gameOverScoreText.text = "Pontuação Final: " + score;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        AudioManager.instance?.PlayBackgroundMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        AudioManager.instance?.PlayBackgroundMusic();
        SceneManager.LoadScene("MainMenu");
    }

    public void UpdateInventoryUI(int currentCount, int maxCount)
    {
        inventoryText.text = currentCount + "/" + maxCount;
    }

    public void UpdateInventoryDetailUI(List<TrashType> inventory)
    {
        if (papelCountText != null)
            papelCountText.text = "" + inventory.Count(t => t == TrashType.Papel);
        if (plasticoCountText != null)
            plasticoCountText.text = "" + inventory.Count(t => t == TrashType.Plastico);
        if (metalCountText != null)
            metalCountText.text = " " + inventory.Count(t => t == TrashType.Metal);
        if (vidroCountText != null)
            vidroCountText.text = "" + inventory.Count(t => t == TrashType.Vidro);
    }
}