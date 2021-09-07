using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GeneratorControls;

namespace PlayControls
{
    public class BoxController : MonoBehaviour
    {

        public Vector2Int nextPos{get; set;}
        public Vector2Int moveVector{get; set;}
		float deltaMoveDist;
        Vector3 nextPosF;
        bool walkingState = false;
        bool getTarget = false;
        float boxSpeed;

        void Start()
        {
			BoxMoverManager.inst.AddBoxToPool(gameObject, this);
			boxSpeed = BoxMoverManager.inst.moveSpeed;
            deltaMoveDist = 0.01f;
        }

        // Update is called once per frame
        void Update()
        {
			MoveCycle();
        }

		void MoveCycle()
		{
			if (walkingState)
            {
                float step = boxSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, nextPosF, step);

                if (Vector3.Distance(transform.position, nextPosF) < deltaMoveDist)
                { 
                    transform.position = nextPosF;
					walkingState = false;

                    // Check if we reach or leave any of target places
					
                    bool reach = BoxMoverManager.inst.ReachDestination(transform.position);

                    // Temporary (!!)
                    // BoxMoverManager.inst.RestorePrevoiusStep();
                    // (!!)

                    if (reach && !getTarget)
                    {
                        BoxMoverManager.inst.GetTarget();
                    }
                    if (!reach && getTarget)
                    {
                        BoxMoverManager.inst.LeaveTarget();
                    }
                    if (reach && getTarget)
                    {
                        BoxMoverManager.inst.SwapTargets();
                    }
                    getTarget = reach;
                }
            }
		}

		public void StartMove()
		{
			nextPosF = transform.position + new Vector3(moveVector.x, 0, moveVector.y) * BoxMoverManager.inst.walkDistance;
            //Debug.Log("currPos: " + transform.position);
            //Debug.Log("nextPos: " + nextPosF);
            walkingState = true;
		}

        public int GetID()
        {
            return gameObject.GetComponent<GridObject>().throughNumber;
        }
    }
}