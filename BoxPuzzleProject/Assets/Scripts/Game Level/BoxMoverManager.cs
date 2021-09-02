using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PlayControls
{
    public class BoxMoverManager : MonoBehaviour
    {
		public static BoxMoverManager inst;

		public int walkDistance {get; private set;}
		public float moveSpeed {get; private set;}

		public Dictionary<GameObject, BoxController> boxesPool;
		public Dictionary<GameObject, TargetController> targetsPool;

		[SerializeField]
		FieldManager fieldManager;
		Stack<SaveData> savedSteps;

		public PlayerController currPlayer;
		GameObject currBox;
		GameObject currTarget;
		GameObject lastTarget;
		int currScore;
        
        void Awake()
		{
			if (inst == null)
			{
				inst = this;
			}
			else
			{
				Destroy(this.gameObject);
			}

			moveSpeed = 0.5f;
			walkDistance = 1;

			boxesPool = new Dictionary<GameObject, BoxController>();
			targetsPool = new Dictionary<GameObject, TargetController>();
			savedSteps = new Stack<SaveData>();

			currBox = null;
			currTarget = null;
			lastTarget = null;
		}

		void Start()
		{
			//fieldManager = GetComponent<FieldManager>();

			// Probably we don`t need this steps
			
			/*
			boxesPool.Clear();
			targetsPool.Clear();
			savedSteps.Clear();
			*/
		}

		public void CorrectPosition(GameObject go)
		{
			go.transform.position = fieldManager.GetAdaptedCoordinates(go.transform.position);

		}

		public void AddBoxToPool(GameObject go, BoxController bc)
		{
			boxesPool.Add(go, bc);
		}

		public void AddTargetToPool(GameObject go, TargetController tc)
		{
			//CorrectPosition(go);
			targetsPool.Add(go, tc);

			LevelManager.inst.lvlMaxScore += 1;
		}

		public bool CanPlayerMoveFurther(Vector2Int dir, Vector3 coords, out int push)
		{
			Vector3 nextCoords = coords + new Vector3Int(dir.x, 0, dir.y) * walkDistance;
			//Debug.Log("Next coords: " + nextCoords + "  dictCount: " + boxesPool.Count);

			GameObject box = null;
			foreach (KeyValuePair<GameObject, BoxController> item in boxesPool)
			{
				//Debug.Log("box item: " + item);
				if (item.Key.transform.position.x == nextCoords.x && item.Key.transform.position.z == nextCoords.z)
				{
					box = item.Key;
					break;
				}
			}
			currBox = box;
			if (currBox != null)
			{
				Debug.Log("currBox.transform.position: " + currBox.transform.position);
			}
			MakeSaveStep(); // (!!!!) HERE WE SAVE STEP DATA (!!!!)
			if (box != null)
			{
				bool check = CanBoxMoveFurther(dir, box.transform.position);
				if (check)
				{
					boxesPool[box].moveVector = dir;
					boxesPool[box].StartMove();
				}
				push = 1;
				return check;
			}

			push = 0;
			return fieldManager.IsPassible(nextCoords); 
		}

		public bool CanBoxMoveFurther(Vector2Int dir, Vector3 coords)
		{
			Vector3 nextCoords = coords + new Vector3Int(dir.x, 0, dir.y) * walkDistance;

			foreach (KeyValuePair<GameObject, BoxController> item in boxesPool)
			{
				if (item.Key.transform.position.x == nextCoords.x && item.Key.transform.position.z == nextCoords.z)
				{
					return false;
				}
			}

			return fieldManager.IsPassible(nextCoords);
		}

		public bool ReachDestination(Vector3 coords)
		{
			bool res =  fieldManager.IsTarget(coords);

			lastTarget = currTarget;
            if (res)
            {
                foreach (KeyValuePair<GameObject, TargetController> item in targetsPool)
                {
                    if (item.Key.transform.position.x == coords.x && item.Key.transform.position.z == coords.z)
                    {
                        currTarget = item.Key;
                        break;
                    }
                }
            }
			else
			{
				currTarget = null;
			}

			return res;
		}
		public void GetTarget()
		{
			targetsPool[currTarget].OnReachTarget(); // need lastTarget(!)
			LevelManager.inst.ChangeLevelScore(1);
		}

		public void LeaveTarget()
		{
			targetsPool[lastTarget].OnDeReachTarget();
			LevelManager.inst.ChangeLevelScore(-1);
		}

		public void SwapTargets()
		{
			targetsPool[lastTarget].OnDeReachTarget();
			targetsPool[currTarget].OnReachTarget();
		}

		//
		//
		// ................................
		// WE NEED TO MAKE SOME INVESTIGATION ABOUNT BINDING REFERENCES OF OBJECTS WITH DICTIONARY(!!!!)
		public void MakeSaveStep()
		{
			bool stat_1 = false;
			bool stat_2 = false;
			if (currBox != null)
			{
				Debug.Log("Saved box: " + currBox.transform.position);
			}
			if (lastTarget != null)
			{
				stat_1 = targetsPool[lastTarget].reachStatus;
			}
			if (currTarget != null)
			{
				stat_2 = targetsPool[currTarget].reachStatus;
			}
			SaveData dat = new SaveData(currPlayer.transform.position, currBox, lastTarget, stat_1, currTarget, stat_2, LevelManager.inst.lvlScore);
			savedSteps.Push(dat);
		}

		public void RestorePrevoiusStep()
		{
			SaveData dat = savedSteps.Pop();

			currPlayer.transform.position = dat.player;
			LevelManager.inst.lvlScore = dat.score; // Need to invoke in LevelManager change scrore, maybe in getter-setter
			if (dat.box != null)
			{
				Debug.Log("dat.box.transform.position" + dat.box.transform.position);
				Debug.Log("boxesPool[dat.box]: " + boxesPool[dat.box]);
			}
			if (dat.oldTarget != null)
			{
				
			}
			if (dat.newTarget != null)
			{
				
			}
		}
    }

	class SaveData
	{
		public Vector3 player {get; private set;}
		public GameObject box {get; private set;}
		public GameObject oldTarget {get; private set;}
		public bool statusOT {get; private set;}
		public GameObject newTarget {get; private set;}
		public bool statusNT {get; private set;}
		public int score {get; private set;}
		
		// THIS SAME METHOD DOES NOT MAKE GOOD COPIES OF GAMEOBJECTS
		public SaveData(Vector3 pl, GameObject bx, GameObject tgt, bool val_1, GameObject tgt_n, bool val_2, int val)
		{
			player = pl;
			box = bx;
			oldTarget = tgt;
			newTarget = tgt_n;
			statusOT = val_1;
			statusNT = val_2;
			score = val;
		}
	}
}