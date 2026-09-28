using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameOverReason { LivesLost, TimeUp }

    [Header("Round Settings")]
    public float roundDuration = 180f; // 3 minutes

    public float TimeRemaining { get; private set; }
    public bool IsGameOver { get; private set; }
    public GameOverReason EndReason { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        TimeRemaining = roundDuration;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (IsGameOver) return;

        TimeRemaining -= Time.deltaTime;
        if (TimeRemaining <= 0f)
        {
            TimeRemaining = 0f;
            EndGame(GameOverReason.TimeUp);
        }
    }

    public void EndGame(GameOverReason reason)
    {
        if (IsGameOver)
        {
            return;
        }

        IsGameOver = true;
        TimeRemaining = 0f;
        EndReason = reason;
        Debug.Log("Game Over!");
        // TODO: freeze cat input, stop human, show game-over UI

        Time.timeScale = 0f;
    }
}
