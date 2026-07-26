using UnityEngine;

public class FireflyFriendPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        FireflyHealth health = other.GetComponent<FireflyHealth>();
        if (health == null) return;

        if (health.IsAtMaxLives) return;

        GameManager.Instance.OnFriendCollected();
        Destroy(gameObject);
    }
}