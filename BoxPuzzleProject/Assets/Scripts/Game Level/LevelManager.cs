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
		public Action OnStepsBackChange;
		public Action OnStepsBackIncrease;
		public Action OnLevelComplete;
		public Action OnTimeIncrease;
		#endregion

		LoadManager loadManager;
		SaveManager saveManager;

		#region StatisticsForLevel
		public int lvlScore;
		public int lvlMaxScore = 0;

		public int currentTime;
		public int prefferableTime;

		public int fixedTime;
		public int levelStepsBack;
		public int addStepsBack;
		public int bonusSteps;
		public int deltaStepsScore;
		#endregion

		SoundResources soundResources;
        
		[SerializeField]
		EndLevelEffect endLevelEffect;
		[SerializeField]
		AddBackStepEffect addBackStepEffect;
		
		[SerializeField]
		AudioSource myAudioEffects;

		float deltaTimer;

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
			levelStepsBack = 3;
			bonusSteps = 0;
			addStepsBack = GameManager.inst.cummSteps;
			currentTime = 0;
			fixedTime = 0;
			Debug.Log("Start of LevelManager");
			prefferableTime = GameManager.inst.GetPrefLevelTime();
			deltaTimer = 0.0f;
        }

		void Update()
		{
			deltaTimer += Time.deltaTime;
			if (deltaTimer >= 1.0f)
			{
				deltaTimer = 0.0f;
				currentTime += 1;
				OnTimeIncrease();
			}
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
			if (addStepsBack > 0)
			{
				addStepsBack -= 1;
				BoxMoverManager.inst.RestorePrevoiusStep();
				OnStepsBackChange();

				return;
			}
			if (addStepsBack == 0 && levelStepsBack > 0)
			{
				levelStepsBack -= 1;
				BoxMoverManager.inst.RestorePrevoiusStep();
				OnStepsBackChange();
				return;
			}
		}

		public void IncreaseBackSteps(int inc, bool showEffects=true)
		{
			addStepsBack += inc;

			// Start Effect
			if (showEffects)
			{
				addBackStepEffect.StartAddbackStepEffects();
				OnStepsBackIncrease();
			}
		}

		public void SetBackSteps(int val_1, int val_2)
		{
			levelStepsBack = val_1;
			addStepsBack = val_2;
			OnStepsBackChange();
		}

		public void ChangeLevelScore(int change, bool original=true)
		{
			lvlScore += change;
			OnScoreChange();

			if (original && change > 0 && lvlScore % deltaStepsScore == 0)
			{
				IncreaseBackSteps(1, true);
			}

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

			// Check, how much additional steps we can add to us
			fixedTime = currentTime;
			float reachTime = (float)currentTime;
			if (prefferableTime/reachTime >= 1.0f && prefferableTime/reachTime < 1.1f)
			{
				bonusSteps = 1;
			}
			if (prefferableTime/reachTime >= 1.1f && prefferableTime/reachTime < 1.2f)
			{
				bonusSteps = 2;
			}
			if (prefferableTime/reachTime >= 1.2f && prefferableTime/reachTime < 1.5f)
			{
				bonusSteps = 5;
			}
			if (prefferableTime/reachTime >= 1.5f)
			{
				bonusSteps = 10;
			}
			addStepsBack += bonusSteps;

			// Show WIN effect
			StopPlayEffect();
			PlayEffect(EffectsNames.WinRound);
			endLevelEffect.StartWinEffect(bonusSteps > 0);	
		}

		public void EndPlayScene()
		{
			// Save level progress (that we pass this level)
			// Load next scene
			SceneLoaderManager.inst.LoadNextScene();
		}
    }
}