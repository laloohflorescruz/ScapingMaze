using UnityEngine;

public class WanderRegion : MonoBehaviour
{
    [Tooltip("Size of the box.")]
    public Vector3 size;

    [HideInInspector]
    public WanderRegion region;

    public Vector3 GetRandomPointWithin()
    {
        float x = transform.position.x + Random.Range(size.x * -.5f, size.x * .5f);
        float z = transform.position.z + Random.Range(size.z * -.5f, size.z * .5f);
        return new Vector3(x, transform.position.y, z);
    }

    public bool IsPointWithinRegion(Vector3 point)
    {
        // Check if the point is within the region's bounds
        return point.x >= transform.position.x - size.x * 0.5f && point.x <= transform.position.x + size.x * 0.5f &&
               point.z >= transform.position.z - size.z * 0.5f && point.z <= transform.position.z + size.z * 0.5f;
    }

    void Awake()
    {
        // Get all of our Wanderer children:
        var wanderers = gameObject.GetComponentsInChildren<WanderRegion>();
        // Loop through the children:
        for (int i = 0; i < wanderers.Length; i++)
        {
            // Set their .region reference to 'this' script instance:
            wanderers[i].region = this;
        }
    }
}
