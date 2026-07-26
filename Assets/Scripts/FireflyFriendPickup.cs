using UnityEngine;

public class FireflyFriendPickup : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FireflyLight>() == null) return;

        GameManager.Instance.OnFriendCollected();
        Destroy(gameObject);
    }
}