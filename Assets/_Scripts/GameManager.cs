using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Padrão Singleton para acesso fácil

    public float gameTime = 60f; // Duração do jogo em segundos
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

    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;

    void Awake()
    {
        // Configuração do Singleton
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
        // Inicializa a UI
        scoreText.text = "" + score;
        timerText.text = "Tempo: " + Mathf.Round(gameTime);
        inventoryText.text = "0/0";
        UpdateInventoryDetailUI(new List<TrashType>());
        gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isGameOver) return;

        // Atualiza o cronômetro
        gameTime -= Time.deltaTime;
        timerText.text = "Tempo: " + Mathf.Max(0, Mathf.RoundToInt(gameTime));

        // Condição de vitória (tempo acabou)
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
        gameOverText.text = reason;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0; // Pausa o jogo
    }

    public void WinGame()
    {
        isGameOver = true;
        gameOverText.text = "Tempo esgotado!\nPontuação Final: " + score;
        gameOverPanel.SetActive(true);
        Time.timeScale = 0; // Pausa o jogo
    }

    // Função para ser chamada pelo botão de reiniciar
    public void RestartGame()
    {
        Time.timeScale = 1; // Retoma o tempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Atualiza a UI do inventário do jogador
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

