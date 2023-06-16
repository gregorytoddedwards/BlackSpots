using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Finish : MonoBehaviour
{
	private Scene currentScene;

	void Start()
	{
		currentScene = SceneManager.GetActiveScene();
	}    
	
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player")
		{
			LoadThisLevel();
		}
	}

	public void LoadLevel(int levelNumber)
    {
        SceneManager.LoadScene(levelNumber);
    }

    public void LoadThisLevel()
    {
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(currentScene.buildIndex + 1);
    }

    public void LoadPreviousLevel()
    {
        SceneManager.LoadScene(currentScene.buildIndex - 1);
    }
}
