using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Dawn Timer")]
    public float dawnTimerSeconds = 300f; // 5 minutes 
    private float timeRemaining;

    [Header("Sky")]
    public SkyboxController skyboxController;


    [Header("Bird Spawning")]
    public GameObject birdPrefab;
    public Transform player;
    public float initialSpawnInterval = 8f;   // seconds between spawns at game start
    public float minSpawnInterval = 1.5f;     // fastest spawn rate, reached near dawn
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

        HandleBirdSpawning();
    }

    void HandleBirdSpawning()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnBird();

            // Closer to dawn (timeRemaining near 0) = shorter interval = more birds
            float t = 1f - Mathf.Clamp01(timeRemaining / dawnTimerSeconds); // 0 at start, 1 at dawn
            spawnTimer = Mathf.Lerp(initialSpawnInterval, minSpawnInterval, t);
        }
    }

    void SpawnBird()
    {
        if (birdPrefab == null || player == null) return;

        // Spawn at a random point on a ring around the player, slightly above
        Vector2 randomCircle = Random.insideUnitCircle.normalized * Random.Range(10f, 15f);
        Vector3 spawnPos = player.position + new Vector3(randomCircle.x, 3f, randomCircle.y);

        //GameObject bird = Instantiate(birdPrefab, spawnPos, Quaternion.identity);
        //BirdAI ai = bird.GetComponent<BirdAI>();
        //if (ai != null) ai.target = player;
    }

    public void OnFriendCollected()
    {
        fireflyFriendsCollected++;
        player.GetComponent<FireflyLight>().IncreaseMaxLight();
        player.GetComponent<FireflyHealth>().CollectFriend();
        UIManager.Instance.UpdateFriendsText(fireflyFriendsCollected);
    }

    public void OnOutOfLight()
    {
        EndGame("Your light went out...");
    }

    public void OnOutOfLives()
    {
        EndGame("The birds got you...");
    }

    public void OnSurvivedToDawn()
    {
        EndGame("You made it to dawn!");
    }

    void EndGame(string message)
    {
        if (isGameOver) return;
        isGameOver = true;
        UIManager.Instance.ShowEndScreen(message, fireflyFriendsCollected);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}