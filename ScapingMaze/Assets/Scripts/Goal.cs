using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public float rotationGoal = 150f;
    public AudioClip winSound; // Assign your audio clip in the Unity editor
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component attached to this GameObject
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            // Play the win sound
            if (winSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(winSound);
            }

            // Load the "main" scene after a delay
            Invoke("LoadMainScene", 2f); // Load the scene after 2 seconds (adjust the delay as needed)
        }
    }

    void Update()
    {
        float angle = rotationGoal * Time.deltaTime;
        transform.Rotate(Vector3.down * angle, Space.World);
    }

    void LoadMainScene()
    {
        SceneManager.LoadScene("main");
    }
}
