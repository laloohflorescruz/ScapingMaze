using UnityEngine;

public class Hazard : MonoBehaviour
{
    // Hazard Script
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
                player.Die();
        }
    }
}
