using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Dawn Timer")]
    public float dawnTimerSeconds = 120f; // 2 mins
    private float timeRemaining;

    [Header("Sky")]
    public SkyboxController skyboxController;


    [Header("Bat Spawning")]
    public GameObject batPrefab;
    public Transform player;
    public float initialSpawnInterval = 8f;
    public float minSpawnInterval = 1.5f;
    private float spawnTimer;

    [Header("Progress")]
    public int fireflyFriendsCollected = 0;
    public bool isGameOver = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        timeRemaining = dawnTimerSeconds;
        spawnTimer = initialSpawnInterval;
    }

    void Update()
    {
        if (isGameOver) return;

        timeRemaining -= Time.deltaTime;
        UIManager.Instance.UpdateDawnClock(timeRemaining);

        if (skyboxController != null)
            skyboxController.UpdateSky(timeRemaining, dawnTimerSeconds);

        if (timeRemaining <= 0f)
        {
            OnSurvivedToDawn();
            return;
        }

        HandleBatSpawning();
    }

    void HandleBatSpawning()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnBat();

            // Closer to dawn (timeRemaining near 0) = shorter interval = more bats
            float t = 1f - Mathf.Clamp01(timeRemaining / dawnTimerSeconds); // 0 at start, 1 at dawn
            spawnTimer = Mathf.Lerp(initialSpawnInterval, minSpawnInterval, t);
        }
    }

    void SpawnBat()
    {
        if (batPrefab == null || player == null) return;

        Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(10f, 15f);
        Vector3 spawnPos = player.position + new Vector3(randomCircle.x, 3f, randomCircle.y);

        GameObject bat = Instantiate(batPrefab, spawnPos, Quaternion.identity);
        BatAI ai = bat.GetComponent<BatAI>();
        if (ai != null) ai.target = player;
    }

    public void OnFriendCollected()
    {
        fireflyFriendsCollected++;
        player.GetComponent<FireflyLight>().IncreaseMaxLight();
        player.GetComponent<FireflyHealth>().CollectFriend();
    }

    public void OnOutOfLight()
    {
        EndGame("Your light went out...", false);
    }

    public void OnOutOfLives()
    {
        EndGame("The bats got you...", false);
    }

    public void OnSurvivedToDawn()
    {
        EndGame("You made it to dawn!", true);
    }

    void EndGame(string message, bool isWin)
    {
        if (isGameOver) return;
        isGameOver = true;
        UIManager.Instance.ShowEndScreen(message, fireflyFriendsCollected, isWin);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}