using UnityEngine;

public class FireflyHealth : MonoBehaviour
{
    public int maxLives = 1;
    public int currentLives;
    public int absoluteLivesCap = 10;
    public float invulnerabilityDuration = 1.5f; // brief invulnerability after a hit so one bird can't chain-hit you
    private float invulnTimer = 0f;

    void Start()
    {
        currentLives = maxLives;
        UIManager.Instance.RefreshLivesUI(maxLives, currentLives);
    }

    void Update()
    {
        if (invulnTimer > 0f) invulnTimer -= Time.deltaTime;
    }

    public bool IsInvulnerable => invulnTimer > 0f;

    public void TakeHit()
    {
        if (IsInvulnerable) return;

        currentLives--;
        invulnTimer = invulnerabilityDuration;
        UIManager.Instance.RefreshLivesUI(currentLives, maxLives);

        if (currentLives <= 0)
            GameManager.Instance.OnOutOfLives();
    }

    public void CollectFriend()
    {
        if (currentLives < maxLives)
        {
            currentLives++; // heal a lost life first
        }
        else if (maxLives < absoluteLivesCap)
        {
            maxLives++;
            currentLives++; // new slot arrives already "alive"
        }
        UIManager.Instance.RefreshLivesUI(maxLives, currentLives);
    }

    public void IncreaseMaxLives(int amount)
    {
        maxLives = Mathf.Clamp(maxLives + amount, 1, absoluteLivesCap);
        currentLives = Mathf.Clamp(currentLives + amount, 0, maxLives); // gaining a life also heals you by that amount, matches your "added" framing
        UIManager.Instance.RefreshLivesUI(maxLives, currentLives);
    }
}