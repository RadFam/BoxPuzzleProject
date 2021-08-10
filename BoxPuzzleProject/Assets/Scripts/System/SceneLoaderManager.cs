using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UIControls;

public class SceneLoaderManager : MonoBehaviour 
{

	public static SceneLoaderManager inst;
	public enum SceneNames {InitMenuScene, GameLevel_01, GameLevel_02, GameLevel_03, GameLevel_04, GameLevel_05};

	Image fadeImage;

	float fadeAlpha;

	int currScene;
	int sceneToLoad;
	int totalLevels;

	void Awake()
	{
		currScene = 0;
		sceneToLoad = 0;
		totalLevels = Enum.GetNames(typeof(SceneNames)).Length;
	}

    // Use this for initialization
    void Start()
    {
        if (inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void LoadScene(int sceneNum)
	{
		if (sceneNum >= 0)
		{
			sceneToLoad = sceneNum;
			if (sceneNum >= totalLevels)
			{
				sceneToLoad = 0;
			}

			StartCoroutine(StartLoadingScene());
		}
	}

	public void LoadNextScene()
	{
		LoadScene(currScene+1);
	}

	IEnumerator StartLoadingScene()
	{
        yield return StartCoroutine(FadeIn());

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(((SceneNames)sceneToLoad).ToString("g"), LoadSceneMode.Single);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
		currScene = sceneToLoad;

        yield return StartCoroutine(FadeOut());
	}

    IEnumerator FadeIn()
    {
        fadeAlpha = 0.0f;
        Color blackC = Color.black;

        for (int i = 0; i < 25; ++i)
        {
            blackC = new Color(0.0f, 0.0f, 0.0f, fadeAlpha);
            fadeAlpha += 0.04f;
            fadeImage.color = blackC;
            yield return new WaitForSeconds(0.01f);
        }

        blackC = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        fadeImage.color = blackC;

        yield return null;
    }

    IEnumerator FadeOut()
    {
        fadeAlpha = 1.0f;
        Color blackC = Color.black;

        for (int i = 0; i < 25; ++i)
        {
            blackC = new Color(0.0f, 0.0f, 0.0f, fadeAlpha);
            fadeAlpha -= 0.04f;
            fadeImage.color = blackC;
            yield return new WaitForSeconds(0.01f);
        }

        blackC = new Color(0.0f, 0.0f, 0.0f, 0.0f);
        fadeImage.color = blackC;

        yield return null;
    }
}
