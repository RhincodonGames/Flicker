using UnityEngine;

public class DayNightDial : MonoBehaviour
{
    public RectTransform clockDial;
    public bool rotateCounterClockwise = true;

    public void UpdateDial(float secondsRemaining, float totalDuration)
    {
        float t = 1f - Mathf.Clamp01(secondsRemaining / totalDuration); // 0 at start, 1 at dawn
        float angle = t * 360f * (rotateCounterClockwise ? 1f : -1f);
        clockDial.localRotation = Quaternion.Euler(0f, 0f, angle);
    }
}