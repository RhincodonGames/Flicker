using UnityEngine;

public class GrubWander : MonoBehaviour
{
    public float wanderRadius = 1.5f;
    public float moveSpeed = 0.5f;
    public float newTargetInterval = 4f;
    public float groundY;

    private Vector3 spawnOrigin;
    private Vector3 currentTarget;
    private float targetTimer;

    void Start()
    {
        spawnOrigin = transform.position;
        groundY = transform.position.y; // lock to whatever height it spawned at
        PickNewTarget();
    }

    void Update()
    {
        targetTimer -= Time.deltaTime;
        if (targetTimer <= 0f) PickNewTarget();

        Vector3 flatTarget = new Vector3(currentTarget.x, groundY, currentTarget.z);
        transform.position = Vector3.MoveTowards(transform.position, flatTarget, moveSpeed * Time.deltaTime);

        // Face movement direction so it doesn't look like it's sliding sideways
        Vector3 toTarget = flatTarget - transform.position;
        if (toTarget.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(toTarget), 5f * Time.deltaTime);
    }

    void PickNewTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        currentTarget = spawnOrigin + new Vector3(randomCircle.x, 0f, randomCircle.y);
        targetTimer = newTargetInterval;
    }
}