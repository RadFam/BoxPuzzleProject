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

		float deltaTimer;
		bool stepAhead;
        // Use this for initialization
        void Start()
        {
			LevelManager.inst.OnScoreChange += ChangeScore;
			deltaTimer = 0.0f;
			stepAhead = true;
        }

		void Update()
		{
			deltaTimer += Time.deltaTime;
			if (deltaTimer >= 2.0f)
			{
				deltaTimer = 0.0f;
				stepAhead = true;
			}
		}
		public void OnCloseIntroScene()
		{
			GameMenu.SetActive(false);
            SettingsMenu.SetActive(false);
		}
        public void OnStepAheadClick()
		{
			if (stepAhead)
			{
				LevelManager.inst.OnStepBack();
				stepAhead = false;
				deltaTimer = 0.0f;
			}
		}

		public void OnGearClick()
		{
			BoxMoverManager.inst.currPlayer.FreezePlayer(true);
			GameMenu.SetActive(true);
		}

		public void ChangeScore()
		{
			scoreText.text = "Score: " + LevelManager.inst.lvlScore.ToString();
		}
    }
}