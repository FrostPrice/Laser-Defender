using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    float delayedSeconds = 2.5f;
    public void LoadStartMenu()
    {
        SceneManager.LoadScene(0); // Remember: You can use the name of the scene as a String to load the Scene
    }

    public void LoadGame()
    {
        FindObjectOfType<GameSession>().ResetScore();
        SceneManager.LoadScene(1);
    }

    public void LoadGameOver()
    {
        StartCoroutine(WaitAndLoad());
    }

    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(delayedSeconds);
        SceneManager.LoadScene(2);
    }

    public void LoadInstructions()
    {
        SceneManager.LoadScene(3);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
