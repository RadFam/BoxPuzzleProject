using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameControls;

namespace UIControls
{
    public class MainMenuControl : MonoBehaviour
    {

        [SerializeField]
        GameObject LevelChooseMenu;
        [SerializeField]
        GameObject SettingsChooseMenu;
        // Use this for initialization
        void Start()
        {

        }

        public void OnLoadIntroScene()
		{

		}

		public void OnCloseIntroScene()
		{
			LevelChooseMenu.SetActive(false);
            SettingsChooseMenu.SetActive(false);
		}

        // Handling button clicks
        public void OnChooseLevelClick()
        {
            LevelChooseMenu.SetActive(true);
        }

        public void OnLoadGameClick()
        {
            GameManager.inst.LoadLevelGame();
        }

        public void OnSettingsOpenClick()
        {
            SettingsChooseMenu.SetActive(true);
        }

        public void OnExitGameClick()
        {
            GameManager.inst.ExitGame();
        }

        public void OnLoadLevel(int lvl)
        {
            OnCloseIntroScene();
            SceneLoaderManager.inst.LoadScene(lvl);
        }
    }
}