using UnityEngine;

public class FireflyWander : MonoBehaviour
{
    [Header("Wander Movement")]
    public float wanderRadius = 2f;       // how far from spawn point it roams
    public float moveSpeed = 0.8f;
    public float verticalBobHeight = 0.5f;
    public float verticalBobSpeed = 1.2f;
    public float newTargetInterval = 3f;  // how often it picks a new spot to drift toward

    [Header("Light Pulse")]
    public Light glowLight;
    public float baseIntensity = 1f;
    public float pulseAmplitude = 0.4f;   // keep this small — this is idle ambience, not the player's flicker
    public float pulseSpeed = 1.5f;

    private Vector3 spawnOrigin;
    private Vector3 currentTarget;
    private float targetTimer;
    private float pulsePhaseOffset; // randomized per instance so a cluster of fireflies doesn't pulse in unison

    void Start()
    {
        spawnOrigin = transform.position;
        pulsePhaseOffset = Random.Range(0f, Mathf.PI * 2f);
        PickNewTarget();
    }

    void Update()
    {
        HandleWander();
        HandlePulse();
    }

    void HandleWander()
    {
        targetTimer -= Time.deltaTime;
        if (targetTimer <= 0f) PickNewTarget();

        Vector3 horizontalTarget = new Vector3(currentTarget.x, transform.position.y, currentTarget.z);
        transform.position = Vector3.MoveTowards(transform.position, horizontalTarget, moveSpeed * Time.deltaTime);

        // Vertical bob layered on top independently, so it's always gently rising/falling regardless of horizontal drift
        float bobY = spawnOrigin.y + Mathf.Sin(Time.time * verticalBobSpeed + pulsePhaseOffset) * verticalBobHeight;
        transform.position = new Vector3(transform.position.x, bobY, transform.position.z);
    }

    void PickNewTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        currentTarget = spawnOrigin + new Vector3(randomCircle.x, 0f, randomCircle.y);
        targetTimer = newTargetInterval;
    }

    void HandlePulse()
    {
        if (glowLight == null) return;
        glowLight.intensity = baseIntensity + Mathf.Sin(Time.time * pulseSpeed + pulsePhaseOffset) * pulseAmplitude;
    }
}