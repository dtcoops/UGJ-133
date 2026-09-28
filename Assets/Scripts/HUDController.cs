using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    [Header("Timer & Score")]
    public TMP_Text timerText;
    public TMP_Text scoreText;

    [Header("Lives")]
    public GameObject[] lifeIcons;

    [Header("Annoyance")]
    public Slider annoyanceSlider;
    public Image annoyanceFillImage;
    public Color calmColor = Color.green;
    public Color chaseColor = Color.red;

    [Header("Stamina")]
    public Slider staminaSlider;
    public CatController cat;

    void Update()
    {
        UpdateTimer();
        UpdateScore();
        UpdateLives();
        UpdateAnnoyance();
        UpdateStamina();
    }

    void UpdateTimer()
    {
        float time = GameManager.Instance.TimeRemaining;
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        timerText.SetText($"{minutes:00}:{seconds:00}");
    }

    void UpdateScore()
    {
        scoreText.text = $"Score: {ScoreManager.Instance.CurrentScore}";
    }

    void UpdateLives()
    {
        int currentLives = LivesManager.Instance.CurrentLives;

        for (int i = 0; i < lifeIcons.Length; i++)
        {
            lifeIcons[i].SetActive(i < currentLives);
        }
    }

    void UpdateAnnoyance()
    {
        AnnoyanceManager annoyance = AnnoyanceManager.Instance;
        annoyanceSlider.value = annoyance.CurrentAnnoyance / annoyance.maxAnnoyance;
        annoyanceFillImage.color = annoyance.IsChasing ? chaseColor : calmColor;
    }

    void UpdateStamina()
    {
        staminaSlider.value = cat.StaminaPercent;
    }
}
