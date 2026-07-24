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
    public Image[] lifeIcons; // drag all life icon Images in, left to right

    [Header("Dawn Clock")]
    public TextMeshProUGUI dawnClockText;

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

    public void UpdateLivesDisplay(int current, int max)
    {
        for (int i = 0; i < lifeIcons.Length; i++)
            lifeIcons[i].enabled = i < current;
    }

    public void UpdateDawnClock(float secondsRemaining)
    {
        if (dawnClockText == null) return;
        secondsRemaining = Mathf.Max(0f, secondsRemaining);
        int minutes = Mathf.FloorToInt(secondsRemaining / 60f);
        int seconds = Mathf.FloorToInt(secondsRemaining % 60f);
        dawnClockText.text = $"{minutes:00}:{seconds:00}";
    }

    public void ShowEndScreen(string message, int friendsCollected)
    {
        if (endPanel != null) endPanel.SetActive(true);
        if (endMessageText != null)
            endMessageText.text = $"{message}\nFriends collected: {friendsCollected}\nClick to restart";
    }
}