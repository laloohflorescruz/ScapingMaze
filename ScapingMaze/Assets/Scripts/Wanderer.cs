using UnityEngine;

public class Wanderer : MonoBehaviour
{
    private enum State
    {
        Idle,
        Rotating,
        Moving
    }

    private State state = State.Idle;
    [HideInInspector] public WanderRegion region;

    [Header("References")]
    public Transform trans;
    public Transform modelTrans;

    [Header("Stats")]
    public float movespeed = 18;
    [Tooltip("Minimum wait time before retargeting again.")]
    public float minRetargetInterval = 4.4f;
    [Tooltip("Maximum wait time before retargeting again.")]
    public float maxRetargetInterval = 6.2f;
    [Tooltip("Time in seconds taken to rotate after targeting, before moving begins.")]
    public float rotationTime = .6f;
    [Tooltip("Time in seconds after rotation finishes before movement starts.")]
    public float postRotationWaitTime = .3f;

    private Vector3 currentTarget; // Position we're currently targeting
    private Quaternion initialRotation; // Our rotation when we first retargeted
    private Quaternion targetRotation; // The rotation we're aiming to reach
    private float rotationStartTime; // Time.time at which we started rotating


    // Called on Start and invokes itself again after each call.
    // Each invoke will wait a random time within the retarget interval.

    void Start()
    {
        Retarget();
    }


    void Update()
    {
        if (state == State.Moving)
        {
            // Measure the distance between current position and target position
            float distanceToTarget = Vector3.Distance(trans.position, currentTarget);
            // Calculate step based on moveSpeed and time.deltaTime
            float step = movespeed * Time.deltaTime;
            // Move towards the target by the step
            trans.position = Vector3.MoveTowards(trans.position, currentTarget, step);

            // If the distance to the target is very small, stop moving and retarget
            if (distanceToTarget < step)
            {
                state = State.Idle;
            }
        }
        else if (state == State.Rotating)
        {
            // Measure the time we've spent rotating so far, in seconds
            float timeSpentRotating = Time.time - rotationStartTime;
            // Rotate from initialRotation towards targetRotation
            modelTrans.rotation = Quaternion.Slerp(initialRotation, targetRotation, timeSpentRotating / rotationTime);

            // If rotation time is exceeded, start moving
            if (timeSpentRotating >= rotationTime)
            {
                BeginMoving();
            }
        }
    }

    void BeginMoving()
    {
        // Set state to Moving
        state = State.Moving;
    }
    void Retarget()
    {
        // Get a random target point within the region
        currentTarget = region.GetRandomPointWithin();
        // Calculate target rotation towards the new target position
        targetRotation = Quaternion.LookRotation((currentTarget - trans.position).normalized);
        // Set state to Rotating
        state = State.Rotating;
        // Mark rotation start time
        rotationStartTime = Time.time;
    }
}
