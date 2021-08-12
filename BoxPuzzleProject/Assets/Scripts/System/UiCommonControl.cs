using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIControls;

namespace GameControls
{
    public class UiCommonControl : MonoBehaviour
    {

		public static UiCommonControl inst;
        void Start()
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

		public void OnCloseScene(int scene)
		{
			if (scene == 0)
			{
				OnMenuSceneClose();
			}
			else
			{
				OnLevelSceneClose();
			}
		}
        void OnLevelSceneClose()
        {

        }
        void OnMenuSceneClose()
        {
            GameObject go = GameObject.Find("LevelMenu");
            if (go != null)
            {
                go.SetActive(false);
            }
        }
    }
}