using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // Singleton!   
    public static ScoreManager Instance { get; private set; }

    public int CurrentScore { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddScore(int itemValue)
    {
        CurrentScore += itemValue;
        Debug.Log($"Score: {CurrentScore} (+{itemValue})");
    }
}
