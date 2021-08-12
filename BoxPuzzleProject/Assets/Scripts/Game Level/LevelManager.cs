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

		FieldManager fieldManager;
		LoadManager loadManager;
		SaveManager saveManager;

		public int lvlScore;
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
			fieldManager = GetComponent<FieldManager>();
			loadManager = GetComponent<LoadManager>();
			saveManager = GetComponent<SaveManager>();
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
    }
}