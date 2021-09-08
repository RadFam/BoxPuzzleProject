using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
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
			// Get Level num
			//int myScene = SceneLoaderManager.inst.CurrSceneNum();
			int myScene = 1;
			string filename = "SceneSav_" + myScene.ToString() + ".ith";
			string savePath = Path.Combine(Application.persistentDataPath, filename);
			if (File.Exists(savePath))
			{
				// Get Dictionary from BinaryFormatter
				Dictionary<string, object> loadData;
				using (FileStream stream = File.Open(savePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                	loadData = (Dictionary<string, object>)formatter.Deserialize(stream);
                }
				BoxMoverManager.inst.FullLoadData(loadData);
			}
		}

		public void OnSave()
		{
			//int myScene = SceneLoaderManager.inst.CurrSceneNum();
			int myScene = 1;
			string filename = "SceneSav_" + myScene.ToString() + ".ith";
			string savePath = Path.Combine(Application.persistentDataPath, filename);

			Dictionary<string, object> saveData = BoxMoverManager.inst.FullSaveData(myScene);
			using (FileStream stream = File.Open(savePath, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, saveData);
            }
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