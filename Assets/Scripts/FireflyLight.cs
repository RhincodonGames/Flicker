using UnityEngine;

public class FireflyLight : MonoBehaviour
{
    [Header("Light Level (fuel — does not auto-refill)")]
    public float maxLightLevel = 20f;         // grows +10 per Firefly Friend collected
    public float currentLightLevel = 20f;
    public float maxLightLevelCap = 100f;
    public float maxLightGainPerFriend = 10f;

    [Header("Brightness (visual, rises/falls smoothly)")]
    public float brightness = 0f;             // 0-1
    public float brightenSpeed = 2.5f;        // how fast it rises while holding Space
    public float fadeSpeed = 1.2f;            // how fast it falls after releasing Space
    public float fuelCostPerSecondAtFullBrightness = 6f;

    [Header("Light Component")]
    public Light glowLight;
    public float maxIntensity = 6f;
    public float maxRange = 9f;
    public float ambientIntensity = 0.3f;     // faint glow even at brightness 0, so you're not pure black
    public float ambientRange = 1.5f;

    public bool IsAlive { get; private set; } = true;

    void Update()
    {
        if (!IsAlive) return;

        bool holdingLight = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        // Brightness rises while held, fades when released — this is what creates the
        // "tap for small burst, hold for sustained light" feel
        if (holdingLight && currentLightLevel > 0f)
            brightness = Mathf.MoveTowards(brightness, 1f, brightenSpeed * Time.deltaTime);
        else
            brightness = Mathf.MoveTowards(brightness, 0f, fadeSpeed * Time.deltaTime);

        // Fuel only drains proportional to current brightness — quick taps barely cost anything
        if (brightness > 0f)
        {
            currentLightLevel -= fuelCostPerSecondAtFullBrightness * brightness * Time.deltaTime;
            currentLightLevel = Mathf.Clamp(currentLightLevel, 0f, maxLightLevel);
        }

        UpdateLightVisual();

        if (currentLightLevel <= 0f)
            Die();
    }

    void UpdateLightVisual()
    {
        if (glowLight == null) return;
        glowLight.intensity = ambientIntensity + (maxIntensity - ambientIntensity) * brightness;
        glowLight.range = ambientRange + (maxRange - ambientRange) * brightness;
    }

    public void AddFuel(float amount)
    {
        currentLightLevel = Mathf.Clamp(currentLightLevel + amount, 0f, maxLightLevel);
    }

    public void IncreaseMaxLight()
    {
        maxLightLevel = Mathf.Clamp(maxLightLevel + maxLightGainPerFriend, 0f, maxLightLevelCap);
        // Collecting a friend also tops up some fuel so growing max capacity feels rewarding, not punishing
        currentLightLevel = Mathf.Clamp(currentLightLevel + maxLightGainPerFriend * 0.5f, 0f, maxLightLevel);
    }

    void Die()
    {
        IsAlive = false;
        brightness = 0f;
        UpdateLightVisual();
        GameManager.Instance.OnOutOfLight();
    }

    // Birds use this to know how attracted they should be
    public float GetBrightness() => brightness;
}