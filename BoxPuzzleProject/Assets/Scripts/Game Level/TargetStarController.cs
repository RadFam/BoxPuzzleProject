using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayControls
{
    public class TargetStarController : MonoBehaviour
    {
		bool reachStatus;
		[SerializeField]
		Vector3 initPos;
		[SerializeField]
		Animator myAnim;
        // Use this for initialization
        void Start()
        {
			reachStatus = false;
			transform.localPosition = initPos;
			myAnim.enabled = true;
        }

		void OnEnable()
		{
			reachStatus = false;
			transform.localPosition = initPos;
			myAnim.enabled = true;
			myAnim.SetTrigger("isRotating");
		}

		public void ReachTarget()
		{
			reachStatus = true;
			myAnim.SetTrigger("isTaken");
		}

		public void OnFadeOut()
		{	transform.localPosition = initPos;
			myAnim.enabled = false;
			gameObject.SetActive(false);

		}
    }
}