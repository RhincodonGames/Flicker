using UnityEngine;

public class FireflyHealth : MonoBehaviour
{
    public int maxLives = 1;
    public int currentLives;
    public int absoluteLivesCap = 10;
    public float invulnerabilityDuration = 1.5f;
    private float invulnTimer = 0f;

    public FireflySwarm swarm;

    public bool IsAtMaxLives => currentLives >= absoluteLivesCap;
    public bool IsInvulnerable => invulnTimer > 0f;

    void Start()
    {
        currentLives = maxLives;
        UIManager.Instance.RefreshLivesUI(maxLives, currentLives);
    }

    void Update()
    {
        if (invulnTimer > 0f) invulnTimer -= Time.deltaTime;
    }

    public void CollectFriend()
    {
        if (currentLives >= absoluteLivesCap) return;

        if (currentLives < maxLives)
            currentLives++;
        else if (maxLives < absoluteLivesCap)
        {
            maxLives++;
            currentLives++;
        }

        swarm.AddCompanion();
        UIManager.Instance.RefreshLivesUI(maxLives, currentLives);
    }

    public void TakeHit()
    {
        if (IsInvulnerable || currentLives <= 0) return;

        currentLives--;
        invulnTimer = invulnerabilityDuration;
        swarm.RemoveCompanion();
        UIManager.Instance.RefreshLivesUI(maxLives, currentLives);

        if (currentLives <= 0)
            GameManager.Instance.OnOutOfLives();
    }

    public void IncreaseMaxLives(int amount)
    {
        maxLives = Mathf.Clamp(maxLives + amount, 1, absoluteLivesCap);
        currentLives = Mathf.Clamp(currentLives + amount, 0, maxLives); // gaining a life also heals you by that amount, matches your "added" framing
        UIManager.Instance.RefreshLivesUI(maxLives, currentLives);
    }
}