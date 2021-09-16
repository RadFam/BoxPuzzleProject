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
		[SerializeField]
		Text currTime;
		[SerializeField]
		Text prefTime;
		[SerializeField]
		Image backStars;
		[SerializeField]
		Text backSteps;
		[SerializeField]
		ChangeScoreEffect changeScoreEffect;
		[SerializeField]
		List<Sprite> starBackSteps;

		float deltaTimer;
		bool stepAhead;
        // Use this for initialization
        void Start()
        {
			LevelManager.inst.OnScoreChange += ChangeScore;
			LevelManager.inst.OnStepsBackChange += ChangeBackSteps;
			LevelManager.inst.OnStepsBackIncrease += ChangeBackSteps;
			LevelManager.inst.OnTimeIncrease += ChangePlayTime;
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

		public void ChangeBackSteps()
		{
			if (LevelManager.inst.levelStepsBack == 3 && LevelManager.inst.addStepsBack >= 0)
			{
				backStars.sprite = starBackSteps[LevelManager.inst.levelStepsBack];
				backSteps.text = " +" + LevelManager.inst.addStepsBack.ToString();
			}
			if (LevelManager.inst.levelStepsBack <= 3 && LevelManager.inst.addStepsBack == 0)
			{
				backStars.sprite = starBackSteps[LevelManager.inst.levelStepsBack];
			}
		}

		public void ChangePlayTime()
		{
			currTime.text = LevelManager.inst.currentTime.ToString();
			prefTime.text = LevelManager.inst.prefferableTime.ToString();
		}
    }
}