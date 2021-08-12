using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
			this.gameObject.SetActive(false);
		}

		public void OnLoadGameClick()
		{
			this.gameObject.SetActive(false);
		}

		public void OnSaveGameClick()
		{
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