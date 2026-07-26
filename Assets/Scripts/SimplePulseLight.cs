using UnityEngine;

public class SimplePulseLight : MonoBehaviour
{
    public Light glowLight;
    public float baseIntensity = 0.8f;
    public float pulseAmplitude = 0.2f;
    public float pulseSpeed = 1f;
    private float phaseOffset;

    void Start() { phaseOffset = Random.Range(0f, Mathf.PI * 2f); }

    void Update()
    {
        if (glowLight == null) return;
        glowLight.intensity = baseIntensity + Mathf.Sin(Time.time * pulseSpeed + phaseOffset) * pulseAmplitude;
    }
}