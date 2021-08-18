using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UIControls;

namespace PlayControls
{
    public class PlayerController : MonoBehaviour
    {
		[SerializeField]
		float deltaMoveDist;
		Vector2Int moveVector;
		Animator playerAnim;
		float playerSpeed;
		List<Vector2Int> dirVectors;
		List<Vector3> locRots;
		Vector2Int nextPos;
		Vector3 nextPosF;
        bool walkingState = false;
		PlayerDirects currentDir;
		PlayerDirects prevDir;
		enum PlayerDirects {Down, Left, Right, Up};
		UIMoveController uiMoveController;
        // Use this for initialization
        void Start()
        {
			playerAnim = GetComponent<Animator>();
			dirVectors = new List<Vector2Int>{new Vector2Int(0, -1), new Vector2Int(-1, 0), new Vector2Int(1, 0), new Vector2Int(0, 1)};
			locRots = new List<Vector3>{new Vector3(0,0,0), new Vector3(0,90,0), new Vector3(0,-90,0), new Vector3(0,180,0)};
			currentDir = PlayerDirects.Down;
			prevDir = currentDir;
			
			playerSpeed = BoxMoverManager.inst.moveSpeed;
			BoxMoverManager.inst.CorrectPosition(gameObject);
        }

        // Update is called once per frame
        void Update()
        {
			CheckInputs();
			WalkCycle();	
        }

		public void SetUIMoves(UIMoveController uiMC)
		{
			uiMoveController = uiMC;
			uiMoveController.LeftButton.onClick.AddListener();
			uiMoveController.DownButton.onClick.AddListener();
			uiMoveController.UpButton.onClick.AddListener();
			uiMoveController.RightButton.onClick.AddListener();
		}
		void CheckInputs()
		{
			if (!walkingState)
			{
				if (Input.GetKeyUp(KeyCode.W))
				{
					currentDir = PlayerDirects.Up;
					// Here must be rotation of person (!)

					CheckForWalking();
				}
				if (Input.GetKeyUp(KeyCode.A))
				{
					currentDir = PlayerDirects.Left;
					// Here must be rotation of person (!)

					CheckForWalking();
				}
				if (Input.GetKeyUp(KeyCode.S))
				{
					currentDir = PlayerDirects.Down;
					CheckForWalking();
				}
				if (Input.GetKeyUp(KeyCode.D))
				{
					currentDir = PlayerDirects.Right;
					CheckForWalking();
				}
			}
		}

		void CheckForWalking()
		{
			moveVector = dirVectors[(int)currentDir];
			RotateCheck();
			if(BoxMoverManager.inst.CanPlayerMoveFurther(moveVector, transform.position))
			{
				playerAnim.SetTrigger("startWalk"); // Here we need to know if we simply walking, or pushing box ahead (!)
				nextPosF = transform.position + new Vector3(moveVector.x, moveVector.y, 0) * BoxMoverManager.inst.walkDistance;
				walkingState = true;
			}
		}

		void WalkCycle()
		{
			if (walkingState)
            {
                float step = playerSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, nextPosF, step);

                if (Vector3.Distance(transform.position, nextPosF) < deltaMoveDist)
                {
					transform.position = nextPosF;
					walkingState = false;
					playerAnim.SetTrigger("startIdle");
                }
            }
		}

		void RotateCheck()
		{
			if (prevDir != currentDir)
			{
				Quaternion rots = new Quaternion();
				rots.eulerAngles = locRots[(int)currentDir];
				transform.rotation = rots;
				prevDir = currentDir;
			}
		}
    }
}