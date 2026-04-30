using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject tutorialTooltip;

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OpenTooltip()
    {
        tutorialTooltip.SetActive(true);
    }

    public void CloseTooltip()
    {
        tutorialTooltip.SetActive(false);
    }
}