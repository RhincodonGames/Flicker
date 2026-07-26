using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    public PauseMenuManager pausedMenuManager;

    [Header("Light + Friends")]
    public Slider lightLevelBar;
    public FireflyLight playerLight;

    [Header("Lives")]
    public LivesUI livesUI; // drag your LivesContainer (with the LivesUI script) in

    [Header("Dawn Clock")]
    public TextMeshProUGUI dawnClockText;
    public DayNightDial dayNightDial;
    public GameManager gameManagerRef; // needed so the dial knows total dawn duration

    [Header("End Screen")]
    public GameObject endPanel;

    [Header("Win Screen")]
    public GameObject winPanel;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (playerLight != null && lightLevelBar != null)
            lightLevelBar.value = playerLight.currentLightLevel / playerLight.maxLightLevel;
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

    public void ShowEndScreen(string message, int friendsCollected, bool isWin)
    {
        if (isWin)
        {
            if (winPanel != null) winPanel.SetActive(true);
        }
        else
        {
            if (endPanel != null) endPanel.SetActive(true);
        }

        pausedMenuManager.FreezeForEndScreen();
    }
}