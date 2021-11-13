using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueButton : MonoBehaviour
{

    int sceneToContinue;

    public void ContinueGame()
	{
		sceneToContinue = PlayerPrefs.GetInt("SavedScene");
		if (sceneToContinue != 0)
		{
			SceneManager.LoadScene(sceneToContinue);
			AudioManager.instance.Play("BackgroundMusic");
			Time.timeScale = 1f;
		}
		else
			return;
	}
}
