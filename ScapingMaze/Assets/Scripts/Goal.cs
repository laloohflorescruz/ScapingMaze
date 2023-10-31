using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public float rotationGoal = 150f;

    public AudioSource winSound;


    void Start()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            if (winSound != null)
            {
                winSound.Play();
            }
            // Load the "main" scene after a delay
            Invoke("LoadMainScene", 1f); // Load the scene after 2 seconds (adjust the delay as needed)
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
