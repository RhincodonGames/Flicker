using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Light + Friends")]
    public Slider lightLevelBar;
    public FireflyLight playerLight;
    public TextMeshProUGUI friendsText;

    [Header("Lives")]
    public LivesUI livesUI; // drag your LivesContainer (with the LivesUI script) in

    [Header("Dawn Clock")]
    public TextMeshProUGUI dawnClockText;
    public DayNightDial dayNightDial;
    public GameManager gameManagerRef; // needed so the dial knows total dawn duration

    [Header("End Screen")]
    public GameObject endPanel;
    public TextMeshProUGUI endMessageText;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (playerLight != null && lightLevelBar != null)
            lightLevelBar.value = playerLight.currentLightLevel / playerLight.maxLightLevel;

        if (GameManager.Instance != null && GameManager.Instance.isGameOver && Input.GetMouseButtonDown(0))
            GameManager.Instance.RestartGame();
    }

    public void UpdateFriendsText(int count)
    {
        if (friendsText != null) friendsText.text = "Friends: " + count;
    }

    public void RefreshLivesUI(int maxLives, int currentLives)
    {
        if (livesUI != null)
            livesUI.UpdateLives(maxLives, currentLives);
    }

    public void UpdateDawnClock(float secondsRemaining)
    {
        if (dawnClockText != null)
        {
            secondsRemaining = Mathf.Max(0f, secondsRemaining);
            int minutes = Mathf.FloorToInt(secondsRemaining / 60f);
            int seconds = Mathf.FloorToInt(secondsRemaining % 60f);
            dawnClockText.text = $"{minutes:00}:{seconds:00}";
        }

        if (dayNightDial != null && gameManagerRef != null)
            dayNightDial.UpdateDial(secondsRemaining, gameManagerRef.dawnTimerSeconds);
    }

    public void ShowEndScreen(string message, int friendsCollected)
    {
        if (endPanel != null) endPanel.SetActive(true);
        if (endMessageText != null)
            endMessageText.text = $"{message}\nFriends collected: {friendsCollected}\nClick to restart";
    }
}