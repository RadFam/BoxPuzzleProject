using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayControls
{
    public class BoxController : MonoBehaviour
    {

        public Vector2Int nextPos{get; set;}
        public Vector2Int moveVector{get; set;}

		[SerializeField]
		float deltaMoveDist;
        Vector3 nextPosF;
        bool walkingState = false;
        bool getTarget = false;
        float boxSpeed;

        void Start()
        {
			BoxMoverManager.inst.AddBoxToPool(gameObject, this);
			boxSpeed = BoxMoverManager.inst.moveSpeed;
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
					/*
                    bool reach = GameManager.gmInst.ReachDestination(transform.position);
                    if (reach && !getTarget)
                    {
                        GameManager.gmInst.GetTarget();
                    }
                    if (!reach && getTarget)
                    {
                        GameManager.gmInst.LeaveTarget();
                    }
                    getTarget = reach;
					*/
                }
            }
		}

		public void StartMove()
		{
			nextPosF = transform.position + new Vector3(moveVector.x, moveVector.y, 0) * BoxMoverManager.inst.walkDistance;
            walkingState = true;
		}
    }
}