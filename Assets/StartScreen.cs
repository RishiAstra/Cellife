using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class StartScreen : MonoBehaviour {

	public void NextScene()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings-1? SceneManager.GetActiveScene().buildIndex + 1:0);
	}
}

