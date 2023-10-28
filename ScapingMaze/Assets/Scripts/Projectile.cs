using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Header("References")]
    public Transform trans;

    [Header("Stats")]
    public float speed = 34;
    public float range = 70;
    private bool hasCollided = false;
    private Vector3 spawnPoint;

    void Start()
    {
        spawnPoint = trans.position;
    }

    void Update()
    {
        if (hasCollided)
        {
            // If the projectile has collided, do nothing and return
            return;
        }

        // Move the projectile along its local Z axis (forward):
        trans.Translate(0, 0, speed * Time.deltaTime, Space.Self);

        // Check if the projectile has hit something:
        RaycastHit hit;
        if (Physics.Raycast(trans.position, trans.forward, out hit, speed * Time.deltaTime))
        {
            // Check if the object hit has a name containing "Wall":
            if (hit.collider.gameObject.name.Contains("Wall"))
            {
                // Reset the projectile position
                trans.position = spawnPoint;
                hasCollided = true; // Set the flag to true to indicate collision
            }
        }

        // Destroy the projectile if it has traveled to or past its range:
        if (Vector3.Distance(trans.position, spawnPoint) >= range)
        {
            ResetProjectile();
        }
    }

    public void ResetProjectile()
    {
        // Reset the collision flag when you want to start shooting again
        hasCollided = false;
    }
}
