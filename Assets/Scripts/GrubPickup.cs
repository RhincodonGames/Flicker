using UnityEngine;

public class GrubPickup : MonoBehaviour
{
    public float fuelRestored = 8f;

    void OnTriggerEnter(Collider other)
    {
        FireflyLight light = other.GetComponent<FireflyLight>();
        if (light == null) return;

        light.AddFuel(fuelRestored);
        Destroy(gameObject);
    }
}