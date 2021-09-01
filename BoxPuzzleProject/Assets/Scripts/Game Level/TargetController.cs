using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayControls
{
    public class TargetController : MonoBehaviour
    {
		public bool reachStatus;
		MeshRenderer meshRenderer;
        TargetStarController targetStarController;

        // Use this for initialization
        void Start()
        {
			reachStatus = false;
        }

        public void OnReachTarget()
        {
            targetStarController.ReachTarget();
        }

        public void OnDeReachTarget()
        {
            targetStarController.gameObject.SetActive(true);
        }
    }
}