using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneratorControls;

namespace PlayControls
{
    public class TargetController : MonoBehaviour
    {
		public bool reachStatus;
		MeshRenderer meshRenderer;
        TargetStarController targetStarController;

        public GridObject selfGrid;

        // Use this for initialization
        void Start()
        {
			reachStatus = false;
            meshRenderer = GetComponent<MeshRenderer>();
            targetStarController = transform.GetChild(0).GetComponent<TargetStarController>();

            BoxMoverManager.inst.AddTargetToPool(gameObject, this);
        }

        public void OnReachTarget()
        {
            targetStarController.ReachTarget();            

            // Change self material
            meshRenderer.material = selfGrid.targetObjects.targetTexR[selfGrid.objectSubType];
            reachStatus = true;
        }

        public void OnDeReachTarget()
        {
            targetStarController.gameObject.SetActive(true);

            // Change self material
            meshRenderer.material = selfGrid.targetObjects.targetTex[selfGrid.objectSubType];
            reachStatus = false;
        }
    }
}