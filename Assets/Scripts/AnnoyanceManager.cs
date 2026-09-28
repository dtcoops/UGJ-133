using UnityEngine;

public class AnnoyanceManager : MonoBehaviour
{
    // Singleton!   
    public static AnnoyanceManager Instance { get; private set; }

    [Header("Annoyance Settings")]
    public float maxAnnoyance = 100f;
    public float decayRate = 1f;

    public float CurrentAnnoyance { get; private set; }
    public bool IsChasing { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Update()
    {
        CheckChaseState();
        DecayAnnoyance();
    }

    public void AddAnnoyance(int annoyanceValue)
    {
        float scaledValue = IsChasing ? annoyanceValue * GetChaseMultiplier() : annoyanceValue;
        CurrentAnnoyance = Mathf.Clamp(CurrentAnnoyance + scaledValue, 0f, maxAnnoyance);
    }

    private void DecayAnnoyance()
    {
        if (CurrentAnnoyance <= 0f)
        {
            return;
        }

        CurrentAnnoyance = Mathf.Max(0f, CurrentAnnoyance - decayRate * Time.deltaTime);
    }

    float GetChaseMultiplier()
    {
        int livesRemaining = LivesManager.Instance.CurrentLives;
        int maxLives = LivesManager.Instance.maxLives;

        return (maxLives - livesRemaining + 1f) / maxLives;
    }

    void CheckChaseState()
    {
        if (!IsChasing && CurrentAnnoyance >= maxAnnoyance)
        {
            IsChasing = true;
        }
        else if (IsChasing && CurrentAnnoyance <= 0f)
        {
            IsChasing = false;
        }
    }

    public void ForceEndChase()
    {
        IsChasing = false;
        CurrentAnnoyance = 0f;
    }
}
