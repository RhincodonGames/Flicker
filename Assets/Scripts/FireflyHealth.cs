using UnityEngine;

public class FireflyHealth : MonoBehaviour
{
    public int maxLives = 4;
    public int currentLives;
    public float invulnerabilityDuration = 1.5f; // brief invulnerability after a hit so one bird can't chain-hit you
    private float invulnTimer = 0f;

    void Start()
    {
        currentLives = maxLives;
        UIManager.Instance.UpdateLivesDisplay(currentLives, maxLives);
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
        UIManager.Instance.UpdateLivesDisplay(currentLives, maxLives);

        if (currentLives <= 0)
            GameManager.Instance.OnOutOfLives();
    }
}