using UnityEngine;
using UnityEngine.SceneManagement;


public class Goal : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
            SceneManager.LoadScene("main");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
