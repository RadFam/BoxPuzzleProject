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
			locRots = new List<Vector3>{new Vector3(0,-90,0), new Vector3(0,180,0), new Vector3(0,0,0), new Vector3(0,90,0)};
			currentDir = PlayerDirects.Right;
			prevDir = currentDir;
			
			playerSpeed = BoxMoverManager.inst.moveSpeed;
			BoxMoverManager.inst.CorrectPosition(gameObject);

			playerAnim.SetBool("isIdle", true);
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
			uiMoveController.LeftButton.onClick.AddListener(MoveLeft);
			uiMoveController.DownButton.onClick.AddListener(MoveDown);
			uiMoveController.UpButton.onClick.AddListener(MoveUp);
			uiMoveController.RightButton.onClick.AddListener(MoveRight);
		}
		void CheckInputs()
		{
			if (!walkingState)
			{
				if (Input.GetKeyUp(KeyCode.W))
				{
					MoveUp();
				}
				if (Input.GetKeyUp(KeyCode.A))
				{
					MoveLeft();
				}
				if (Input.GetKeyUp(KeyCode.S))
				{
					MoveDown();
				}
				if (Input.GetKeyUp(KeyCode.D))
				{
					MoveRight();
				}
			}
		}

		void MoveUp()
		{
			currentDir = PlayerDirects.Up;
			CheckForWalking();
		}

		void MoveDown()
		{
			currentDir = PlayerDirects.Down;
			CheckForWalking();
		}

		void MoveLeft()
		{
			currentDir = PlayerDirects.Left;
			CheckForWalking();
		}

		void MoveRight()
		{
			currentDir = PlayerDirects.Right;
			CheckForWalking();
		}

		void CheckForWalking()
		{
			moveVector = dirVectors[(int)currentDir];
			RotateCheck();
			if(BoxMoverManager.inst.CanPlayerMoveFurther(moveVector, transform.position))
			{
				playerAnim.SetBool("isIdle", false);
				playerAnim.SetBool("isWalk", true); // Here we need to know if we simply walking, or pushing box ahead (!)
				nextPosF = transform.position + new Vector3(moveVector.x, 0, moveVector.y) * BoxMoverManager.inst.walkDistance;
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
					playerAnim.SetBool("isWalk", false);
					playerAnim.SetBool("isIdle", true);
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