using UnityEngine;
using UnityEngine.UI;

public class LivesUI : MonoBehaviour
{
    public Image[] lifeIcons; // drag LifeIcon_0 through _9 in, in order
    [Range(0f, 1f)] public float lostAlpha = 0.25f;

    public void UpdateLives(int maxLives, int currentLives)
    {
        maxLives = Mathf.Clamp(maxLives, 0, lifeIcons.Length);

        for (int i = 0; i < lifeIcons.Length; i++)
        {
            bool unlocked = i < maxLives;
            lifeIcons[i].gameObject.SetActive(unlocked);

            if (unlocked)
            {
                bool alive = i < currentLives;
                Color c = lifeIcons[i].color;
                c.a = alive ? 1f : lostAlpha;
                lifeIcons[i].color = c;
            }
        }
    }
}