using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;

namespace GameControls
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager inst;
		
		#region GlobalGameSettings
		public int currScore;
		public int cummSteps;
		public int openLevels;
		public float currMusicVol;
		public float currEffectsVol;
		public bool saveProgress;
		string saveFilename;

		public Action OnSettingsChange;

		Dictionary<string, object> savebleGameSettings;
		#endregion
		[SerializeField]
		List<int> prefLevelTimes;

		[SerializeField]
		AudioSource myAudioMusic;
		SoundResources soundResources;
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

			savebleGameSettings = new Dictionary<string, object>();
			saveFilename = "SettingsBM.ith";

			soundResources = Resources.Load<SoundResources>("ScriptableObjects/SoundData");

			LoadGame();
        }

		public void SaveGame()
		{
			if (saveProgress)
			{
				savebleGameSettings.Clear();
				savebleGameSettings.Add("ScoreValue", (object)currScore);
				savebleGameSettings.Add("StepsBack", (object)cummSteps);
				savebleGameSettings.Add("MusicVolume", (object)currMusicVol);
				savebleGameSettings.Add("EffectsVolume", (object)currEffectsVol);
				savebleGameSettings.Add("OpenedLevels", (object)openLevels);

                string savePath = Path.Combine(Application.persistentDataPath, saveFilename);
                using (FileStream stream = File.Open(savePath, FileMode.Create))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    formatter.Serialize(stream, savebleGameSettings);
                }
            }
		}

		public void LoadGame()
		{
			string savePath = Path.Combine(Application.persistentDataPath, saveFilename);
			savebleGameSettings.Clear();
			// Check, if file exists
			if (File.Exists(savePath))
			{
				using (FileStream stream = File.Open(savePath, FileMode.Open))
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                	savebleGameSettings = (Dictionary<string, object>)formatter.Deserialize(stream);
                }
				currScore = (int)savebleGameSettings["ScoreValue"];
				cummSteps = (int)savebleGameSettings["StepsBack"];
				currMusicVol = (int)savebleGameSettings["MusicVolume"];
				currEffectsVol = (int)savebleGameSettings["EffectsVolume"];
				openLevels = (int)savebleGameSettings["OpenedLevels"];

				return;
			}

			// Else restore data from Scriptable Object with default settings
			DefaultSettings ds = Resources.Load("ScriptableObjects/SettingsData") as DefaultSettings;
			currScore = ds.defaultScore;
			currMusicVol = ds.defaultMusic;
			currEffectsVol = ds.defaultEffects;
			openLevels = ds.defaultLevel;
			cummSteps = 0;
		}

		public void LoadLevelGame()
		{
			// Load saved level (if it was saved)
		}

		public void ReachLevelNum(int num)
		{
			if (num > openLevels)
			{
				openLevels = num;
			}
		}

		public int GetPrefLevelTime()
		{
			int currScene = SceneLoaderManager.inst.NextSceneNum();
			return prefLevelTimes[currScene-1];
		}

		public void ExitGame()
		{
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
			Application.Quit();
#endif
        }
    }
}