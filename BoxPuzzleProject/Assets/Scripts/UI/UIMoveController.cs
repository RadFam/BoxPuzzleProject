using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayControls;

namespace UIControls
{
    public class UIMoveController : MonoBehaviour
    {
		[SerializeField]
		Button leftBtn;
		[SerializeField]
		Button downBtn;
		[SerializeField]
		Button upBtn;
		[SerializeField]
		Button rightBtn;

		public Button LeftButton{get{return leftBtn;}}
		public Button DownButton{get{return downBtn;}}
		public Button UpButton{get{return upBtn;}}
		public Button RightButton{get{return rightBtn;}}
        // Use this for initialization
        void Start()
        {
			PlayerController pc = GameObject.Find("PlayerController").GetComponent<PlayerController>();
			pc.SetUIMoves(this);
        }
    }
}