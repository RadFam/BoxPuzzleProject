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

		PlayerController currPlayer;
		GameObject currBox;
		GameObject currTarget;
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

			moveSpeed = 1.0f;
			walkDistance = 1;

			boxesPool = new Dictionary<GameObject, BoxController>();
			targetsPool = new Dictionary<GameObject, TargetController>();
			savedSteps = new Stack<SaveData>();
		}

		void Start()
		{
			//fieldManager = GetComponent<FieldManager>();

			// Probably we don`t need this steps
			boxesPool.Clear();
			targetsPool.Clear();
			savedSteps.Clear();
		}

		public void CorrectPosition(GameObject go)
		{
			// Correct position with filedManager

		}

		public void AddBoxToPool(GameObject go, BoxController bc)
		{
			CorrectPosition(go);
			boxesPool.Add(go, bc);
		}

		public void AddTargetToPool(GameObject go, TargetController tc)
		{
			CorrectPosition(go);
			targetsPool.Add(go, tc);

			LevelManager.inst.lvlMaxScore += 1;
		}

		public bool CanPlayerMoveFurther(Vector2Int dir, Vector3 coords)
		{
			Vector3 nextCoords = coords + new Vector3Int(dir.x, 0, dir.y) * walkDistance;

			GameObject box = null;
			foreach (var item in boxesPool)
			{
				if (item.Key.transform.position == nextCoords)
				{
					box = item.Key;
					break;
				}
			}
			if (box != null)
			{
				bool check = CanBoxMoveFurther(dir, box.transform.position);
				if (check)
				{
					boxesPool[box].moveVector = dir;
					boxesPool[box].StartMove();
				}
				return check;
			}

			return fieldManager.IsPassible(nextCoords); 
		}

		public bool CanBoxMoveFurther(Vector2Int dir, Vector3 coords)
		{
			Vector3 nextCoords = coords + new Vector3Int(dir.x, 0, dir.y) * walkDistance;

			GameObject box = null;
			foreach (var item in boxesPool)
			{
				if (item.Key.transform.position == nextCoords)
				{
					return false;
				}
			}

			return fieldManager.IsPassible(nextCoords);
		}

		public void GetTarget()
		{
			LevelManager.inst.ChangeLevelScore(1);
		}

		public void LeaveTarget()
		{
			LevelManager.inst.ChangeLevelScore(-1);
		}

		// WE NEED TO MAKE SOME INVESTIGATION ABOUNT BINDING REFERENCES OF OBJECTS WITH DICTIONARY(!!!!)
		public void MakeSaveStep()
		{
			SaveData dat = new SaveData(currPlayer.transform.position, currBox, currTarget, LevelManager.inst.lvlScore);
			savedSteps.Push(dat);
			currBox = null;
			currTarget = null;
		}

		public void RestorePrevoiusStep()
		{
			SaveData dat = savedSteps.Pop();

			currPlayer.transform.position = dat.player;
			LevelManager.inst.lvlScore = dat.score; // Need to invoke in LevelManager change scrore, maybe in getter-setter
			if (dat.box != null)
			{

			}
			if (dat.target != null)
			{
				
			}
		}
    }

	class SaveData
	{
		public Vector3 player {get; private set;}
		public GameObject box {get; private set;}
		public GameObject target {get; private set;}
		public int score {get; private set;}
		public SaveData(Vector3 pl, GameObject bx, GameObject tgt, int val)
		{
			player = pl;
			box = bx;
			target = tgt;
			score = val;
		}
	}
}