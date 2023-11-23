using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitch : MonoBehaviour
{
    private int numberOfScene = 4;
    private int currentScene = 0;
    private int nextScene = 0;
    
    private void Start()
    {
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        nextScene = (currentScene + 1) % numberOfScene;
    }
    
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SwitchScene();
        }
    }

    private void SwitchScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
        currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;
        nextScene = (currentScene + 1) % numberOfScene;
    }
}
