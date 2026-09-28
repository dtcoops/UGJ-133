using UnityEngine;

public class LivesManager : MonoBehaviour
{
    public static LivesManager Instance { get; private set; }

    [Header("Lives Settings")]
    public int maxLives = 3;

    public int CurrentLives { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        CurrentLives = maxLives;
    }

    public void LoseLife()
    {
        CurrentLives = Mathf.Max(0, CurrentLives - 1);

        if (CurrentLives <= 0)
        {
            ReportEndGame();
        }

    }

    private void ReportEndGame()
    {
        GameManager.Instance.EndGame(GameManager.GameOverReason.LivesLost);
    }
}
