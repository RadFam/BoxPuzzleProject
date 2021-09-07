using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameControls;

namespace PlayControls
{
    public class LevelManager : MonoBehaviour
    {
		public static LevelManager inst;

		#region ActionsDuringLevelPlay
		public Action OnScoreChange;
		public Action OnLevelComplete;
		#endregion

		LoadManager loadManager;
		SaveManager saveManager;

		public int lvlScore;
		public int lvlMaxScore = 0;
        
		[SerializeField]
		EndLevelEffect endLevelEffect;

		void Awake()
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
        void Start()
        {
			loadManager = GetComponent<LoadManager>();
			saveManager = GetComponent<SaveManager>();

			lvlScore = 0;
        }

        public void OnLoad()
		{

		}

		public void OnSave()
		{

		}

		public void OnStepBack()
		{
			BoxMoverManager.inst.RestorePrevoiusStep();
		}

		public void ChangeLevelScore(int change)
		{
			lvlScore += change;
			OnScoreChange();

			Debug.Log("Max score: " + lvlMaxScore);
			// Check if we has max score
			if (lvlScore == lvlMaxScore)
			{
				OnMaxScoreGain();
			}
		}

		void OnMaxScoreGain()
		{
			// Stop walking player
			BoxMoverManager.inst.currPlayer.FreezePlayer(true);

			// Show WIN effect
			endLevelEffect.StartWinEffect();	
		}

		public void EndPlayScene()
		{
			// Save level progress (that we pass this level)
			// Load next scene
			SceneLoaderManager.inst.LoadNextScene();
		}
    }
}