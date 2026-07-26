using UnityEngine;

public class SwarmFollower : MonoBehaviour
{
    public Transform target; // the player
    public int indexInSwarm;
    public float orbitRadius = 1.5f;
    public float orbitSpeed = 1f;
    public float followLerp = 5f;

    void Update()
    {
        if (target == null) return;

        float angle = (Time.time * orbitSpeed) + (indexInSwarm * (Mathf.PI * 2f / 10f));
        Vector3 offset = new Vector3(Mathf.Cos(angle), 0.3f, Mathf.Sin(angle)) * orbitRadius;
        Vector3 desiredPos = target.position + offset;

        transform.position = Vector3.Lerp(transform.position, desiredPos, followLerp * Time.deltaTime);
    }
}