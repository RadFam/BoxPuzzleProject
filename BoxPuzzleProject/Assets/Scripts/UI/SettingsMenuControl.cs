using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameControls;

namespace UIControls
{
    public class SettingsMenuControl : MonoBehaviour
    {
		public Sprite[] chkBoxSprites = new Sprite[2];
		[SerializeField]
		public Image chkBoxImage;
		[SerializeField]
		Slider musicVolSlider;
		[SerializeField]
		Slider effectsVolSlider;
        // Use this for initialization

		void Awake()
		{
			musicVolSlider.onValueChanged.AddListener(AlterMusicVol);
			effectsVolSlider.onValueChanged.AddListener(AlterEffectsVol);
		}
        void OnEnable()
        {
			musicVolSlider.maxValue = 1.0f;
			effectsVolSlider.maxValue = 1.0f;

			musicVolSlider.value = GameManager.inst.currMusicVol;
			effectsVolSlider.value = GameManager.inst.currEffectsVol;
			if (GameManager.inst.saveProgress)
			{
				chkBoxImage.sprite = chkBoxSprites[1];
			}
			else
			{
				chkBoxImage.sprite = chkBoxSprites[0];
			}
        }

		public void OnCloseSettings()
		{
			gameObject.SetActive(false);
		}

		public void OnChkBoxClick()
		{
			bool sp = GameManager.inst.saveProgress;
			GameManager.inst.saveProgress = !sp;
			if (sp)
			{
				chkBoxImage.sprite = chkBoxSprites[1];
			}
			else
			{
				chkBoxImage.sprite = chkBoxSprites[0];
			}
		}

		void AlterMusicVol(float val)
		{
			GameManager.inst.currMusicVol = val;
			// Also we need to apply changes to music immediately
		}

		void AlterEffectsVol(float val)
		{
			GameManager.inst.currEffectsVol = val;
		}

    }
}