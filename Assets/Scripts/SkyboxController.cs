using UnityEngine;

public class SkyboxController : MonoBehaviour
{
    [Header("Key Colors (sampled across the night)")]
    public Gradient skyGradient; // set keys in Inspector: sunset orange -> deep night blue -> pale dawn

    [Header("Fog sync (optional, ties fog color to the same gradient)")]
    public bool syncFogColor = true;

    private Material skyboxMaterialInstance;

    void Awake()
    {
        skyboxMaterialInstance = new Material(RenderSettings.skybox);
        RenderSettings.skybox = skyboxMaterialInstance;
    }

    public void UpdateSky(float secondsRemaining, float totalDuration)
    {
        float t = 1f - Mathf.Clamp01(secondsRemaining / totalDuration);
        Color currentColor = skyGradient.Evaluate(t);

        if (skyboxMaterialInstance.HasProperty("_SkyTint"))
            skyboxMaterialInstance.SetColor("_SkyTint", currentColor);

        if (syncFogColor)
            RenderSettings.fogColor = currentColor;
    }
}