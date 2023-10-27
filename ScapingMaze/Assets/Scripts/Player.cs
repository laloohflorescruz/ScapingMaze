using UnityEngine;

public class Player : MonoBehaviour
{
    // References
    [Header("References")]
    public Transform trans;
    public Transform modelTrans;
    public CharacterController characterController;

    // Movement
    [Header("Movement")]
    [Tooltip("Units moved per second at maximum speed.")]
    public float movespeed = 24;

    [Tooltip("Time, in seconds, to reach maximum speed.")]
    public float timeToMaxSpeed = .26f;

    private float VelocityGainPerSecond { get { return movespeed / timeToMaxSpeed; } }

    [Tooltip("Time, in seconds, to go from maximum speed to stationary.")]
    public float timeToLoseMaxSpeed = .2f;

    private float VelocityLossPerSecond { get { return movespeed / timeToLoseMaxSpeed; } }

    [Tooltip("Multiplier for momentum when attempting to move in a direction opposite the current traveling direction.")]
    public float reverseMomentumMultiplier = 2.2f;

    private Vector3 movementVelocity = Vector3.zero;

    [Tooltip("Time after dashing finishes before it can be performed again.")]
    public float dashCooldown = 1.8f;

    // Death and Respawning Variables
    [Header("Death and Respawning")]
    [Tooltip("How long after the player's death, in seconds, before they are respawned?")]
    public float respawnWaitTime = 2f;
    private bool dead = false;
    private Vector3 spawnPoint;


    // Specify the layers you want to ignore collisions for
    public int layer1; // The first layer
    public int layer2; // The second layer
    public bool ignore; // Set to true to ignore collisions, false to enable collisions

    [Header("Dashing")]
    [Tooltip("Total distance traveled during a dash.")]



    private Quaternion spawnRotation;
    void Start()
    {
        spawnPoint = transform.position;
        spawnRotation = modelTrans.rotation;
        // Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        // rb.isKinematic = true;
        gameObject.layer = LayerMask.NameToLayer("LayerName");
        Physics.IgnoreLayerCollision(layer1, layer2, ignore);

    }


    private bool CanDashNow
    {

        get
        {
            return (Time.time > dashBeginTime + dashTime + dashCooldown);
        }


    }


    public float dashDistance = 17;
    [Tooltip("Time taken for a dash (in seconds).")]
    public float dashTime = .26f;

    private bool IsDashing
    {

        get
        {
            return (Time.time < dashBeginTime + dashTime);
        }
    }

    private Vector3 dashDirection;
    private float dashBeginTime = Mathf.NegativeInfinity;

    private void Dashing()
    {
        if (!IsDashing && CanDashNow)
        //If not dashing right now
        {
            if (Input.GetKey(KeyCode.Space)) //If space key is pressed
            {
                Vector3 movementDir = Vector3.zero;

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                    movementDir.z = 1;
                else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                    movementDir.z = -1;

                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                    movementDir.x = 1;
                else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                    movementDir.x = -1;

                if (movementDir.x != 0 || movementDir.z != 0)
                {
                    dashDirection = movementDir;
                    dashBeginTime = Time.time;
                    movementVelocity = dashDirection * movespeed;
                    modelTrans.forward = dashDirection;
                }
            }
        }
        else //If dashing
        {
            characterController.Move(dashDirection * (dashDistance / dashTime) * Time.deltaTime);
        }
    }



    // Method to handle movement logic
    private void Movement()
    {
        // Forward Movement (Z Axis)
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            if (movementVelocity.z >= 0)
                movementVelocity.z = Mathf.Min(movespeed, movementVelocity.z + VelocityGainPerSecond * Time.deltaTime);
            else
                movementVelocity.z = Mathf.Min(0, movementVelocity.z + VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
        }

        // Backward Movement (Z Axis)
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            if (movementVelocity.z > 0)
                movementVelocity.z = Mathf.Max(0, movementVelocity.z - VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
            else
                movementVelocity.z = Mathf.Max(-movespeed, movementVelocity.z - VelocityGainPerSecond * Time.deltaTime);
        }

        // If neither forward nor back are being held
        else
        {
            if (movementVelocity.z > 0)
                movementVelocity.z = Mathf.Max(0, movementVelocity.z - VelocityLossPerSecond * Time.deltaTime);
            else
                movementVelocity.z = Mathf.Min(0, movementVelocity.z + VelocityLossPerSecond * Time.deltaTime);
        }

        // Right Movement (X Axis)
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (movementVelocity.x >= 0)
                movementVelocity.x = Mathf.Min(movespeed, movementVelocity.x + VelocityGainPerSecond * Time.deltaTime);
            else
                movementVelocity.x = Mathf.Min(0, movementVelocity.x + VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
        }

        // Left Movement (X Axis)
        else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (movementVelocity.x > 0)
                movementVelocity.x = Mathf.Max(0, movementVelocity.x - VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
            else
                movementVelocity.x = Mathf.Max(-movespeed, movementVelocity.x - VelocityGainPerSecond * Time.deltaTime);
        }

        // If neither right nor left are being held
        else
        {
            if (movementVelocity.x > 0)
                movementVelocity.x = Mathf.Max(0, movementVelocity.x - VelocityLossPerSecond * Time.deltaTime);
            else
                movementVelocity.x = Mathf.Min(0, movementVelocity.x + VelocityLossPerSecond * Time.deltaTime);
        }

        if (movementVelocity.x != 0 || movementVelocity.z != 0)
        {
            characterController.Move(movementVelocity * Time.deltaTime);
            modelTrans.rotation = Quaternion.Slerp(modelTrans.rotation, Quaternion.LookRotation(movementVelocity), .18F);
        }

    }

    // Call the Movement method in the Update method
    private void Update()
    {


        Movement();
        Dashing();

    }

    public void Die()
    {
        if (!dead)
        {
            dead = true;
            Invoke("Respawn", respawnWaitTime);
            movementVelocity = Vector3.zero;
            enabled = false;
            characterController.enabled = false;
            modelTrans.gameObject.SetActive(false);
        }
        dashBeginTime = Mathf.NegativeInfinity;

    }
    public void Respawn()
    {
        dead = false;
        trans.position = spawnPoint;
        enabled = true;
        characterController.enabled = true;
        modelTrans.gameObject.SetActive(true);
        modelTrans.rotation = spawnRotation;

    }

    // Remove Box Collider component
    void RemoveBoxCollider(GameObject gameObject)
    {
        BoxCollider collider = gameObject.GetComponent<BoxCollider>();
        if (collider != null)
        {
            Destroy(collider);
        }
    }
}
