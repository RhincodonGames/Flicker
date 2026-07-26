using UnityEngine;

public class BatAI : MonoBehaviour
{
    public enum State { Roost, Hunting, Diving }

    [Header("Target")]
    public Transform target; // assigned by GameManager at spawn time

    [Header("Detection")]
    public float attractionRadius = 12f;
    public float brightnessThreshold = 0.3f;
    public float loseInterestRadius = 20f;

    [Header("Movement")]
    public float roostSpeed = 1.5f;
    public float huntSpeed = 4f;
    public float diveSpeedMultiplier = 2.2f;
    public float diveTriggerDistance = 4f;
    public float attackRange = 1f;
    public float turnSpeed = 6f; // bats snap direction faster than birds — twitchier

    [Header("Erratic Flight Jitter")]
    public float jitterStrength = 0.6f;
    public float jitterSpeed = 8f;

    [Header("Roost/Patrol")]
    public float roostRadius = 6f;
    public float newRoostPointInterval = 2.5f; // shorter than a bird's — bats change direction more often

    [Header("Lifetime")]
    public float maxDistanceFromTarget = 40f;

    private State currentState = State.Roost;
    private FireflyLight targetLight;
    private FireflyHealth targetHealth;
    private Vector3 roostOrigin;
    private Vector3 currentRoostPoint;
    private float roostTimer;
    private float jitterPhaseOffset;

    void Start()
    {
        roostOrigin = transform.position;
        jitterPhaseOffset = Random.Range(0f, Mathf.PI * 2f);
        PickNewRoostPoint();

        if (target != null)
        {
            targetLight = target.GetComponent<FireflyLight>();
            targetHealth = target.GetComponent<FireflyHealth>();
        }
    }

    void Update()
    {
        if (target == null) return;

        float distance = Vector3.Distance(transform.position, target.position);

        if (distance > maxDistanceFromTarget)
        {
            Destroy(gameObject);
            return;
        }

        float brightness = targetLight != null ? targetLight.GetBrightness() : 0f;
        UpdateState(distance, brightness);

        switch (currentState)
        {
            case State.Roost:
                DoRoost();
                break;
            case State.Hunting:
                DoHunt();
                break;
            case State.Diving:
                DoDive(distance);
                break;
        }
    }

    void UpdateState(float distance, float brightness)
    {
        bool playerIsBright = brightness >= brightnessThreshold;
        bool playerInRange = distance < attractionRadius;

        if (currentState == State.Roost)
        {
            if (playerIsBright && playerInRange)
                currentState = State.Hunting;
        }
        else
        {
            bool shouldGiveUp = !playerIsBright && distance > loseInterestRadius;
            if (shouldGiveUp)
            {
                currentState = State.Roost;
                roostOrigin = transform.position;
                PickNewRoostPoint();
                return;
            }

            if (currentState == State.Hunting && distance < diveTriggerDistance)
                currentState = State.Diving;
        }
    }

    void DoRoost()
    {
        roostTimer -= Time.deltaTime;
        if (roostTimer <= 0f) PickNewRoostPoint();

        MoveTowards(currentRoostPoint, roostSpeed);
    }

    void DoHunt()
    {
        MoveTowards(target.position, huntSpeed);
    }

    void DoDive(float distance)
    {
        MoveTowards(target.position, huntSpeed * diveSpeedMultiplier);

        if (distance < attackRange)
        {
            targetHealth?.TakeHit();
            Destroy(gameObject);
        }
    }

    void MoveTowards(Vector3 destination, float speed)
    {
        Vector3 direction = (destination - transform.position);
        if (direction.sqrMagnitude < 0.001f) return;

        // Layer a small erratic offset onto the straight-line path — this is what makes it read as "bat" not "bird"
        Vector3 jitterOffset = new Vector3(
            Mathf.Sin(Time.time * jitterSpeed + jitterPhaseOffset),
            Mathf.Cos(Time.time * jitterSpeed * 1.3f + jitterPhaseOffset),
            0f
        ) * jitterStrength;

        Vector3 wobblyDestination = destination + jitterOffset;
        transform.position = Vector3.MoveTowards(transform.position, wobblyDestination, speed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    void PickNewRoostPoint()
    {
        Vector2 randomCircle = Random.insideUnitCircle * roostRadius;
        currentRoostPoint = roostOrigin + new Vector3(randomCircle.x, Random.Range(2f, 5f), randomCircle.y);
        roostTimer = newRoostPointInterval;
    }
}