using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameControls;
using PlayControls;

namespace UIControls
{
    public class LevelUIControl : MonoBehaviour
    {
		[SerializeField]
		GameObject SettingsMenu;
		[SerializeField]
		GameObject GameMenu;
		[SerializeField]
		Text scoreText;
        // Use this for initialization
        void Start()
        {
			LevelManager.inst.OnScoreChange += ChangeScore;
        }
		public void OnCloseIntroScene()
		{
			GameMenu.SetActive(false);
            SettingsMenu.SetActive(false);
		}
        public void OnStepAheadClick()
		{
			LevelManager.inst.OnStepBack();
		}

		public void OnGearClick()
		{
			GameMenu.SetActive(true);
		}

		public void ChangeScore()
		{
			scoreText.text = "Score: " + LevelManager.inst.lvlScore.ToString();
		}
    }
}