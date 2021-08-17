using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		public int lvlMaxScore;
        // Use this for initialization

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

			lvlMaxScore = 0;
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

		}

		public void ChangeLevelScore(int change)
		{
			
		}
    }
}