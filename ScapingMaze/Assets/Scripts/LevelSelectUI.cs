using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectUI : MonoBehaviour
{
    private int currentScene = 0;
    private GameObject levelViewCamera;
    private AsyncOperation currentLoadOperation;

    void PlayCurrentLevel()
    {
        levelViewCamera.SetActive(false);
        var playerGobj = GameObject.Find("Player");
        if (playerGobj == null)
            Debug.LogError("Couldn't find a Player in the level!");
        else
        {
            var playerScript = playerGobj.GetComponent<Player>();
            playerScript.enabled = true;
            playerScript.cam.SetActive(true);
            Destroy(this.gameObject);
        }
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (currentLoadOperation != null && currentLoadOperation.isDone)
        {
            currentLoadOperation = null;
            levelViewCamera = GameObject.Find("Level View Camera");
            if (levelViewCamera == null)
                Debug.LogError("No level view camera was found in the scene!");
        }
    }

    void OnGUI()
    {
        GUIStyle levelStyle = new GUIStyle(GUI.skin.button);
        levelStyle.alignment = TextAnchor.MiddleCenter;
        levelStyle.fontSize = 25;
        levelStyle.fontStyle = FontStyle.Bold;

        GUILayout.Label("Scape Maze Game ", levelStyle);


        // Check if the current scene is not the Main scene (scene with index 0)
        if (currentScene != 0)
        {
            GUILayout.Label("Currently viewing Level " + currentScene.ToString().ToUpper(), levelStyle);
        }
        else
        {
            GUILayout.Label("Select a level to preview it.");
        }

        // Define the button width and height
        float buttonWidth = 200f;
        float buttonHeight = 150f;

        // Define the vertical space between buttons
        float buttonSpacing = 20f;

        // Define the vertical position of the first button
        float startY = 20f;

        // Starting at scene build index 1, loop through the remaining scene indexes:
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            // Check if the current scene index is not equal to the level being played (currentScene)
            if (i != currentScene)
            {
                // Calculate the horizontal position for the button (right side)
                float startX = Screen.width - buttonWidth - 20f;

                // Draw the level button
                if (GUI.Button(new Rect(startX, startY, buttonWidth, buttonHeight), "LEVEL " + i, levelStyle))
                {
                    if (currentLoadOperation == null)
                    {
                        currentLoadOperation = SceneManager.LoadSceneAsync(i);
                        currentScene = i;
                    }
                }

                // Load and display level thumbnail
                Texture2D thumbnail = Resources.Load<Texture2D>("LevelThumbnails/Level" + i);
                GUI.DrawTexture(new Rect(startX, startY + buttonHeight + buttonSpacing, buttonWidth, buttonHeight), thumbnail, ScaleMode.ScaleToFit);

                // Increment the vertical position for the next button
                startY += buttonHeight + buttonSpacing;
            }
        }
    }
}
