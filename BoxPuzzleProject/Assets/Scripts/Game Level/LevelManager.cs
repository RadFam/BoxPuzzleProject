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
		public enum EffectsNames { WalkStomp, DragBox, WinRound};

		#region ActionsDuringLevelPlay
		public Action OnScoreChange;
		public Action OnLevelComplete;
		#endregion

		LoadManager loadManager;
		SaveManager saveManager;

		public int lvlScore;
		public int lvlMaxScore = 0;

		SoundResources soundResources;
        
		[SerializeField]
		EndLevelEffect endLevelEffect;
		
		[SerializeField]
		AudioSource myAudioEffects;

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

			soundResources = Resources.Load<SoundResources>("ScriptableObjects/SoundData");
			
			// Temporary turn off(!!!)
			//myAudioEffects.volume = GameManager.inst.currEffectsVol;
			//GameManager.inst.OnSettingsChange += ChangeEffectsVol;

			lvlScore = 0;
        }

		public void ChangeEffectsVol()
		{
			myAudioEffects.volume = GameManager.inst.currEffectsVol;
		}

		public void PlayEffect(EffectsNames en)
		{
			if (en == EffectsNames.WalkStomp)
			{
				myAudioEffects.loop = true;
				myAudioEffects.clip = soundResources.stompEffect;
				myAudioEffects.Play();
				return;
			}

			if (en == EffectsNames.DragBox)
			{
				myAudioEffects.loop = true;
				myAudioEffects.clip = soundResources.dragEffect;
				myAudioEffects.Play();
				return;
			}

			if (en == EffectsNames.WinRound)
			{
				myAudioEffects.loop = false;
				myAudioEffects.clip = soundResources.winEffect;
				myAudioEffects.Play();
				return;
			}
		}

		public void StopPlayEffect()
		{
			myAudioEffects.Stop();
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
			StopPlayEffect();
			PlayEffect(EffectsNames.WinRound);
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