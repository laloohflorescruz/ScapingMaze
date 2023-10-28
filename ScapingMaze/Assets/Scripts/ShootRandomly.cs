
using UnityEngine;

public class ShootRandomly : MonoBehaviour
{
    [Header("References")]
    public Transform spawnPoint;
    public GameObject projectilePrefab;

    [Header("Stats")]
    [Tooltip("Time, in seconds, between the firing of each projectile.")]
    public float fireRate = 1;
    private float lastFireTime = 0;

    void Update()
    {
        if (Time.time >= lastFireTime + fireRate)
        {
            lastFireTime = Time.time;

            // Generate a random rotation for the spawn point
            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            // Instantiate the projectile with the random rotation
            Instantiate(projectilePrefab, spawnPoint.position, randomRotation);
        }
    }
}
