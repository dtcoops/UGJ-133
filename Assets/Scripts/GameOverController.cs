using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    [Header("Panel")]
    public GameObject panel;
    public TMP_Text headlineText;
    public TMP_Text scoreText;

    [Header("Scene")]
    public string gameplaySceneName;
    public string mainMenuSceneName;

    int bestScore;

    void Start()
    {
        panel.SetActive(false);
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver && !panel.activeSelf)
        {
            ShowGameOver();
        }
    }

    void ShowGameOver()
    {
        panel.SetActive(true);

        headlineText.text = GameManager.Instance.EndReason == GameManager.GameOverReason.LivesLost
            ? "Bad Kitty!"
            : "Time for a Cat-Nap!";

        int currentScore = ScoreManager.Instance.CurrentScore;
        bool isNewBest = currentScore > bestScore;

        if (isNewBest)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", bestScore);
        }

        scoreText.text = isNewBest
            ? $"New High Score: {currentScore}!"
            : $"Score: {currentScore}  (Best: {bestScore})";
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
