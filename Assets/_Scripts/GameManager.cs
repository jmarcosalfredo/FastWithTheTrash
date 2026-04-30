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
    }

    void Update()
    {
        if (isGameOver) return;

        gameTime -= Time.deltaTime;
        timerText.text = "Tempo: " + Mathf.Max(0, Mathf.RoundToInt(gameTime));

        if (gameTime <= 0)
        {
            WinGame();
        }
    }

    public void AddScore(int amount)
    {
        if (isGameOver) return;
        score += amount;
        scoreText.text = "" + score;
    }

    public void GameOver(string reason)
    {
        if (isGameOver) return;
        isGameOver = true;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
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