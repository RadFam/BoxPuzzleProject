using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayControls;

namespace UIControls
{
    public class GameMenuControl : MonoBehaviour
    {
		[SerializeField]
		GameObject settingsMenu;
        // Use this for initialization
        void Start()
        {

        }

        public void OnProceedClick()
		{
			BoxMoverManager.inst.currPlayer.FreezePlayer(false);
			this.gameObject.SetActive(false);
		}

		public void OnLoadGameClick()
		{
			LevelManager.inst.OnLoad();
			BoxMoverManager.inst.currPlayer.FreezePlayer(false);
			this.gameObject.SetActive(false);
		}

		public void OnSaveGameClick()
		{
			LevelManager.inst.OnSave();
			BoxMoverManager.inst.currPlayer.FreezePlayer(false);
			this.gameObject.SetActive(false);
		}

		public void OnMainMenuClick()
		{
			this.gameObject.SetActive(false);
		}

		public void OnSettingsClick()
		{
			settingsMenu.SetActive(true);
		}
    }
}